using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly RectangleLayouter rectangleLayouter;

    public CircularCloudLayouter(Point center)
    {
        rectangleLayouter = new RectangleLayouter(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return rectangleLayouter.PutNextRectangle(rectangleSize);
    }

    public List<Rectangle> GetRectangles()
    {
        return rectangleLayouter.GetRectangles();
    }
}