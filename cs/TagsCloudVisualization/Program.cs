using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TagsCloudVisualization.Tests")]
namespace TagsCloudVisualization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var center = new Point(400, 400);
            var minRectangleWidth = 30;
            var maxRectangleWidth = 70;
            var minRectangleHeight = 20;
            var maxRectangleHeight = 50;
            var rectanglesCount = 1000;
            var imageFileName = "circularCloudLayouter.png";

            var rectangles = new Rectangle[rectanglesCount];
            var circularCloudLayouter = new CircularCloudLayouter(center);

            for (var i = 0; i < rectanglesCount; i++)
            {
                var nextRectangleSize = CircularCloudLayouterWorker.GetNextRectangleSize(
                    minRectangleWidth,
                    maxRectangleWidth,
                    minRectangleHeight,
                    maxRectangleHeight);
                rectangles[i] = circularCloudLayouter.PutNextRectangle(nextRectangleSize);
            }

            ImageSaver.SaveFile(CircularCloudLayouterPainter.Draw(rectangles), imageFileName);
        }
    }
}