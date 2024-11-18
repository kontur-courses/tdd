using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisiualizationTests.Utils;
using TagsCloudVisualization;
using TagsCloudVisualization.LayoutRectanglesInCloudAlgorithms;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisiualizationTests;

[TestFixture]
public class CircularCloudLayoutTests
{
    private ICircularCloudLayouter cloudLayouter;
    private List<Rectangle> addedRectangles;
    
    [SetUp]
    public void Setup()
    {
        var center = new Point(0, 0);
        cloudLayouter = new CircularCloudLayouter(new CircularLayoutAlgorithm(center));
        addedRectangles = [];
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) 
            return;
        
        var pathImageStored = TestContext.CurrentContext.TestDirectory + @"\imageFailedTests";

        if (!Directory.Exists(pathImageStored))
        {
            Directory.CreateDirectory(pathImageStored);
        }

        var testName = TestContext.CurrentContext.Test.Name;
        
        var bitmap = ImageDrawer.DrawLayout(addedRectangles, 10);
            
        ImageSaver.Save(bitmap, pathImageStored, $"{testName}.png", ImageFormat.Png);
        
        Console.WriteLine($@"Tag cloud visualization saved to file {pathImageStored}\{testName}.png");
    }
    
    [TestCase(10, 5, 15)]
    [TestCase(50, 30, 100)]
    [TestCase(100, 5, 50)]
    public void PutNextRectangle_ShouldAddedRectanglesDoNotIntersect(int countRectangles, int minSideLength,
        int maxSideLength)
    {
        var rectangleSizes = GeometryUtils.GenerateRectangleSizes(countRectangles, minSideLength, maxSideLength);
        
        addedRectangles.AddRange(rectangleSizes.Select(t => cloudLayouter.PutNextRectangle(t)));

        for (var i = 0; i < addedRectangles.Count-1; i++)
        {
            addedRectangles
                .Skip(i + 1)
                .Any(addedRectangle => addedRectangle.IntersectsWith(addedRectangles[i]))
                .Should()
                .BeFalse();
        }
    }
    
    [TestCase(10, 5, 15)]
    [TestCase(50, 30, 100)]
    [TestCase(100, 5, 50)]
    public void CircleShape_ShouldBeCloseToCircle_WhenAddMultipleRectangles(int countRectangles, int minSideLength, 
        int maxSideLength)
    {
        var rectangleSizes = GeometryUtils.GenerateRectangleSizes(countRectangles, minSideLength, maxSideLength);
        
        addedRectangles.AddRange(rectangleSizes.Select(t => cloudLayouter.PutNextRectangle(t)));

        var distances = addedRectangles
            .Select(rectangle => 
                GeometryUtils.CalculateDistanceBetweenCenterRectangleAndCenterCloud(rectangle, new Point(0, 0)))
            .ToArray();

        for (var i = 1; i < distances.Length; i++)
        {
            var distanceBetweenRectangles =
                GeometryUtils.CalculateDistanceBetweenRectangles(addedRectangles[i], addedRectangles[i - 1]);
            distances[i].Should().BeApproximately(distances[i - 1], distanceBetweenRectangles);
        }
    }
}