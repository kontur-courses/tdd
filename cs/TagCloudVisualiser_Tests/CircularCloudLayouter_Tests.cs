using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloudVisualizer.CloudLayouter;
using TagCloudVisualizer.TagCloudImageGenerator;

namespace TagCloudVisualiser_Tests;

[TestFixture]
public class CircularCloudLayouter_Tests
{
    private static readonly Size CANVAS_SIZE = new Size(250, 250);
    private static readonly Point CENTER = new Point(CANVAS_SIZE.Width / 2, CANVAS_SIZE.Height / 2);
    
    private CircularCloudLayouter layouter;
    
    [SetUp]
    public void SetUpTest()
    {
        layouter = new CircularCloudLayouter(CENTER);
    }

    [TearDown]
    public void TearDownTest()
    {
        if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
            return;

        var path = Environment.CurrentDirectory + "\\FailedTests\\";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var timestamp = DateTime.Now;
        var filename = $"{TestContext.CurrentContext.Test.Name}_{timestamp:yyyy-MM-dd-HH-mm-ss}.png";
        var fullpath = path + filename;
        
        var image = TagCloudImageGenerator.GenerateImage(layouter.Rectangles.ToArray(), CANVAS_SIZE);
        image.Save(fullpath, ImageFormat.Png);
        
        Console.WriteLine($"Image of generated tag cloud saved as {fullpath}");
    }
    
    [TestCase(0, 0, Description = "Zero coordinates")]
    [TestCase(1, 1, Description = "Positive coordinates")]
    [TestCase(-1, -1, Description = "Negative coordinates")]
    public void Constructor_ShouldAcceptAnyPoint(int x, int y)
    {
        Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(x, y)));
    }

    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    public void PutNextRectangle_ShouldThrowArgumentException_OnNegativeSize(int width, int height)
    {
        Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
    }

    [Test]
    public void PutNextRectangle_ShouldPlaceFirstRectangleInCenter()
    {
        Assert.That(layouter.PutNextRectangle(new Size(10, 10)),
            Is.EqualTo(new Rectangle(new Point(CENTER.X - 5, CENTER.X - 5), new Size(10, 10))));
    }
    
    [TestCase(1, Description = "One rectangle should not intersect")]
    [TestCase(10, Description = "Ten rectangles should not intersect")]
    [TestCase(100, Description = "A hundred rectangles should not intersect")]
    [TestCase(1000, Description = "A thousand rectangles should not intersect")]
    public void PutNextRectangle_RectanglesShouldNotIntersect(int count)
    {
        var checkedRectangles = new List<Rectangle>();
        for (var i = 0; i < count; i++)
        {
            var rectangle = layouter.PutNextRectangle(new Size(10, 10));
            if (checkedRectangles.Any(r => r.IntersectsWith(rectangle)))
                Assert.Fail($"Rectangles should not intersect, failed on iteration {i + 1}");
            checkedRectangles.Add(rectangle);
        }
    }
}