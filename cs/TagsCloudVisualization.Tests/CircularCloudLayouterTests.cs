using System.Drawing;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private const double MinCircularity = 0.75;
    private const double MinDensity = 0.7;

    private Point center;
    private CircularCloudLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        center = new Point(500, 500);
        layouter = new CircularCloudLayouter(center);
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_Centered()
    {
        var size = new Size(100, 50);
        var rectangle = layouter.PutNextRectangle(size);

        Assert.That(rectangle.Size, Is.EqualTo(size));
        Assert.That(rectangle.X + rectangle.Width / 2, Is.EqualTo(center.X));
        Assert.That(rectangle.Y + rectangle.Height / 2, Is.EqualTo(center.Y));
    }

    [Test]
    public void PutNextRectangle_RequestedSizes_Preserved()
    {
        var random = new Random(42);
        var requestedSizes = Enumerable.Range(0, 100)
            .Select(_ => new Size(random.Next(20, 80), random.Next(20, 80)))
            .ToList();

        foreach (var size in requestedSizes)
            layouter.PutNextRectangle(size);

        Assert.That(layouter.Rectangles, Has.Count.EqualTo(requestedSizes.Count));

        for (var i = 0; i < requestedSizes.Count; i++)
        {
            Assert.That(layouter.Rectangles[i].Size, Is.EqualTo(requestedSizes[i]),
                $"Rectangle {i} has an incorrect size");
        }
    }

    [Test]
    public void PutNextRectangle_Rectangles_NoIntersections()
    {
        var random = new Random(42);
        var rectangleCount = 100;

        for (var i = 0; i < rectangleCount; i++)
        {
            var size = new Size(random.Next(20, 80), random.Next(20, 80));
            layouter.PutNextRectangle(size);
        }

        var rectangles = layouter.Rectangles;

        Assert.That(rectangles, Has.Count.EqualTo(rectangleCount));

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False,
                    $"Rectangles {i} and {j} intersect: {rectangles[i]} and {rectangles[j]}");
            }
        }
    }

    [Test]
    public void PutNextRectangle_CloudShape_Circular()
    {
        var random = new Random(42);
        var rectangleCount = 100;

        for (var i = 0; i < rectangleCount; i++)
        {
            var size = new Size(random.Next(30, 60), random.Next(30, 60));
            layouter.PutNextRectangle(size);
        }

        var rectangles = layouter.Rectangles;
        var corners = GetRectangleCorners(rectangles);
        var hull = GetConvexHull(corners);
        var area = CalculatePolygonArea(hull);
        var perimeter = CalculatePolygonPerimeter(hull);
        var circularity = 4 * Math.PI * area / (perimeter * perimeter);

        Assert.That(circularity, Is.GreaterThan(MinCircularity),
            $"Cloud is not circular enough. Circularity: {circularity:F3}");
    }

    [Test]
    public void PutNextRectangle_CloudShape_Compact()
    {
        var random = new Random(42);
        var rectangleCount = 100;

        for (var i = 0; i < rectangleCount; i++)
        {
            var size = new Size(random.Next(20, 50), random.Next(20, 50));
            layouter.PutNextRectangle(size);
        }

        var rectangles = layouter.Rectangles;
        var rectanglesArea = rectangles.Sum(r => (long)r.Width * r.Height);

        var radius = GetCloudRadius(rectangles, center);
        var circleArea = Math.PI * radius * radius;
        var density = rectanglesArea / circleArea;

        Assert.That(density, Is.GreaterThan(MinDensity),
            $"Cloud is too sparse. Density: {density:F2}, expected more than {MinDensity}");
    }

    [TestCase(20, 20)]
    [TestCase(50, 10)]
    [TestCase(10, 50)]
    [TestCase(1, 1)]
    [TestCase(100, 100)]
    public void PutNextRectangle_DifferentSizes_Supported(int width, int height)
    {
        var size = new Size(width, height);

        var rectangle = layouter.PutNextRectangle(size);

        Assert.That(rectangle.Size, Is.EqualTo(size));
    }

    [TestCase(0, 10)]
    [TestCase(10, 0)]
    [TestCase(0, 0)]
    [TestCase(-10, 10)]
    [TestCase(10, -10)]
    [TestCase(-10, -10)]
    public void PutNextRectangle_InvalidSize_ArgumentExceptionThrown(int width, int height)
    {
        var invalidSize = new Size(width, height);
        var rectanglesBeforeCall = layouter.Rectangles.Count;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => layouter.PutNextRectangle(invalidSize));

        Assert.That(layouter.Rectangles, Has.Count.EqualTo(rectanglesBeforeCall),
            "Layouter should not add a rectangle when the size is invalid");
    }

    [Test]
    public void Rectangles_InterfaceAccess_Available()
    {
        ICircularCloudLayouter interfaceLayouter = layouter;
        var size = new Size(40, 30);

        var rectangle = interfaceLayouter.PutNextRectangle(size);

        Assert.That(interfaceLayouter.Rectangles, Has.Count.EqualTo(1));
        Assert.That(interfaceLayouter.Rectangles[0], Is.EqualTo(rectangle));
    }

    [Test]
    public void PutNextRectangle_OneCall_OneRectangleAdded()
    {
        var sizes = new[]
        {
            new Size(30, 40),
            new Size(50, 20),
            new Size(15, 60),
            new Size(45, 45)
        };

        foreach (var size in sizes)
            layouter.PutNextRectangle(size);

        Assert.That(layouter.Rectangles, Has.Count.EqualTo(sizes.Length));
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_CloudCenteredAtCenter()
    {
        var random = new Random(42);
        for (var i = 0; i < 100; i++)
            layouter.PutNextRectangle(new Size(random.Next(20, 60), random.Next(20, 60)));

        var bounds = GetCloudBounds(layouter.Rectangles);
        var cloudCenter = new Point(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2);

        var tolerance = Math.Max(bounds.Width, bounds.Height) / 10.0;

        Assert.That(Math.Abs(cloudCenter.X - center.X), Is.LessThan(tolerance),
            $"Cloud is shifted horizontally: cloud center X={cloudCenter.X}, expected ≈{center.X}");
        Assert.That(Math.Abs(cloudCenter.Y - center.Y), Is.LessThan(tolerance),
            $"Cloud is shifted vertically: cloud center Y={cloudCenter.Y}, expected ≈{center.Y}");
    }

    [TearDown]
    public void TearDown()
    {
        if (layouter == null)
            return;

        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            return;

        if (!layouter.Rectangles.Any())
            return;

        var testName = TestContext.CurrentContext.Test.Name;
        var directory = Path.Combine(Directory.GetCurrentDirectory(), "FailedTests");

        Directory.CreateDirectory(directory);

        var filePath = Path.Combine(directory, $"{testName}.png");

        CloudVisualizer.SaveToFile(layouter.Rectangles, center, filePath);

        TestContext.WriteLine(
            $"Tag cloud visualization saved to file {filePath}");
    }

    private static Rectangle GetCloudBounds(IReadOnlyList<Rectangle> rectangles)
    {
        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        var maxX = rectangles.Max(r => r.Right);
        var maxY = rectangles.Max(r => r.Bottom);

        return Rectangle.FromLTRB(minX, minY, maxX, maxY);
    }

    private static List<Point> GetRectangleCorners(
    IReadOnlyList<Rectangle> rectangles)
    {
        return rectangles
            .SelectMany(r => new[]
            {
            new Point(r.Left, r.Top),
            new Point(r.Right, r.Top),
            new Point(r.Right, r.Bottom),
            new Point(r.Left, r.Bottom)
            })
            .Distinct()
            .ToList();
    }

    private static List<Point> GetConvexHull(List<Point> points)
    {
        var sortedPoints = points
            .Distinct()
            .OrderBy(p => p.X)
            .ThenBy(p => p.Y)
            .ToList();

        if (sortedPoints.Count <= 1)
            return sortedPoints;

        var lower = new List<Point>();

        foreach (var point in sortedPoints)
        {
            while (lower.Count >= 2 &&
                   Cross(lower[^2], lower[^1], point) <= 0)
            {
                lower.RemoveAt(lower.Count - 1);
            }

            lower.Add(point);
        }

        var upper = new List<Point>();

        foreach (var point in sortedPoints.ReverseWithoutAllocation())
        {
            while (upper.Count >= 2 &&
                   Cross(upper[^2], upper[^1], point) <= 0)
            {
                upper.RemoveAt(upper.Count - 1);
            }

            upper.Add(point);
        }

        lower.RemoveAt(lower.Count - 1);
        upper.RemoveAt(upper.Count - 1);

        return lower.Concat(upper).ToList();
    }

    private static double Cross(Point origin, Point a, Point b)
    {
        return (double)(a.X - origin.X) * (b.Y - origin.Y)
             - (double)(a.Y - origin.Y) * (b.X - origin.X);
    }

    private static double CalculatePolygonArea(List<Point> polygon)
    {
        double area = 0;

        for (var i = 0; i < polygon.Count; i++)
        {
            var current = polygon[i];
            var next = polygon[(i + 1) % polygon.Count];

            area += (double)current.X * next.Y
                  - (double)next.X * current.Y;
        }

        return Math.Abs(area) / 2;
    }

    private static double CalculatePolygonPerimeter(List<Point> polygon)
    {
        double perimeter = 0;

        for (var i = 0; i < polygon.Count; i++)
        {
            var current = polygon[i];
            var next = polygon[(i + 1) % polygon.Count];

            var dx = next.X - current.X;
            var dy = next.Y - current.Y;

            perimeter += Math.Sqrt((double)dx * dx + (double)dy * dy);
        }

        return perimeter;
    }

    private static double GetCloudRadius(
    IReadOnlyList<Rectangle> rectangles, Point center)
    {
        var distances = new List<double>();

        foreach (var rectangle in rectangles)
        {
            foreach (var corner in new[]
            {
            new Point(rectangle.Left, rectangle.Top),
            new Point(rectangle.Right, rectangle.Top),
            new Point(rectangle.Right, rectangle.Bottom),
            new Point(rectangle.Left, rectangle.Bottom)
        })
            {
                var dx = (double)corner.X - center.X;
                var dy = (double)corner.Y - center.Y;
                distances.Add(Math.Sqrt(dx * dx + dy * dy));
            }
        }

        distances.Sort();
        //Не будем учитывать примерно 2 процента, так как краевой многоугольник выпирает и добавляет ненужное пространство
        var index = (int)(distances.Count * 0.98);
        return distances[Math.Min(index, distances.Count - 1)];
    }
}