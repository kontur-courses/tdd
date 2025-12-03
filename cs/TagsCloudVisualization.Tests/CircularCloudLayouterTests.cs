using System.Drawing;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.CoordinateGenerators;
using TagsCloudVisualization.Drawers;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter _layouter;
    private Point _center;
    private string _testName;

    [SetUp]
    public void SetUp()
    {
        _center = new Point(100, 100);
        var generator = new SpiralCoordinateGenerator(_center, 0.1);
        _layouter = new CircularCloudLayouter(_center, generator);
        _testName = TestContext.CurrentContext.Test.Name;
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            SaveVisualization();
        }
    }

    private void SaveVisualization()
    {
        var outputDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "FailedTests");
        Directory.CreateDirectory(outputDir);

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var fileName = $"{_testName}_{timestamp}.png";
        var filePath = Path.Combine(outputDir, fileName);

        var drawer = new TagCloudDrawer(_layouter.Rectangles);
        drawer.DrawRectanglesToFile(filePath);

        Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
        TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
    }

    [Test]
    public void PutNextRectangle_ShouldAddRectangleToCollection()
    {
        var rectangle = _layouter.PutNextRectangle(new Size(50, 30));
        
        Assert.That(_layouter.Rectangles, Contains.Item(rectangle));
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithCorrectSize()
    {
        var size = new Size(50, 30);
        var rectangle = _layouter.PutNextRectangle(size);
        
        Assert.Multiple(() =>
        {
            Assert.That(rectangle.Width, Is.EqualTo(50));
            Assert.That(rectangle.Height, Is.EqualTo(30));
        });
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ShouldNotIntersect()
    {
        var rect1 = _layouter.PutNextRectangle(new Size(50, 30));
        var rect2 = _layouter.PutNextRectangle(new Size(40, 40));
        
        Assert.That(rect1.IntersectsWith(rect2), Is.False);
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_ShouldNotIntersect()
    {
        var rect1 = _layouter.PutNextRectangle(new Size(50, 30));
        var rect2 = _layouter.PutNextRectangle(new Size(40, 40));
        var rect3 = _layouter.PutNextRectangle(new Size(60, 20));
        
        Assert.Multiple(() =>
        {
            Assert.That(rect1.IntersectsWith(rect2), Is.False);
            Assert.That(rect1.IntersectsWith(rect3), Is.False);
            Assert.That(rect2.IntersectsWith(rect3), Is.False);
        });
    }

    [Test]
    public void PutNextRectangle_TenRectangles_ShouldNotIntersect()
    {
        for (var i = 0; i < 10; i++)
        {
            _layouter.PutNextRectangle(new Size(30, 30));
        }

        var rectangles = _layouter.Rectangles.ToList();
        
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False);
            }
        }
    }

    [Test]
    public void Rectangles_InitiallyEmpty()
    {
        Assert.That(_layouter.Rectangles, Is.Empty);
    }

    [Test]
    public void Rectangles_AfterAddingOne_CountIsOne()
    {
        _layouter.PutNextRectangle(new Size(50, 30));
        
        Assert.That(_layouter.Rectangles, Has.Count.EqualTo(1));
    }

    [Test]
    public void Rectangles_AfterAddingThree_CountIsThree()
    {
        _layouter.PutNextRectangle(new Size(50, 30));
        _layouter.PutNextRectangle(new Size(40, 40));
        _layouter.PutNextRectangle(new Size(60, 20));
        
        Assert.That(_layouter.Rectangles, Has.Count.EqualTo(3));
    }

    [Test]
    public void PutNextRectangle_DifferentSizes_ShouldNotIntersect()
    {
        var rect1 = _layouter.PutNextRectangle(new Size(10, 10));
        var rect2 = _layouter.PutNextRectangle(new Size(100, 50));
        var rect3 = _layouter.PutNextRectangle(new Size(30, 80));
        
        Assert.Multiple(() =>
        {
            Assert.That(rect1.IntersectsWith(rect2), Is.False);
            Assert.That(rect1.IntersectsWith(rect3), Is.False);
            Assert.That(rect2.IntersectsWith(rect3), Is.False);
        });
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_ShouldNotIntersect()
    {
        for (var i = 0; i < 50; i++)
        {
            _layouter.PutNextRectangle(new Size(20, 20));
        }

        var rectangles = _layouter.Rectangles.ToList();
        
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False);
            }
        }
    }
}