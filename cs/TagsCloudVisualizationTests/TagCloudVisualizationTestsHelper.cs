using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public static class TagCloudVisualizationTestsHelper
    {
        public static double CalculateCloudRadius(this Cloud cloud)
        {
            return cloud.Rectangles
                .Select(rectangle => rectangle.Location + rectangle.Size / 2)
                .Select(rectangleCenter => rectangleCenter.GetDistance(cloud.Center))
                .Max();
        }
    }
}
