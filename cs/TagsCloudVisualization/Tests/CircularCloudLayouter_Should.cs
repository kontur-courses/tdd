using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouter_Should
{
    private readonly string path = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestsResults");
    private CircularCloudLayouter cloudLayouter;
    private Point center;
    private Size canvasSize;

    private Randomizer Random => TestContext.CurrentContext.Random;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        canvasSize = new Size(900, 900);
        center = new Point(450, 450);

        DeleteTestResults();
    }

    [SetUp]
    public void SetUp()
    {
        cloudLayouter = new CircularCloudLayouter(center);
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var test = TestContext.CurrentContext.Test;
        if (status != TestStatus.Failed) return;
        SaveFailedTestResult(test);
    }

    [Test]
    public void PutNextRectangle_ShouldThrow_WhenSizeNotPositive()
    {
        var action = () => cloudLayouter.PutNextRectangle(new Size(0, 0));

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithGivenSize()
    {
        var size = new Size(10, 10);

        var rectangle = cloudLayouter.PutNextRectangle(size);

        rectangle.Size.Should().Be(size);
    }

    [Test]
    public void PutNextRectangle_ShouldPlaceFirstRectangleInCenter()
    {
        var size = new Size(50, 50);
        var offsettedCenter = center - size / 2;

        var rectangle = cloudLayouter.PutNextRectangle(size);

        rectangle.Location.Should().Be(offsettedCenter);
    }

    [Test]
    public void PutNextRectangle_ShouldReturnNotIntersectingRectangles()
    {
        var sizes = GetRandomSizes(70);

        var actualRectangles = sizes.Select(x => cloudLayouter.PutNextRectangle(x)).ToList();

        for (var i = 0; i < actualRectangles.Count - 1; i++)
        {
            for (var j = i + 1; j < actualRectangles.Count; j++)
            {
                actualRectangles[i].IntersectsWith(actualRectangles[j]).Should().BeFalse();
            }
        }
    }

    [Test]
    public void PutNextRectangle_ShouldCreateLayoutCloseToCircle()
    {
        var sizes = GetRandomSizes(50);

        var rectangles = sizes.Select(size => cloudLayouter.PutNextRectangle(size)).ToList();
        var rectanglesSquare = rectangles
            .Select(x => x.Width * x.Height)
            .Sum();
        var circleSquare = CalculateBoundingCircleSquare(rectangles);

        rectanglesSquare.Should().BeInRange((int)(circleSquare * 0.8), (int)(circleSquare * 1.2));
    }

    private void DeleteTestResults()
    {
        var di = new DirectoryInfo(path);
        foreach (var file in di.GetFiles())
        {
            file.Delete();
        }
    }

    private void SaveFailedTestResult(TestContext.TestAdapter test)
    {
        Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestsResults"));
        var pathToImage = Path.Combine(path, test.Name + ".jpeg");

        var visualizer = new LayouterVisualizer(canvasSize);
        visualizer.VisualizeRectangles(cloudLayouter.AddedRectangles);

        visualizer.SaveImage(pathToImage, ImageFormat.Jpeg);
        visualizer.Dispose();
    }
    
    private Size GetRandomSize()
    {
        //return new Size(Random.Next(30, 40), Random.Next(30, 40));
        return new Size(Random.Next(100, 120), Random.Next(30, 90));
    }

    private List<Size> GetRandomSizes(int count)
    {
        var sizes = new List<Size>(count);
        for (var i = 0; i < count; i++)
            sizes.Add(GetRandomSize());
        return sizes;
    }

    private double CalculateBoundingCircleSquare(List<Rectangle> rectangles)
    {
        var rect = rectangles
            .Where(x => x.Contains(x.X, center.Y))
            .MaxBy(x => Math.Abs(x.X - center.X));
        var width = Math.Abs(rect.X - center.X);

        rect = rectangles
            .Where(x => x.Contains(center.X, x.Y))
            .MaxBy(x => Math.Abs(x.Y - center.Y));
        var height = Math.Abs(rect.Y - center.Y);

        return Math.Max(width, height) * Math.Max(width, height) * Math.PI;
    }
}