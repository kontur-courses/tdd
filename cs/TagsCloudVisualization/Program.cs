using System;
using System.Linq;
using TagsCloudVisualization.CloudFactories;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    static class Program
    {
        private const string WebCloudFilename = "WebTagCloud.bmp";
        private const string CommonWordsCloudFilename = "CommonWordsTagCloud.bmp";

        private static readonly TagCloudContext[] cloudFactoryByImageName =
        {
            new TagCloudContext(WebCloudFilename, TagCloudContent.WebCloudStrings, new WebCloudFactory()),
            new TagCloudContext(CommonWordsCloudFilename, TagCloudContent.CommonWordsCloudStrings,
                                new CommonWordsCloudFactory())
        };

        private static void Main()
        {
            foreach (var cloudContext in cloudFactoryByImageName)
                CreateTagCloudImage(cloudContext);
        }

        private static void CreateTagCloudImage(TagCloudContext cloudContext)
        {
            var shuffledContentStrings = cloudContext.TagCloudContent.Take(1)
                                                     .Concat(cloudContext.TagCloudContent.Skip(1)
                                                                         .SequenceShuffle(new Random()))
                                                     .ToArray();

            var bitmap = TagCloudBitmapCreator.CreateBitmap(shuffledContentStrings,
                                                            cloudContext.CircularCloudLayouter,
                                                            cloudContext.CloudFactory);
            bitmap.Save(cloudContext.ImageName);
        }
    }
}