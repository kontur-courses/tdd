using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class TagCloudCreator
{
    public static TagCloud Create(IEnumerable<Size> rectangleSizes, ICloudLayouter cloudLayouter)
    {
        var newTagCloud = new TagCloud(cloudLayouter);

        foreach (var rectangleSize in rectangleSizes)
            newTagCloud.AddNextRectangleWith(rectangleSize);

        return newTagCloud;
    }
}