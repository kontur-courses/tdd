using System.Drawing;
using TagsCloudVisualization.Layouter;

namespace TagsCloudVisualization.CloudFiller;

public class CloudFiller
{
    private ICloudLayouter layouter;

    public CloudFiller(ICloudLayouter layouter)
    {
        this.layouter = layouter;
    }

    public void FillCloud(int height, int width, int tagsCount)
    {
        for (var i = 0; i < tagsCount; i++)
            layouter.PutNextRectangle(new Size(width, height));
    }

    public void FillRandomCloud(int tagsCount, int seed)
    {
        var rnd = new Random(seed);
        for (var i = 0; i < tagsCount; i++)
            layouter.PutNextRectangle(new Size(rnd.Next(20, 60), rnd.Next(20, 60)));
    }
}