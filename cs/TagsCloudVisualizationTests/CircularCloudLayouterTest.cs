using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.PointsGenerators;
using TagsCloudVisualization.Savers;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualizationTests;

[TestFixture]
[TestOf(typeof(CircularCloudLayouter))]
public class CircularCloudLayouterTest
{
    private readonly Random randomizer = new();
    private Rectangle[] testRectangles = null!;
    private Point layoutCenter;
    private const string ImagesDirectory = "testImages";
    private const string RectanglesDirectory = "testRectangles";
    
    [TearDown]
    public void TearDown()
    {
        var currentContext = TestContext.CurrentContext;
        if (currentContext.Result.Outcome.Status != TestStatus.Failed)
            return;
        
        var visualizer = new DefaultVisualizer(GetLayoutSize(testRectangles));
        
        using var bitmap = visualizer.CreateBitmap(testRectangles);
        var saver = new DefaultBitmapSaver(ImagesDirectory);
        saver.SaveBitmap(bitmap, currentContext.Test.Name + ".jpg");

        SaveInformationAboutTestRectangles(currentContext);
        
        TestContext.Out
            .WriteLine($"Tag cloud visualization saved to file " +
                       $"{Path.Combine(ImagesDirectory, currentContext.Test.Name + ".jpg")}");
        TestContext.Out
            .WriteLine($"Information about testRectangles saved to file " +
                       $"{Path.Combine(RectanglesDirectory, currentContext.Test.Name + "txt")}");
    }
    
    [Test]
    public void PutNextRectangle_ShouldPutRectangle()
    {
        testRectangles = [];
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        var rectangleSize = randomizer.RandomSize();

        var rectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        testRectangles = [rectangle];
        
        GetLayoutSize(testRectangles).Should().Be(rectangleSize);
    }

    [Test]
    public void PutNextRectangle_ShouldThrowInvalidOperationException_IfFiniteGenerator()
    {
        var finiteGenerator = new FinitePointsGenerator(0);
        var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0), finiteGenerator);
        var invoke = () => circularCloudLayouter.PutNextRectangle(new Size(1, 1));
        
        invoke.Should().Throw<InvalidOperationException>();

    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectangleInCenter_IfFirstInvoke()
    {
        testRectangles = [];
        var rectangleSize = randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        
        var actualRectangle = circularCloudLayouter
            .PutNextRectangle(rectangleSize);
        var expectedRectangle = new Rectangle()
            .CreateRectangleWithCenter(layoutCenter, rectangleSize);
        
        testRectangles = [actualRectangle];
        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithRightSize()
    {
        testRectangles = [];
        var rectangleSize = randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();

        var actualRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        
        testRectangles = [actualRectangle];
        actualRectangle.Size.Should().Be(rectangleSize);
    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesWithoutIntersections()
    {
        testRectangles = [];
        var rectanglesNumber = randomizer.Next(100, 250);
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        
        testRectangles = PutRectanglesInLayouter(rectanglesNumber, circularCloudLayouter);
        
        IsIntersectionBetweenRectangles(testRectangles).Should().BeFalse();
    }

    [Test]
    [Repeat(10)]
    public void ShouldGenerateLayoutThatHasHighTightnessAndShapeOfCircularCloud_WhenOptimalParametersAreUsed()
    {
        testRectangles = [];
        const double allowableDelta = 0.38;

        var circularCloudLayouter = SetupLayouterWithOptimalParameters(); 
        testRectangles = PutRectanglesInLayouter(randomizer.Next(800, 1200), circularCloudLayouter);
        
        var circumcircleRadius = testRectangles
            .Max(r => r
                .GetDistanceToMostRemoteCorner(layoutCenter));
        var circumcircleArea = Math.PI * Math.Pow(circumcircleRadius, 2);
        var rectanglesArea = (double)testRectangles
            .Select(rectangle => rectangle.Height * rectangle.Width)
            .Sum();
        
        var areasFraction = circumcircleArea / rectanglesArea;
        areasFraction.Should().BeApproximately(1, allowableDelta);        
    }

    private Rectangle[] PutRectanglesInLayouter(int rectanglesNumber, 
        CircularCloudLayouter circularCloudLayouter)
    {
        return Enumerable
            .Range(0, rectanglesNumber)
            .Select(_ => randomizer.RandomSize(10, 25))
            .Select(circularCloudLayouter.PutNextRectangle)
            .ToArray();
    }

    private CircularCloudLayouter SetupLayouterWithOptimalParameters()
    {
        layoutCenter = new Point(0, 0);
        return new CircularCloudLayouter(layoutCenter, 1, 0.5);
    }
    
    private CircularCloudLayouter SetupLayouterWithRandomParameters()
    {
        var radius = randomizer.Next(1, 10);
        var angleOffset = randomizer.Next(1, 10);
        layoutCenter = randomizer.RandomPoint(-10, 10);
        
        return new CircularCloudLayouter(layoutCenter, radius, angleOffset);
    }

    private bool IsIntersectionBetweenRectangles(Rectangle[] rectangles)
    {
        for (var i = 0; i < rectangles.Length; i++)
        {
            for (var j = i + 1; j < rectangles.Length; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }

    private void SaveInformationAboutTestRectangles(TestContext currentContext)
    {
        var rectanglesPath = Path.Combine(RectanglesDirectory, 
            currentContext.Test.Name + ".txt");
        Directory.CreateDirectory(RectanglesDirectory);
        File.WriteAllLines(rectanglesPath, 
            testRectangles.Select(rectangle => rectangle.ToString()).ToArray());
    }
    
    private Size GetLayoutSize(IEnumerable<Rectangle> rectangles)
    {
        var layoutWidth = rectangles.Max(rectangle => rectangle.Right) - 
                          rectangles.Min(rectangle => rectangle.Left);
        var layoutHeight = rectangles.Max(rectangle => rectangle.Bottom)
                           - rectangles.Min(rectangle => rectangle.Top);
        return new Size(layoutWidth, layoutHeight);
    }

    class FinitePointsGenerator(int end) : IPointsGenerator
    {
        public IEnumerable<Point> GeneratePoints(Point startPoint)
        {
            return Enumerable.Range(0, end)
                .Select(x => new Point(x, x));
        }
    }
    
}