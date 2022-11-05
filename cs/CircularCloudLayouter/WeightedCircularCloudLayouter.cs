using System.Drawing;

namespace CircularCloudLayouter;

public class WeightedCircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point _center;

    private readonly WeightedSegmentsHelper _rightWeights = new();
    private readonly WeightedSegmentsHelper _topWeights = new();
    private readonly WeightedSegmentsHelper _leftWeights = new();
    private readonly WeightedSegmentsHelper _bottomWeights = new();

    public WeightedCircularCloudLayouter(Point center)
    {
        _center = center;
    }

    public Rectangle PutNextRectangle(Size rectSize)
    {
        var rect = _topWeights.Segments.Count == 0
            ? CreateRectangle(_center, rectSize)
            : FindBestRectangle(rectSize);
        UpdateWeights(rect);

        return rect;
    }

    private Rectangle FindBestRectangle(Size rectSize)
    {
        var rightAvg = _rightWeights.AverageWeight;
        var topAvg = _topWeights.AverageWeight;
        var leftAvg = _leftWeights.AverageWeight;
        var bottomAvg = _bottomWeights.AverageWeight;

        if (rightAvg < topAvg && rightAvg < leftAvg && rightAvg < bottomAvg)
        {
            var bestPlace = FindBestPlace(_rightWeights, rectSize.Height, _center.Y);
            return new Rectangle(
                _center.X + bestPlace.Weight + 2,
                bestPlace.Start - 2,
                rectSize.Width, rectSize.Height
            );
        }

        if (topAvg < leftAvg && topAvg < bottomAvg)
        {
            var bestPlace = FindBestPlace(_topWeights, rectSize.Width, _center.X);
            return new Rectangle(
                bestPlace.Start - 2,
                _center.Y - bestPlace.Weight - rectSize.Height - 2,
                rectSize.Width, rectSize.Height
            );
        }

        if (leftAvg < bottomAvg)
        {
            var bestPlace = FindBestPlace(_leftWeights, rectSize.Height, _center.Y);
            return new Rectangle(
                _center.X - bestPlace.Weight - rectSize.Width - 2,
                bestPlace.Start - 2,
                rectSize.Width, rectSize.Height
            );
        }

        {
            var bestPlace = FindBestPlace(_bottomWeights, rectSize.Width, _center.X);
            return new Rectangle(
                bestPlace.Start - 2,
                _center.Y + bestPlace.Weight + 2,
                rectSize.Width, rectSize.Height
            );
        }
    }

    public static WeightedSegment FindBestPlace(WeightedSegmentsHelper weights, int sideLength, int middle)
    {
        sideLength += 4;
        var offset = (int) Math.Floor(0.8 * sideLength);
        var mergedSegments = new Queue<WeightedSegment>();
        mergedSegments.Enqueue(new WeightedSegment(weights.Start - offset, weights.Start));
        var mergedWeight = 0;
        var mergedLength = offset;
        var bestDistToCenter = double.MaxValue;
        var bestWeightedSegment = new WeightedSegment(0, 0, int.MaxValue);

        foreach (var segment in weights.Segments.Concat(new[] {new WeightedSegment(weights.End, weights.End + offset)}))
        {
            mergedSegments.Enqueue(segment);
            mergedLength += segment.Length;
            mergedWeight = Math.Max(mergedWeight, segment.Weight);
            if (mergedLength - mergedSegments.Peek().Length > sideLength)
            {
                mergedLength -= mergedSegments.Dequeue().Length;
                mergedWeight = mergedSegments.Max(sgm => sgm.Weight);
            }

            if (mergedLength < sideLength)
                continue;
            if (mergedWeight > bestWeightedSegment.Weight)
                continue;

            var start = segment.End - sideLength;
            if (start + sideLength / 2 > middle)
                start = Math.Max(mergedSegments.Peek().Start, middle - sideLength / 2);

            var merged = new WeightedSegment(start, start + sideLength, mergedWeight);
            var distToCenter = Math.Abs(
                middle - (merged.Start + (merged.End - merged.Start) / 2d)
            );
            if (merged.Weight == bestWeightedSegment.Weight && distToCenter > bestDistToCenter)
                continue;

            bestWeightedSegment = merged;
            bestDistToCenter = distToCenter;
        }

        bestWeightedSegment.Start += 4;
        return bestWeightedSegment;
    }

    private void UpdateWeights(Rectangle rect)
    {
        if (rect.Right > _center.X)
            _rightWeights.UpdateGreaterWeights(
                new WeightedSegment(rect.Top, rect.Bottom, rect.Right - _center.X));

        if (rect.Top < _center.Y)
            _topWeights.UpdateGreaterWeights(
                new WeightedSegment(rect.Left, rect.Right, _center.Y - rect.Top));

        if (rect.Left < _center.X)
            _leftWeights.UpdateGreaterWeights(
                new WeightedSegment(rect.Top, rect.Bottom, _center.X - rect.Left));

        if (rect.Bottom > _center.Y)
            _bottomWeights.UpdateGreaterWeights(
                new WeightedSegment(rect.Left, rect.Right, rect.Bottom - _center.Y));
    }

    private static Rectangle CreateRectangle(Point rectCenter, Size rectSize) =>
        new(rectCenter - rectSize / 2, rectSize);
}

public class WeightedSegmentsHelper
{
    private readonly LinkedList<WeightedSegment> _segments = new();
    public IReadOnlyCollection<WeightedSegment> Segments => _segments;

    public int Start =>
        _segments.Count > 0
            ? _segments.First!.Value.Start
            : throw new InvalidOperationException("No segments added!");

    public int End =>
        _segments.Count > 0
            ? _segments.Last!.Value.End
            : throw new InvalidOperationException("No segments added!");

    public double AverageWeight =>
        _segments.Count > 0
            ? _segments.Average(segment => segment.FullWeight)
            : throw new InvalidOperationException("No segments added!");


    public void UpdateGreaterWeights(WeightedSegment newSegment)
    {
        if (_segments.Count == 0)
        {
            _segments.AddFirst(newSegment);
        }
        else
        {
            var intersections = GetNodesBetween(newSegment.Start, newSegment.End).ToList();
            if (intersections.Count == 0)
                InsertFirstOrLast(newSegment);
            else
                InsertInMiddle(newSegment, intersections);
        }
    }

    private IEnumerable<LinkedListNode<WeightedSegment>> GetNodesBetween(int start, int end)
    {
        var current = _segments.First;
        while (current != null && current.Value.End <= start)
            current = current.Next;
        while (current != null && current.Value.Start < end)
        {
            yield return current;
            current = current.Next;
        }
    }

    private void InsertFirstOrLast(WeightedSegment newSegment)
    {
        var first = _segments.First!;
        if (newSegment.End <= first.Value.Start)
        {
            if (newSegment.End < first.Value.Start)
                _segments.AddFirst(new WeightedSegment(newSegment.End, first.Value.Start));
            _segments.AddFirst(newSegment);
        }
        else
        {
            var last = _segments.Last!;
            if (newSegment.Start > last.Value.End)
                _segments.AddLast(new WeightedSegment(last.Value.End, newSegment.Start));
            _segments.AddLast(newSegment);
        }
    }

    private void InsertInMiddle(WeightedSegment newSegment, List<LinkedListNode<WeightedSegment>> intersections)
    {
        var first = intersections.First();
        var added = newSegment.Start > first.Value.Start
            ? _segments.AddAfter(first, newSegment)
            : _segments.AddBefore(first, newSegment);

        foreach (var node in intersections)
        {
            if (added.Value.Weight > node.Value.Weight)
            {
                var cutParts = node.Value.Cut(added.Value);
                if (cutParts.Length == 0)
                {
                    _segments.Remove(node);
                }
                else
                {
                    node.Value = cutParts[0];
                    if (cutParts.Length == 2)
                    {
                        _segments.AddAfter(added, cutParts[1]);
                    }
                }
            }
            else
            {
                var cutParts = added.Value.Cut(node.Value);
                if (cutParts.Length == 0)
                {
                    _segments.Remove(added);
                }
                else
                {
                    added.Value = cutParts[0];
                    if (added.Value.Start == node.Value.End)
                    {
                        _segments.Remove(added);
                        added = _segments.AddAfter(node, cutParts[0]);
                    }

                    if (cutParts.Length == 2)
                    {
                        added = _segments.AddAfter(node, cutParts[1]);
                    }
                }
            }
        }
    }
}

public class WeightedSegment
{
    private int _start;

    public int Start
    {
        get => _start;
        set
        {
            if (value > End)
                throw new ArgumentException("Start cannot be less than end!");
            _start = value;
        }
    }

    private int _end;

    public int End
    {
        get => _end;
        set
        {
            if (Start > value)
                throw new ArgumentException("Start cannot be less than end!");
            _end = value;
        }
    }

    public int Weight { get; set; }

    public int Length => End - Start;
    public int FullWeight => Length * Weight;

    public WeightedSegment(int start, int end, int weight = 0)
    {
        if (start > end)
            throw new ArgumentException("Start cannot be less than end!");
        _start = start;
        _end = end;
        Weight = weight;
    }

    public bool Contains(int value) => Start <= value && value <= End;

    public WeightedSegment[] Cut(WeightedSegment segment)
    {
        if (segment.Start > Start && segment.End < End)
            return new[]
            {
                new WeightedSegment(Start, segment.Start, Weight),
                new WeightedSegment(segment.End, End, Weight)
            };

        var start = segment.Contains(Start) ? segment.End : Start;
        var end = segment.Contains(End) ? segment.Start : End;

        return start >= end
            ? Array.Empty<WeightedSegment>()
            : new[] {new WeightedSegment(start, end, Weight)};
    }
}