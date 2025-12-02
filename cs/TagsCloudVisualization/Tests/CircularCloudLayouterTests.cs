using System.Drawing;
using System.Runtime.Versioning;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter cloudLayouter;
    private int imageSize;
    private Point center;
    private double spiralStep;
    private double spiralParameterA;
    
    [SetUp]
    public void SetUp()
    {
        center = new Point(500, 500);
        spiralStep = Math.PI / 180;
        spiralParameterA = 0.1;
        imageSize = 1000;
        cloudLayouter = new CircularCloudLayouter(center);
        cloudLayouter.SetNewSpiral(new ArchimedeanSpiral(center, spiralStep, spiralParameterA));
    }
    
    [SupportedOSPlatform("Windows")]
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{TestContext.CurrentContext.Test.Name}.png";
            var imagesDirectory = Path.Combine(desktopPath, "Images failed tests");
            
            Directory.CreateDirectory(imagesDirectory);

            var filePath = Path.Combine(imagesDirectory, fileName);
            
            Visualizer.GenerateImageFromCircularCloudLayouter(imageSize, filePath, cloudLayouter);
            
            TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }
    }
    
    [TestCaseSource(nameof(CasesWhenInvalidSize))]
    public void PutNextRectangle_ThrowsArgumentException_WhenInvalidSize(Size input)
    {
        var action = () => cloudLayouter.PutNextRectangle(input);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCaseSource(nameof(CasesWhenRandomSize))]
    public void PutNextRectangle_ReturnsCorrectSizeRectangle_WhenRandomSize(Size input)
    {
        var result = cloudLayouter.PutNextRectangle(input);
        
        result.Height.Should().Be(input.Height);
        result.Width.Should().Be(input.Width);
    }
    
    [TestCaseSource(nameof(CasesWhenSeveralRandomSizes))]
    public void PutNextRectangle_NonIntersectingRectangles_WhenSeveralSizes(IEnumerable<Size> input)
    {
        foreach (var size in input)
        {
            cloudLayouter.PutNextRectangle(size);
        }

        for (var i = 0; i < cloudLayouter.Rectangles.Count; i++)
        {
            for (var j = i + 1; j < cloudLayouter.Rectangles.Count; j++)
            {
                cloudLayouter.Rectangles[i].IntersectsWith(cloudLayouter.Rectangles[j]).Should().Be(false);
            }
        }
    }
    
    [TestCaseSource(nameof(CasesWhenRandomSize))]
    public void PutNextRectangle_FirstRectangleInCenter_WhenRandomSize(Size input)
    {
        var result = cloudLayouter.PutNextRectangle(input);
        
        result.GetCenter().X.Should().BeApproximately(center.X, 0.5f);
        result.GetCenter().Y.Should().BeApproximately(center.Y, 0.5f);
    }
    
    [Explicit]
    [TestCaseSource(nameof(CasesWhen10IEnumerableWith100SmallRangeRandomSizes))]
    public void PutNextRectangle_AverageCloudDensityIsOver70Percents_When10IEnumerableWith100SmallRangeRandomSizes(IEnumerable<IEnumerable<Size>> inputs)
    {
        var density = new List<double>();
        
        foreach (var input in inputs)
        {
            cloudLayouter = new CircularCloudLayouter(center);
            cloudLayouter.SetNewSpiral(new ArchimedeanSpiral(center, spiralStep, spiralParameterA));
            foreach (var size in input)
            {
                cloudLayouter.PutNextRectangle(size);
            }
            density.Add(FindAreaOfAllRectangles() / FindAreaOfCircleWithMinimumRadiusThatEnclosesAllRectangles()); 
        }

        var result = density.Average();
        
        result.Should().BeGreaterThan(0.7d);
        
        TestContext.WriteLine($"Average density: {result}");
    }
    
    [Explicit]
    [TestCaseSource(nameof(CasesWhen10IEnumerableWith100BigRangeRandomSizes))]
    public void PutNextRectangle_AverageCloudDensityIsOver55Percents_When10IEnumerableWith100BigRangeRandomSizes(IEnumerable<IEnumerable<Size>> inputs)
    {
        var density = new List<double>();
        
        foreach (var input in inputs)
        {
            cloudLayouter = new CircularCloudLayouter(center);
            cloudLayouter.SetNewSpiral(new ArchimedeanSpiral(center, spiralStep, spiralParameterA));
            foreach (var size in input)
            {
                cloudLayouter.PutNextRectangle(size);
            }
            density.Add(FindAreaOfAllRectangles() / FindAreaOfCircleWithMinimumRadiusThatEnclosesAllRectangles()); 
        }

        var result = density.Average();
        
        result.Should().BeGreaterThan(0.55d);
        
        TestContext.WriteLine($"Average density: {result}");
    }
    
    public static IEnumerable<TestCaseData> CasesWhenInvalidSize()
    {
        yield return new TestCaseData(new Size(1, 0));
        yield return new TestCaseData(new Size(0, 1));
        yield return new TestCaseData(new Size(-4, 1));
        yield return new TestCaseData(new Size(1, -4));
        yield return new TestCaseData(new Size(0, 0));
        yield return new TestCaseData(new Size(-4, -4));
    }
    
    public static IEnumerable<TestCaseData> CasesWhenSeveralRandomSizes()
    {
        var random = new Random();
        var testData = new List<Size>();
        for (var i = 0; i < 10; i++)
        {
            var width = random.Next(2, 101);
            var height = random.Next(2, width);
            testData.Add(new Size(width, height));
        }
        yield return new TestCaseData(testData);
    }
    
    public static IEnumerable<TestCaseData> CasesWhen10IEnumerableWith100SmallRangeRandomSizes()
    {
        var random = new Random();
        var testData = new List<List<Size>>();
        for (var i = 0; i < 10; i++)
        {
            var internalTestData = new List<Size>();
            for (var j = 0; j < 100; j++)
            {
                var width = random.Next(50, 70);
                var height = random.Next(50, width);
                internalTestData.Add(new Size(width, height));
            }
            testData.Add(internalTestData);
        }

        yield return new TestCaseData(testData);
    }
    
    public static IEnumerable<TestCaseData> CasesWhen10IEnumerableWith100BigRangeRandomSizes()
    {
        var random = new Random();
        var testData = new List<List<Size>>();
        for (var i = 0; i < 10; i++)
        {
            var internalTestData = new List<Size>();
            for (var j = 0; j < 100; j++)
            {
                var width = random.Next(2, 101);
                var height = random.Next(2, width);
                internalTestData.Add(new Size(width, height));
            }
            testData.Add(internalTestData);
        }

        yield return new TestCaseData(testData);
    }
    
    public static IEnumerable<TestCaseData> CasesWhenRandomSize()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var width = random.Next(2, 101);
            var height = random.Next(2, width);
            yield return new TestCaseData(new Size(width, height));
        }
    }
    
    private double FindAreaOfCircleWithMinimumRadiusThatEnclosesAllRectangles()
    {
        var points = new List<Point>();
        
        foreach (var rectangle in cloudLayouter.Rectangles)
        {
            points.Add(new Point(rectangle.Left, rectangle.Top));
            points.Add(new Point(rectangle.Right, rectangle.Top));
            points.Add(new Point(rectangle.Left, rectangle.Bottom));
            points.Add(new Point(rectangle.Right, rectangle.Bottom));
        }
        
        var maxDistance = points.Select(point => point.GetDistance(center)).Prepend(0).Max();

        var result = Math.PI * maxDistance * maxDistance;

        return result;
    }
    
    private double FindAreaOfAllRectangles()
    {
        var result = cloudLayouter.Rectangles.Aggregate(0f, (current, rectangle) => current + rectangle.Width * rectangle.Height);

        return result;
    }
}