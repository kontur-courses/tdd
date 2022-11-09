namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private Point center;
    private int radius;
    private List<Rectangle> rectangles = new();
    public CircularCloudLayouter(Point center)
    {
        if (center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("X and Y much be positive");
        this.center = center;
        radius = Math.Min(center.X, center.Y);
    }
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("Height and Width much be positive");
        Rectangle buffer = new(); 
        if (!TryToPutRectangleInsideSpiral(rectangleSize, out buffer))
            throw new ArgumentException("Rectangle size so much");
        rectangles.Add(buffer);
        return buffer;
    }

    private bool TryToPutRectangleInsideSpiral(Size rectangleSize, out Rectangle buffer)
    {
        int x, y = x = 0;
        double angel = 0;
        buffer = new Rectangle();
        while (GetDiagonal(x, y) < radius)
        {
            var rectPos = new Point(x - rectangleSize.Width / 2 + center.X,
                y - rectangleSize.Height / 2 + center.Y);
            buffer = new Rectangle(rectPos, rectangleSize);
            var stepLength = GetStepLengthToSpiral(7);
            angel += Math.PI/180;
            x = (int)(angel * Math.Cos(angel) * stepLength);
            y = (int)(angel * Math.Sin(angel) * stepLength);
            if (rectangleSize.GetDiagonal() > 2 * radius) return false;
            if (rectangles.Count == 0) return true;
            if (CheckIntersect(buffer) && CheckInsideRectangles(buffer.GetCenter()))
            {
                
                return true;
            }
        }
        return false;
    }

    private void CorrectPosition(Rectangle buffer)
    {
        var horizontalOffset = true;
        var verticalOffset = true;
        int count = 0;
        while ((horizontalOffset || verticalOffset) && count < 1000)
        {
            horizontalOffset = buffer.GetCenter().X < center.X
                ? TryToOffset(1, 0, buffer)
                : TryToOffset(-1, 0, buffer);
            verticalOffset = buffer.GetCenter().Y < center.Y
                ? TryToOffset(0, 1, buffer)
                : TryToOffset(0, -1, buffer);
            count += 1;
        }
    }
    private bool TryToOffset(int x, int y, Rectangle buffer)
    {
        buffer.Offset(x, y);
        if (rectangles.Any(x => x.IntersectsWith(buffer)))
        {
            buffer.Offset(-x, -y);
            return false;
        }
        return true;
    }
    private int GetStepLengthToSpiral(int stepLength)
    {
        return (int)(stepLength / (2 * Math.PI));
    }
    private bool CheckInsideRectangles(Point point)
    {
        return !rectangles.Any(x => x.Contains(point));
    }
    private bool CheckIntersect(Rectangle rect)
    {
        return !rectangles.Any(x => x.IntersectsWith(rect));
    }
    private double GetDiagonal(int x, int y)
    {
        return Math.Sqrt(x * x + y * y);
    }
    public bool SaveAsPic(string pathFolder)
    {
        Bitmap bitmap = new Bitmap(radius * 2, radius * 2);; 
        Graphics graph = Graphics.FromImage(bitmap);
        graph.DrawRectangles(new Pen(Color.Black), rectangles.ToArray());
        bitmap.Save(pathFolder + "pics.btm");
        return true;
    }
    public bool SaveAsPic(string pathFolder, string name)
    {
        Bitmap bitmap = new Bitmap(radius * 2, radius * 2);; 
        Graphics graph = Graphics.FromImage(bitmap);
        graph.DrawRectangles(new Pen(Color.Black), rectangles.ToArray());
        bitmap.Save(pathFolder + "\\" + name);
        return true;
    }
}