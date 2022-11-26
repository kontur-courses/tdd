namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    public List<Rectangle> GenerateCloud(Point center, List<Size> rectangleSizes)
    {
        var requestedList = new List<Rectangle>();
        foreach (var rectangleSize in rectangleSizes)
        {
            requestedList.Add(GetNextRectangle(center, requestedList, rectangleSize));
        }

        return requestedList;
    }

    public Rectangle GetNextRectangle(Point center, List<Rectangle> rectangles, Size rectangleSize)
    {
        if (center.X <= 0 || center.Y <= 0) 
            throw new ArgumentException("Center position much be positive");
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("Height and Width much be positive");
        Rectangle newRectangle;
        if (!TryToPutRectangleInsideSpiral(center, rectangles, rectangleSize, out newRectangle))
            throw new ArgumentException("Rectangle size so much");
        return newRectangle;
    }

    private bool TryToPutRectangleInsideSpiral(Point center, List<Rectangle> rectangles, Size rectangleSize,
        out Rectangle newRectangle)
    {
        var x = 0;
        var y = 0;
        double angle = 0;
        var rectPos = new Point(x - rectangleSize.Width / 2 + center.X,
            y - rectangleSize.Height / 2 + center.Y);
        var rectangle = new Rectangle(rectPos, rectangleSize);
        newRectangle = rectangle;
        var stepLength = (int)(7 / (2 * Math.PI));
        int count = 0;
        while (count < 100000)
        {
            newRectangle = rectangle;
            if (rectangles.Count == 0) return true;
            var isContain = rectangles.Any(x => x.Contains(rectangle.GetCenter()));
            var isIntersect = rectangles.Any(x => x.IntersectsWith(rectangle));
            if (!isIntersect && !isContain)
            {
                newRectangle = rectangle;
                return true;
            }

            angle += Math.PI / 180 * (Math.Min(rectangleSize.Height, rectangleSize.Width) / 3);
            x = (int)(angle * Math.Cos(angle) * stepLength);
            y = (int)(angle * Math.Sin(angle) * stepLength);
            rectPos = new Point(x - rectangleSize.Width / 2 + center.X,
                y - rectangleSize.Height / 2 + center.Y);
            rectangle = new Rectangle(rectPos, rectangleSize);
            count++;
        }

        return false;
    }
}