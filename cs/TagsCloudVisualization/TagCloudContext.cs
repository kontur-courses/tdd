using System.Drawing;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization
{
    public class TagCloudContext
    {
        public readonly string ImageName;
        public readonly Size ImageSize;
        public readonly string[] TagCloudContent;
        public readonly TagCloud Cloud;
        public readonly ICloudLayouter CloudLayouter;

        public TagCloudContext(string imageName, Size imageSize, string[] tagCloudContent, TagCloud cloud,
                               ICloudLayouter cloudLayouter)
        {
            ImageName = imageName;
            ImageSize = imageSize;
            TagCloudContent = tagCloudContent;
            Cloud = cloud;
            CloudLayouter = cloudLayouter;
        }
    }
}