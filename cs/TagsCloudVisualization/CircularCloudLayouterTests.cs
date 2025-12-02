using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;


namespace TagsCloudVisualization;

public class CircularCloudLayouterTests
{
    private Point center;
    private CircularCloudLayouter layouter;
    private Size[] sizes;
    private string imagePath;
    private Rectangle[] rectangles;
    private RectanglesDrawer rectanglesDrawer;
    
    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        layouter = new CircularCloudLayouter(center);
        rectanglesDrawer = new RectanglesDrawer(rectangles, center);
        imagePath = Path.Combine(TestContext.CurrentContext.TestDirectory, $"{TestContext.CurrentContext.Test.Name}_cloud.png");
        
        var len = 500;
        sizes = new Size[len];
        rectangles = new Rectangle[len];
        var random = new Random();
        
        for (int i = 0; i < len; i++)
        {
            sizes[i] =  new Size(random.Next(40, 240), random.Next(15, 30));
            rectangles[i] = layouter.PutNextRectangle(sizes[i]);
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        var testResult = TestContext.CurrentContext.Result.Outcome;
        
        if (testResult.Status == TestStatus.Failed)
        {
            try
            {
                rectanglesDrawer.GenerateImage(imagePath);
                Console.WriteLine($"Tag cloud visualization saved to file {imagePath}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fail to save image: {e.Message}");
            }
        }
    }
    
    public bool TwoRectanglesIsPressed(Rectangle firstRectangle, Rectangle secondRectangle)
    {
        var IsPressedX = (firstRectangle.X + firstRectangle.Width == secondRectangle.X ||
                          secondRectangle.X + secondRectangle.Width == firstRectangle.X);
        var IsPressedY = (firstRectangle.Y + firstRectangle.Height == secondRectangle.Y ||
                          secondRectangle.Y + secondRectangle.Height == firstRectangle.Y);
        
        return IsPressedY || IsPressedX;
    }

    public double GetTotalAreaOfRectangles(Rectangle[] rectangles)
    {
        var totalAreaOfRectangles = 0;
        foreach (var rectangle in rectangles)
        {
            totalAreaOfRectangles += rectangle.Height  *  rectangle.Width;
        }
        return totalAreaOfRectangles;
    }
    
    public double GetDistanceBetweenPoints(Point firstPoint, Point secondPoint)
    {
        var distanceX =  firstPoint.X - secondPoint.X;
        var distanceY = firstPoint.Y - secondPoint.Y;
        return Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }
    
    public double GetAreaOfRound(Rectangle[] rectangles, Point center)
    {
        double maxRadius = 0;
        
        foreach (var rectangle in rectangles)
        {
            Point[] points =
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Right, rectangle.Bottom)
            };
            foreach (var point in points)
            {
                var radius = GetDistanceBetweenPoints(center, point);
                maxRadius = Math.Max(radius, maxRadius);
            }
        }
        return Math.PI * maxRadius * maxRadius;
    }
    
    [TestCase(-30, -10)]
    [TestCase(-30, 10)]
    [TestCase(30, -10)]
    [TestCase(0, 10)]
    [TestCase(10, 0)]
    [TestCase(0, 0)]
    public void PutNextRectangle_RectangleWithInvalidSize_ShouldBeThrowException(int width, int height)
    {
        var newRecSize = new Size(width, height);
    
        var act = () => layouter.PutNextRectangle(newRecSize);

        act.Should().Throw<ArgumentException>();
    }

    [TestCase(-200, -100)]
    [TestCase(-300, 100)]
    [TestCase(30, -10)]
    [TestCase(0, 0)]
    public void PutNextRectangle_FirstRectangle_ShouldBeOnCenter(int x, int y)
    {
        var newCenter = new Point(x, y);
        layouter = new CircularCloudLayouter(newCenter);
        var newRecSize = new Size(30, 10);

        var rectangle = layouter.PutNextRectangle(newRecSize);
        
        var rectangleCenter = rectangle.GetCenter();
        rectangleCenter.X.Should().Be(x);
        rectangleCenter.Y.Should().Be(y);
    }
    
    [Test]
    public void PutNextRectangle_SomeRectangles_ShouldNotIntersect()
    {
        for (int i = 0; i < rectangles.Length; i++)
        {
            for (int j = i + 1; j < rectangles.Length; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should()
                    .BeFalse(rectangles[i] + " " + i + " and " + rectangles[j] + " " + j + " intersect");
            }
        }
    }
    
    [Test]
    public void PutNextRectangle_EachRectangles_ShouldPressedToOtherRectangle()
    {
        for (int i = 0; i < rectangles.Length; i++)
        {
            var touches = false;
            
            for (int j = 0; j < rectangles.Length; j++)
            {
                if (i != j && TwoRectanglesIsPressed(rectangles[i], rectangles[j]))
                {
                    touches = true;
                    break; 
                }
            }
            touches.Should().BeTrue("rec" + rectangles[i] + " not  touch");
        }
    }
    
    [Test]
    public void PutNextRectangle_Cloud_ShouldBeTightly()
    {
        var totalAreaOfRectangles = GetTotalAreaOfRectangles(rectangles);
        var areaOfRound = GetAreaOfRound(rectangles, center);
        var aspectOfAreas =  totalAreaOfRectangles / areaOfRound;
        
        aspectOfAreas.Should().BeGreaterThan(0.5);
    }
}