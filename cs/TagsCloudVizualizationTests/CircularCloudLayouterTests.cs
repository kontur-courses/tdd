using System.Drawing;
using TagsCloudVizualization;
using FluentAssertions;
using TagsCloudVizualization.Utility;

namespace TagsCloudVizualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter sut;
    private Point center = new(720, 720);
    
    [SetUp]
    public void Setup()
    {
        center = new Point(720, 720);
        var spiral = new Spiral(center, 0.02, 0.01);
        sut = new CircularCloudLayouter(center, spiral);
    }
    
    [Test]
    public void Constructor_ShouldNotThrow()
    {
        Assert.DoesNotThrow(() => new CircularCloudLayouter(center, new Spiral(center, 0.02, 0.01)));
    }

    [TestCase(-4, 16, TestName = "with negative rectangle width")]
    [TestCase(77, -8, TestName = "with negative rectangle height")]
    public void PutNextRectangle_InvalidSize_ThrowsArgumentException(int rectangleWidth, int rectangleHeight)
    {
        Assert.Throws<ArgumentException>(() => sut.PutNextRectangle(new Size(rectangleWidth, rectangleHeight)));
    }
    
    [Test]
    public void PutNextRectangle_ShouldReturnCorrectRectangleSize()
    {
        var rectangle = sut.PutNextRectangle(new Size(8, 8));
        
        rectangle.Size.Should().Be(new Size(8, 8));
    }
    
    [TestCase(10, TestName = "10 rectangles with maximum density")]
    [TestCase(100, TestName = "100 rectangles with maximum density")]
    [TestCase(200, TestName = "200 rectangles with maximum density")]
   public void PutNextRectangle_RectanglesShouldHaveMaximumDensity(int rectanglesCount)
{
    var center = new Point(0, 0);
    var spiral = new Spiral(center, 0.02, 0.01);
    var layouter = new CircularCloudLayouter(center, spiral);
    var rectangleSize = new Size(10, 10);

    GenerateRectangles(layouter, rectangleSize, rectanglesCount);

    CheckDensity(layouter.Rectangles).Should().BeTrue();
}
        private void GenerateRectangles(CircularCloudLayouter layouter, Size rectangleSize, int rectanglesCount)
        {
            for (int i = 0; i < rectanglesCount; i++)
            {
                layouter.PutNextRectangle(rectangleSize);
            }
        }

        private bool CheckDensity(IList<Rectangle> rectangles)
        {
            var thresholdOverlapPercentage = 0.2;
            var totalPairs = rectangles.Count * (rectangles.Count - 1) / 2;

            var intersectedPairsCount = rectangles
                .SelectMany((rect1, index1) => rectangles.Skip(index1 + 1)
                    .Select(rect2 => rect1.IntersectsWith(rect2)))
                .Count(isIntersected => isIntersected);

            var overlapPercentage = (double)intersectedPairsCount / totalPairs;

            return overlapPercentage <= thresholdOverlapPercentage;
        }
        
    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldHaveCenterAtLayoutCenter()
    {
        var rectangle = sut.PutNextRectangle(new Size(8, 8));
        
        var expectedRectangleCenter = new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
        expectedRectangleCenter.Should().Be(center);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_SecondRectangleCenterShouldNotBeAtLayoutCenter()
    {
        sut.PutNextRectangle(new Size(8, 8));
        
        var secondRectangle = sut.PutNextRectangle(new Size(6, 6));
        
        var expectedRectangleCenter = new Point(
            secondRectangle.Left + secondRectangle.Width / 2,
            secondRectangle.Top + secondRectangle.Height / 2);

        expectedRectangleCenter.Should().NotBe(center);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ShouldNotIntersect()
    {
        var firstRectangle = sut.PutNextRectangle(new Size(8, 8));
        
        var secondRectangle = sut.PutNextRectangle(new Size(77, 77));
        
        secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
    }
    
    [TestCase(10, TestName = "10 rectangles WithNoIntersects")]
    [TestCase(100, TestName = "100 rectangles WithNoIntersects")]
    [TestCase(200, TestName = "200 rectangles WithNoIntersects")]
    public void PutNextRectangle_GeneratesRectanglesWithoutIntersects(int rectanglesCount)
    {
        GenerateRectangles(rectanglesCount, new Size(10, 10));

        Assert.IsFalse(HasIntersectedRectangles(sut.Rectangles));
    }
    
    private bool HasIntersectedRectangles(IList<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count - 1; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if (RectanglesIntersect(rectangles[i], rectangles[j]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool RectanglesIntersect(Rectangle first, Rectangle second)
    {
        return first.IntersectsWith(second);
    }

    private void GenerateRectangles(int count, Size rectangleSize)
    {
        for (var i = 0; i < count; i++)
        {
            sut.PutNextRectangle(rectangleSize);
        }
    }
    
    [TearDown]
    public void SaveImageWhenTestFails()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            SaveFailedTestImage();
        }
    }

    private void SaveFailedTestImage()
    {
        var pathToFailedTests = @"..\..\..\FailedTests";
        CreateDirectoryIfNotExists(pathToFailedTests);

        var image = Visualizer.VisualizeRectangles(sut.Rectangles, 500, 500);
        var fileName = $"{TestContext.CurrentContext.Test.Name}.png";
        Visualizer.SaveBitmap(image, fileName, pathToFailedTests);
    }

    private void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    
}
