using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudTests;

[TestFixture]
public abstract class CircularCloudLayouterTestsBase
{
    protected CircularCloudLayouter Layouter;
    protected CloudVisualizer Visualizer;
    protected ArchimedeanSpiralPointGenerator Generator;
    protected SizeF[] RectangleSizes;
    protected PointF Center;
    protected int MaxDistanceToCenter;
    protected string ImagesDirectory;

    [SetUp]
    public virtual void SetUp()
    {
        Center = new PointF(600, 450);
        Layouter = new CircularCloudLayouter(Center);
        Visualizer = new CloudVisualizer();
        RectangleSizes =
        [
            new SizeF(70, 100),
            new SizeF(60, 60),
            new SizeF(90, 30),
            new SizeF(75, 115),
            new SizeF(100, 100)
        ];
        Generator = new ArchimedeanSpiralPointGenerator(Center);
        MaxDistanceToCenter = 120;
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                               ?? throw new InvalidOperationException("Не удалось определить директорию проекта.");
        ImagesDirectory = Path.Combine(projectDirectory, "Images");
        if (!Directory.Exists(ImagesDirectory))
        {
            Directory.CreateDirectory(ImagesDirectory);
        }
    }

    [TearDown]
    public virtual void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Failed) 
            return;
        var testName = TestContext.CurrentContext.Test.Name;
        var filePath = Path.Combine(ImagesDirectory, $"{testName}_failed.png");
        Visualizer.SaveVisualization(Layouter.Rectangles, filePath);
        Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
    }
}