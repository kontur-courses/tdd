using System.Drawing;
using TagsCloudVisualization.CloudFactories;
using TagsCloudVisualization.CloudLayouters;

namespace TagsCloudVisualization
{
    public class TagCloudContext
    {
        public readonly string ImageName;
        public readonly string[] TagCloudContent;
        public readonly TagCloudFactory CloudFactory;
        public readonly ICloudLayouter CircularCloudLayouter;

        public TagCloudContext(string imageName, string[] tagCloudContent, TagCloudFactory cloudFactory)
        {
            ImageName = imageName;
            TagCloudContent = tagCloudContent;
            CloudFactory = cloudFactory;

            var canvasCenter = new Point(cloudFactory.CanvasSize.Width / 2, cloudFactory.CanvasSize.Height / 2);
            CircularCloudLayouter = new CircularCloudLayouter(canvasCenter);
        }
    }
}