namespace CircularCloudLayouter.Segments;

public class WeightedSegmentsCollection
{
    private readonly WeightedSegmentsOptimizationOptions _optimizationOptions;
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

    public int Length =>
        End - Start;

    public WeightedSegmentsCollection() : this(WeightedSegmentsOptimizationOptions.Default)
    {
    }

    public WeightedSegmentsCollection(WeightedSegmentsOptimizationOptions optimizationOptions)
    {
        _optimizationOptions = optimizationOptions;
    }

    public int GetWeightAt(int point) =>
        _segments.Where(s => s.Start <= point && s.End >= point).Max(s => s.Weight);


    public void UpdateGreaterWeights(WeightedSegment newSegment)
    {
        if (newSegment.Length == 0)
            return;
        var modified = new List<LinkedListNode<WeightedSegment>>();
        if (_segments.Count == 0)
        {
            _segments.AddFirst(newSegment);
        }
        else if (newSegment.End <= Start)
        {
            if (newSegment.End < Start)
                modified.Add(_segments.AddFirst(new WeightedSegment(newSegment.End, Start)));
            modified.Add(_segments.AddFirst(newSegment));
        }
        else if (newSegment.Start >= End)
        {
            if (newSegment.Start > End)
                modified.Add(_segments.AddLast(new WeightedSegment(End, newSegment.Start)));
            modified.Add(_segments.AddLast(newSegment));
        }
        else
        {
            modified.AddRange(InsertWithIntersectionsHandling(newSegment));
        }

        foreach (var node in modified.Where(node => node.List == _segments))
            OptimizeNodeNeighbours(node);
    }

    private IEnumerable<LinkedListNode<WeightedSegment>> InsertWithIntersectionsHandling(WeightedSegment newSegment)
    {
        var intersections = GetNodesIntersectedWith(newSegment).ToList();
        var added = _segments.AddBefore(intersections.First(), newSegment);
        yield return added;

        foreach (var node in intersections)
        {
            if (newSegment.Weight < node.Value.Weight)
            {
                added.Value = added.Value.WithEnd(Math.Max(node.Value.Start, added.Value.Start));
                if (added.Value.Length == 0)
                    _segments.Remove(added);

                if (newSegment.End > node.Value.End)
                    yield return added = _segments.AddAfter(node, newSegment.WithStart(node.Value.End));
            }
            else
            {
                if (node.Value.Start < newSegment.Start)
                    yield return _segments.AddBefore(added, node.Value.WithEnd(added.Value.Start));

                node.Value = node.Value.WithStart(Math.Min(added.Value.End, node.Value.End));
                if (node.Value.Length == 0)
                    _segments.Remove(node);
            }
        }
    }

    private IEnumerable<LinkedListNode<WeightedSegment>> GetNodesIntersectedWith(Segment segment)
    {
        var current = _segments.First;
        while (current != null && segment.Start >= current.Value.End)
            current = current.Next;
        while (current != null && segment.End > current.Value.Start)
        {
            yield return current;
            current = current.Next;
        }
    }

    private void OptimizeNodeNeighbours(LinkedListNode<WeightedSegment> node)
    {
        OptimizePreviousNeighbours(node);
        OptimizeNextNeighbours(node);
    }

    private void OptimizeNextNeighbours(LinkedListNode<WeightedSegment> node)
    {
        while (node.Previous is not null)
        {
            var curVal = node.Value;
            var prevVal = node.Previous.Value;

            if (curVal.Weight > prevVal.Weight && prevVal.Length <= _optimizationOptions.MaxLengthToRemove)
            {
                node.Value = curVal.WithStart(prevVal.Start);
                _segments.Remove(node.Previous);
            }
            else if (Math.Abs(curVal.Weight - prevVal.Weight) <= _optimizationOptions.MaxWeightDeltaToCombine)
            {
                node.Value = new WeightedSegment(prevVal.Start, curVal.End, Math.Max(curVal.Weight, prevVal.Weight));
                _segments.Remove(node.Previous);
            }
            else
            {
                break;
            }
        }
    }

    private void OptimizePreviousNeighbours(LinkedListNode<WeightedSegment> node)
    {
        while (node.Next is not null)
        {
            var curVal = node.Value;
            var nextVal = node.Next.Value;

            if (curVal.Weight > nextVal.Weight && nextVal.Length <= _optimizationOptions.MaxLengthToRemove)
            {
                node.Value = curVal.WithEnd(nextVal.End);
                _segments.Remove(node.Next);
            }
            else if (Math.Abs(curVal.Weight - nextVal.Weight) <= _optimizationOptions.MaxWeightDeltaToCombine)
            {
                node.Value = new WeightedSegment(curVal.Start, nextVal.End, Math.Max(curVal.Weight, nextVal.Weight));
                _segments.Remove(node.Next);
            }
            else
            {
                break;
            }
        }
    }
}