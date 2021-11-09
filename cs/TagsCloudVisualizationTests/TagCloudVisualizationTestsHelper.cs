using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public static class TagCloudVisualizationTestsHelper
    {
        public static void PutManyRectangles(this CircularCloudLayouter layouter, int count)
        {
            for (var i = 0; i < count; i++)
                layouter.PutNextRectangle(new Size(50, 50));
        }

        public static double CalculateLayoutRadius(this CircularCloudLayouter layouter)
        {
            return layouter.Rectangles
                .Select(rectangle => rectangle.Location + rectangle.Size / 2)
                .Select(rectangleCenter => rectangleCenter.GetDistance(layouter.Center))
                .Max();
        }
    }
}
