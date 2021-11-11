using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public static class TagCloudVisualizationTestsHelper
    {
        public static double CalculateLayoutRadius(this CircularCloudLayouter layouter)
        {
            return layouter.Rectangles
                .Select(rectangle => rectangle.Location + rectangle.Size / 2)
                .Select(rectangleCenter => rectangleCenter.GetDistance(layouter.Center))
                .Max();
        }
    }
}
