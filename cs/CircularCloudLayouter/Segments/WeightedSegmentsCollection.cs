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
        if (_segments.Count == 0)
        {
            _segments.AddFirst(newSegment);
        }
        else if (newSegment.End <= Start)
        {
            if (newSegment.End < Start)
                _segments.AddFirst(new WeightedSegment(newSegment.End, Start));
            _segments.AddFirst(newSegment);
        }
        else if (newSegment.Start >= End)
        {
            if (newSegment.Start > End)
                _segments.AddLast(new WeightedSegment(End, newSegment.Start));
            _segments.AddLast(newSegment);
        }
        else
        {
            InsertWithIntersectionsHandling(newSegment);
        }
    }

    private void InsertWithIntersectionsHandling(WeightedSegment newSegment)
    {
        var intersections = GetNodesIntersectedWith(newSegment).ToList();
        var added = _segments.AddBefore(intersections.First(), newSegment);

        foreach (var node in intersections)
        {
            if (newSegment.Weight < node.Value.Weight)
            {
                added.Value = added.Value.WithEnd(Math.Max(node.Value.Start, added.Value.Start));
                if (added.Value.Length == 0)
                    _segments.Remove(added);

                if (newSegment.End > node.Value.End)
                    added = _segments.AddAfter(node, newSegment.WithStart(node.Value.End));
            }
            else
            {
                if (node.Value.Start < newSegment.Start)
                    _segments.AddBefore(added, node.Value.WithEnd(added.Value.Start));

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

    public void OptimizeWeights()
    {
        var current = _segments.First;
        while (current?.Next is not null)
        {
            if (current.Previous is null)
            {
                current = current.Next;
                continue;
            }
            if (TryMaxLengthOptimization(current))
                continue;
            if (TryWeightDeltaOptimization(current))
                continue;
            current = current.Next;
        }
    }

    private bool TryMaxLengthOptimization(LinkedListNode<WeightedSegment> node)
    {
        if (node.Value.Length > _optimizationOptions.MaxLengthToRemove)
            return false;

        if (node.Previous!.Value.Weight >= node.Value.Weight && node.Previous.Value.Weight < node.Next!.Value.Weight)
        {
            CombineWithPrev(node, node.Previous.Value.Weight);
            return true;
        }

        if (node.Next!.Value.Weight >= node.Value.Weight)
        {
            CombineWithNext(node, node.Next.Value.Weight);
            return true;
        }

        return false;
    }

    private bool TryWeightDeltaOptimization(LinkedListNode<WeightedSegment> node)
    {
        var prevWeightDelta = Math.Abs(node.Value.Weight - node.Previous!.Value.Weight);
        var nextWeightDelta = Math.Abs(node.Value.Weight - node.Next!.Value.Weight);

        if (prevWeightDelta <= _optimizationOptions.MaxWeightDeltaToCombine)
        {
            CombineWithPrev(node, Math.Max(node.Value.Weight, node.Previous.Value.Weight));
            return true;
        }

        if (nextWeightDelta <= _optimizationOptions.MaxWeightDeltaToCombine)
        {
            CombineWithNext(node, Math.Max(node.Value.Weight, node.Next.Value.Weight));
            return true;
        }

        return false;
    }

    private void CombineWithPrev(LinkedListNode<WeightedSegment> node, int combinedWeight)
    {
        node.Value = new WeightedSegment(node.Previous!.Value.Start, node.Value.End, combinedWeight);
        _segments.Remove(node.Previous);
    }

    private void CombineWithNext(LinkedListNode<WeightedSegment> node, int combinedWeight)
    {
        node.Value = new WeightedSegment(node.Value.Start, node.Next!.Value.End, combinedWeight);
        _segments.Remove(node.Next);
    }
}