using System.Drawing;

namespace TagsCloudVisualization.Tests;

internal static class TestBitmapExtensions
{
    public static Bitmap DrawFailedTestImage(
        this Bitmap bitmap,
        Rectangle[] rectangles,
        Point centerPoint,
        Point barycenterPoint,
        int maxDistanceFromBarycenter,
        TestType testType)
    {
#pragma warning disable CA1416
        bitmap.DrawRectangles(rectangles, new Pen(Color.Blue));

        if (testType != TestType.BarycenterTest)
            return bitmap;
        
        bitmap.DrawEllipse(
            new Rectangle(centerPoint.X, centerPoint.Y, maxDistanceFromBarycenter, maxDistanceFromBarycenter),
            new Pen(Color.Lime));
        bitmap.DrawEllipse(
            new Rectangle(barycenterPoint.X, barycenterPoint.Y, 1, 1),
            new Pen(Color.Red));
#pragma warning restore CA1416
        return bitmap;
    }
}