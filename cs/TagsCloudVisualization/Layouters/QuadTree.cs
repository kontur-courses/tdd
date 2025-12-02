using System.Drawing;

namespace TagsCloudVisualization.Layouters;

public class QuadTree
{
    private readonly List<Rectangle> elements = new();
    private readonly int bucketCapacity;
    private readonly int maxDepth;
    private readonly int level;

    private QuadTree? upperLeft;
    private QuadTree? upperRight;
    private QuadTree? bottomLeft;
    private QuadTree? bottomRight;

    private Rectangle bounds;

    public bool IsLeaf => upperLeft == null;

    public QuadTree(Rectangle bounds, int bucketCapacity = 32, int maxDepth = 5, int level = 0)
    {
        this.bounds = bounds;
        this.bucketCapacity = bucketCapacity;
        this.maxDepth = maxDepth;
        this.level = level;
    }

    public void Insert(Rectangle element)
    {
        if (!bounds.Contains(element))
            ExpandBounds(element);

        if (elements.Count >= bucketCapacity)
            Split();

        var containingChild = GetContainingChild(element);

        if (containingChild != null)
            containingChild.Insert(element);
        else
            elements.Add(element);
    }

    public bool HasIntersection(Rectangle element)
    {
        var nodes = new Queue<QuadTree>();
        nodes.Enqueue(this);

        while (nodes.Count > 0)
        {
            var node = nodes.Dequeue();

            if (!element.IntersectsWith(node.bounds))
                continue;

            foreach (var item in node.elements)
                if (item != element && element.IntersectsWith(item))
                    return true;

            if (node.IsLeaf) continue;

            if (node.upperLeft != null && element.IntersectsWith(node.upperLeft.bounds))
                nodes.Enqueue(node.upperLeft);

            if (node.upperRight != null && element.IntersectsWith(node.upperRight.bounds))
                nodes.Enqueue(node.upperRight);

            if (node.bottomLeft != null && element.IntersectsWith(node.bottomLeft.bounds))
                nodes.Enqueue(node.bottomLeft);

            if (node.bottomRight != null && element.IntersectsWith(node.bottomRight.bounds))
                nodes.Enqueue(node.bottomRight);
        }

        return false;
    }

    private void Clear()
    {
        elements.Clear();
        upperLeft = upperRight = bottomLeft = bottomRight = null;
    }

    private void ExpandBounds(Rectangle newElement)
    {
        var minX = Math.Min(bounds.X, newElement.X);
        var minY = Math.Min(bounds.Y, newElement.Y);
        var maxX = Math.Max(bounds.Right, newElement.Right);
        var maxY = Math.Max(bounds.Bottom, newElement.Bottom);

        var width = maxX - minX;
        var height = maxY - minY;

        var newWidth = width * 2;
        var newHeight = height * 2;

        var centerX = (minX + maxX) / 2;
        var centerY = (minY + maxY) / 2;

        var newBounds = new Rectangle(
            centerX - newWidth / 2,
            centerY - newHeight / 2,
            newWidth,
            newHeight);

        RebuildWithNewBounds(newBounds);
    }

    private void RebuildWithNewBounds(Rectangle newBounds)
    {
        var allElements = GetAllElements();

        Clear();

        bounds = newBounds;

        foreach (var element in allElements) Insert(element);
    }

    private List<Rectangle> GetAllElements()
    {
        var result = new List<Rectangle>(elements);

        if (!IsLeaf)
        {
            result.AddRange(upperLeft.GetAllElements());
            result.AddRange(upperRight.GetAllElements());
            result.AddRange(bottomLeft.GetAllElements());
            result.AddRange(bottomRight.GetAllElements());
        }

        return result;
    }

    private void Split()
    {
        if (!IsLeaf)
            return;

        if (level + 1 > maxDepth)
            return;

        var halfWidth = bounds.Width / 2;
        var halfHeight = bounds.Height / 2;
        var x = bounds.X;
        var y = bounds.Y;

        upperLeft = new QuadTree(
            new Rectangle(x, y, halfWidth, halfHeight),
            bucketCapacity, maxDepth, level + 1);

        upperRight = new QuadTree(
            new Rectangle(x + halfWidth, y, halfWidth, halfHeight),
            bucketCapacity, maxDepth, level + 1);

        bottomLeft = new QuadTree(
            new Rectangle(x, y + halfHeight, halfWidth, halfHeight),
            bucketCapacity, maxDepth, level + 1);

        bottomRight = new QuadTree(
            new Rectangle(x + halfWidth, y + halfHeight, halfWidth, halfHeight),
            bucketCapacity, maxDepth, level + 1);

        var elements = this.elements.ToList();

        foreach (var element in elements)
        {
            var containingChild = GetContainingChild(element);
            if (containingChild != null)
            {
                this.elements.Remove(element);
                containingChild.Insert(element);
            }
        }
    }

    private QuadTree? GetContainingChild(Rectangle element)
    {
        if (IsLeaf) return null;

        foreach (var child in new[] { upperLeft, upperRight, bottomLeft, bottomRight })
            if (child != null && child.bounds.Contains(element))
                return child;

        return null;
    }
}