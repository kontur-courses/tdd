using System.Drawing;
using TagsCloudVisualization;

class CircularCloudLayouter(Point centerPoint)
{
    private ArchimedeanSpiral spiral = new(centerPoint);

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("rectangleSize with zero or negative height or width is prohibited!");
        throw new NotImplementedException();
    }
}
