using System.Reflection;
using NUnit.Framework.Interfaces;
using TagsCloudVisualizationTests.Extensions;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    [SetUp]
    public void SetUp()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status is not TestStatus.Failed)
            return;
        
        var visualizerParams = new VisualizerParams(width:100, height: 100);
        visualizerParams.PathToFile = $"../../../TestFailedVisualization";
        visualizerParams.FileName = $"{TestContext.CurrentContext.Test.Name}.png";
        
        var visualizer = new CircularCloudVisualizer(visualizerParams);
        visualizer.Visualize(_layouter);
        
        Console.WriteLine($"Tag cloud visualization saved to file {Path.GetFullPath(visualizerParams.PathWithFileName)}");
    }
    
    private CircularCloudLayouter _layouter;
    
    [TestCase(0, 0, TestName = "ZeroDimensions")]
    [TestCase(-1, -1, TestName = "NegativeDimensions")]
    public void PutNextRectangle_ThrowsArgumentExceptionOn(int width, int height)
    {
        new Action(() => _layouter.PutNextRectangle(new Size(width, height)))
            .Should()
            .ThrowExactly<ArgumentException>()
            .Where(e => e.Message.Equals("Width and height of rectangle must be positive"));
    }

    [Test]
    public void PutNextRectangle_RectanglesShouldNotIntersect()
    {
        var rectangles = AddRectangles(_layouter).ToArray();
        
        rectangles
            .Where(r1 => rectangles.Where(r2 => r2 != r1).Any(r1.IntersectsWith))
            .Should()
            .BeEmpty();
    }

    [Test, Timeout(500)]
    public void PutNextRectangle_MustBeEfficient()
    {
        AddRectangles(_layouter).Count();
    }

    [TestCase(0, TestName = "WhenEmpty")]
    [TestCase(1, TestName = "WhenOneRectangleGiven")]
    [TestCase(6, TestName = "WhenMultipleRectanglesGiven")]
    public void HasCenterInInitCoords(int rectanglesToAdd)
    {
        for (var i = 0; i < rectanglesToAdd; i++)
            _layouter.PutNextRectangle(new Size(1, 1));

        _layouter.Center
            .Should()
            .Be(new Point(0, 0));
    }
    
    [Test]
    public void CanPutRectangle_ReturnsFalseOnPuttingRectangleToOccupiedPlace()
    {
        var type = _layouter.GetType();
        
        type.GetAndInvokeMethod(
            "PutRectangle", 
            BindingFlags.NonPublic | BindingFlags.Instance,
            _layouter, 
            new Rectangle(0, 0, 1, 1));
        
        type.GetAndInvokeMethod(
                "CanPutRectangle", 
                BindingFlags.NonPublic | BindingFlags.Instance,
                _layouter, 
                new Rectangle(0, 0, 1, 1))
            .Should()
            .Be(false);
    }
    
    [Test]
    public void CanPutRectangle_ReturnsTrueOnPuttingRectangleToEmptyPlace()
    {
        var type = _layouter.GetType();
        
        type.GetAndInvokeMethod(
            "PutRectangle", 
            BindingFlags.NonPublic | BindingFlags.Instance,
            _layouter, 
            new Rectangle(0, 0, 1, 1));
        
        type.GetAndInvokeMethod(
                "CanPutRectangle", 
                BindingFlags.NonPublic | BindingFlags.Instance,
                _layouter, 
                new Rectangle(2, 2, 1, 1))
            .Should()
            .Be(true);
    }
    
    // Тест сделан для демонстрации метода TearDown
    [Test]
    public void Teardown_Fails()
    {
        var type = _layouter.GetType();
        
        type.GetAndInvokeMethod(
            "PutRectangle", 
            BindingFlags.NonPublic | BindingFlags.Instance,
            _layouter, 
            new Rectangle(0, 0, 3, 3));
        
        type.GetAndInvokeMethod(
                "CanPutRectangle", 
                BindingFlags.NonPublic | BindingFlags.Instance,
                _layouter, 
                new Rectangle(2, 2, 1, 1))
            .Should()
            .Be(true);
    }

    [Test]
    public void GetRectanglesIsReadOnly()
    {
        _layouter.GetRectangles.GetType()
            .Should()
            .Implement(typeof(IReadOnlyCollection<Rectangle>));
    }
    
    private static IEnumerable<Rectangle> AddRectangles(CircularCloudLayouter layouter, int count = 100)
    {
        var rnd = new Random(228_666);
        
        for (var i = 0; i < count; i++) 
            yield return layouter.PutNextRectangle(GetNextRandomSize(rnd));
    }

    private static Size GetNextRandomSize(Random rnd)
    {
        return new Size(rnd.Next(1, 10), rnd.Next(1, 10));
    }
}