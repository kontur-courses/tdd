namespace CircularCloudLayouter;

public class WeightedSegmentsCollection
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

    public int Length =>
        End - Start;

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
}