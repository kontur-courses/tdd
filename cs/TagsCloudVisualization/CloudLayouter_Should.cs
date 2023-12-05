using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization;

public class CloudLayouter_Should
{
    private CloudLayouter? circularCloudLayouter;

    [SetUp]
    public void SetCircularCloudFieldToNull()
    {
        circularCloudLayouter = null;
    }

    [TearDown]
    public void CreateLayoutImage_IfTestFailed()
    {
        if (TestContext.CurrentContext.Result.FailCount < 1) return;

        var testName = TestContext.CurrentContext.Test.Name;
        var workDirectory = TestContext.CurrentContext.WorkDirectory;

        LayoutDrawer.CreateLayoutImage(circularCloudLayouter?.CreatedRectangles!, testName, workDirectory);

        var filePath = $@"{workDirectory}\{testName}.png";

        TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
        TestContext.AddTestAttachment($"{testName}.png");
    }

    [TestCase(-1, 0, TestName = "Negative width")]
    [TestCase(0, -1, TestName = "Negative height")]
    [TestCase(-5, -5, TestName = "Negative width and height")]
    public void PutNextRectangleThrowsArgumentException_WhenNegativeParameters(int rectWidth, int rectHeight)
    {
        circularCloudLayouter = new CloudLayouter(new SpiralGenerator());
        var rectangleSize = new Size(rectWidth, rectHeight);
        var rectangleCreation = () => circularCloudLayouter.PutNextRectangle(rectangleSize);
        rectangleCreation.Should().Throw<ArgumentException>();
    }
}