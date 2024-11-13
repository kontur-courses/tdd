using System.Drawing;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.TagsCloudDistributions;

namespace TagsCloudVisualization;

public class TagsCloudRunner
{
    private readonly int rectanglesCount;
    private readonly string imagesPath;
    private readonly ICloudLayouter cloudLayouter;

    private TagsCloudRunner(int rectanglesCount, string imagesPath)
    {
        this.imagesPath = imagesPath;
        this.rectanglesCount = rectanglesCount;
        cloudLayouter = new CircularCloudLayouter(new SpiralDistribution());
    }

    public static TagsCloudRunner Create(int rectanglesCount, string imagesPath)
    {
        if (rectanglesCount <= 0)
            throw new ArgumentException("The rectangles count must be greater than zero.");
        
        if (imagesPath == null)
            throw new ArgumentException("The images path cannot be null.");
        
        return new TagsCloudRunner(rectanglesCount, imagesPath);
    }

    public void Run()
    {
        FillInCloud();
        RenderAndSaveImage();
    }

    private void FillInCloud()
    {
        var rnd = new Random();
        for (var i = 0; i < rectanglesCount; i++)
        {
            var rectangleSize = new Size(rnd.Next(5, 30), rnd.Next(5, 30));
            cloudLayouter.PutNextRectangle(rectangleSize);
        }
    }

    private void RenderAndSaveImage()
    {
        var render = TagsCloudRenderer.Create(cloudLayouter);
        render.Render();
        render.SaveToPath($"rectangle_{rectanglesCount}.png", imagesPath);
    }
}