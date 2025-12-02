using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Helpers;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Visualizers;

namespace Tests.CloudLayouterTests;

public class CircularCloudLayouterTestBase
{
    protected Point Center;
    private ISpiral spiral;
    protected ICircularCloudLayouter Layouter;
    protected const int TestSeed = 123;

    [SetUp]
    public void SetUp()
    {
        Center = new Point(100, 100);
        spiral = new ArchimedeanSpiral(Center);
        Layouter = new SpiralCloudLayouter(spiral);
    }

    [TearDown]
    public void TearDown()
    {
        if(TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var failuresFolder = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestFailures");
            Directory.CreateDirectory(failuresFolder); 

            var testName = TestContext.CurrentContext.Test.Name;
            var fileName = $"test_failure_{testName}.png";
            var fullPath = Path.Combine(failuresFolder, fileName);
        
            SaveCloudVisualization(fullPath);
            TestContext.WriteLine($"Test {testName} failed. Tag cloud visualization saved to {fullPath}");
        }
    }

    private void SaveCloudVisualization(string fileName)
    {
        var visualizer = new BitmapCloudVisualizer(VisualizerSettings.Default());
        var image = visualizer.CreateImage(Layouter.Rectangles);
        new BitmapCloudSaver().SaveToFile(image, fileName, ImageFormat.Png);
    }
}