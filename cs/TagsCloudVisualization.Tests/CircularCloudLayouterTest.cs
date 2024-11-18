using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Draw;
using TagsCloudVisualization.Extension;
using TagsCloudVisualization.RectangleGenerator;
using TagsCloudVisualization.Saver;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTest()
{
    private IReadOnlyList<Rectangle> rectanglesForCrashTest;
    
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var drawer = new RectangleDraftsman(1500, 1500);
            var filename = $"{TestContext.CurrentContext.WorkDirectory}\\{TestContext.CurrentContext.Test.Name}.png";
            drawer.CreateImage(rectanglesForCrashTest);
            drawer.SaveImageToFile(filename);

            Console.WriteLine($"Tag cloud visualization saved to file {filename}");
        }
    }

    [Test]
    public void PutNextRectangle_PlaceFirstRectangleAtCenter()
    {
        var center = new Point(1, 1);
        var layouter = new CircularCloudLayouter(center);
        var nextRectangle = layouter.PutNextRectangle(new Size(10, 10));

        rectanglesForCrashTest = layouter.Rectangles;
        nextRectangle.GetCenter().Should().Be(center);
    }

    [TestCase(0, 0)]
    [TestCase(-1, -1)]
    public void PutNextRectangle_WhenIncorrectSize_Throw(int sizeX, int sizeY)
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);

        Action action = () => layouter.PutNextRectangle(new Size(sizeX, sizeY));
        action.Should().Throw<ArgumentException>().WithMessage("Width and height should be greater than zero.");
    }

    [TestCase(1, 1, 10)]
    public void PutNextRectangle_AddRectangles(int sizeX, int sizeY, int count)
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);

        for (var i = 0; i < count; i++)
        {
            layouter.PutNextRectangle(new Size(sizeX, sizeY));
        }

        rectanglesForCrashTest = layouter.Rectangles;
        layouter.Rectangles.Count.Should().Be(count);
    }

    [TestCase(30)]
    [TestCase(50)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PutNextRectangle_CreateLayoutWithoutIntersections(int countRectangles)
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var rectangles = RectangleGenerator.GenerateRandomRectangles(countRectangles).ToList();
        layouter.PutRectangles(rectangles);
        rectanglesForCrashTest = layouter.Rectangles;

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                layouter.Rectangles[i].IntersectsWith(layouter.Rectangles[j]).Should().BeFalse();
            }
        }
    }

    [TestCase(0, 0, 10, Description = "Center at the center of the coordinate axis")]
    [TestCase(0, 0, 100, Description = "Center at the center of the coordinate axis")]
    [TestCase(0, 0, 1000, Description = "Center at the center of the coordinate axis")]
    [TestCase(100, 100, 10, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(100, 100, 100, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(100, 100, 1000, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(-100, 100, 10, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, 100, 100, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, 100, 1000, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, -100, 10, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, -100, 100, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, -100, 1000, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, 100, 10, Description = "Center in the fourth quadrant of the coordinate axis")]
    [TestCase(-100, 100, 100, Description = "Center in the fourth quadrant of the coordinate axis")]
    [TestCase(-100, 100, 1000, Description = "Center in the fourth quadrant of the coordinate axis")]
    public void PutNextRectangle_CreateLaoyoutWithMore40PercentRoundLayout_OnRandomNumberRectangles(
        int xCenter,
        int yCenter,
        int countRectangles)
    {
        var center = new Point(xCenter, yCenter);
        var randomRectangles = RectangleGenerator.GenerateRandomRectangles(countRectangles);
        var layouter = new CircularCloudLayouter(center);
        layouter.PutRectangles(randomRectangles);

        var maxX = GetMaxX(layouter.Rectangles);
        var minX = GetMinX(layouter.Rectangles);
        var maxY = GetMaxY(layouter.Rectangles);
        var minY = GetMinY(layouter.Rectangles);

        // Вычитаю центр, чтобы сместить нашу окружность в начало координат
        var radius = Max(maxX - center.X, minX - center.X, maxY - center.Y, minY - center.Y);

        var deviationMaxXFromRadius = Math.Abs(radius - maxX);
        var deviationMinXFromRadius = Math.Abs(radius - minX);
        var deviationMaxYFromRadius = Math.Abs(radius - maxY);
        var deviationMinYFromRadius = Math.Abs(radius - minY);
        var numberOfPointsToMeasure = 4.0;
        var averageSizeOfRectangles = layouter.Rectangles.Average(r => r.Height * r.Width);

        var percentageOfRoundLayout =
            1 - (deviationMaxXFromRadius + deviationMinXFromRadius + deviationMaxYFromRadius +
                deviationMinYFromRadius) / numberOfPointsToMeasure / averageSizeOfRectangles;

        percentageOfRoundLayout.Should().BeInRange(0.4, 1);
    }

    [TestCase(0, 0, 10, Description = "Center at the center of the coordinate axis")]
    [TestCase(0, 0, 100, Description = "Center at the center of the coordinate axis")]
    [TestCase(0, 0, 1000, Description = "Center at the center of the coordinate axis")]
    [TestCase(100, 100, 10, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(100, 100, 100, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(100, 100, 1000, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(-100, 100, 10, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, 100, 100, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, 100, 1000, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, -100, 10, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, -100, 100, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, -100, 1000, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, 100, 10, Description = "Center in the fourth quadrant of the coordinate axis")]
    [TestCase(-100, 100, 100, Description = "Center in the fourth quadrant of the coordinate axis")]
    [TestCase(-100, 100, 1000, Description = "Center in the fourth quadrant of the coordinate axis")]
    public void PutNextRectangle_CreateLaoyoutWithOver75PercentDensity_OnRandomNumberRectangles(
        int xCenter,
        int yCenter,
        int countRectangles)
    {
        var center = new Point(xCenter, yCenter);
        var randomRectangles = RectangleGenerator.GenerateRandomRectangles(countRectangles);
        var layouter = new CircularCloudLayouter(center);
        layouter.PutRectangles(randomRectangles);
        rectanglesForCrashTest = layouter.Rectangles;

        var distancesFromCenterToRectangles = GetDistancesFromCenterToRectangles(layouter.Rectangles, center);
        var averageDistanceFromCenterToRectangles = distancesFromCenterToRectangles.Average();
        var radiusOfCircleAroundRectangles = GetMostDistantFromCenterToRectangles(layouter.Rectangles, center);
        var areaOfCircle = GetAreaOfCircle(radiusOfCircleAroundRectangles);
        var areaOfRectangles = GetAreaOfRectangles(layouter.Rectangles);
        var densityCoefficient = GetDensityCoefficient(areaOfRectangles, areaOfCircle,
            averageDistanceFromCenterToRectangles, radiusOfCircleAroundRectangles);

        densityCoefficient.Should().BeLessThan(0.36);
    }

    private static IEnumerable<double> GetDistancesFromCenterToRectangles(
        IEnumerable<Rectangle> rectangles,
        Point center) =>
        rectangles
            .Select(r => Math.Sqrt(Math.Pow(r.Location.X - center.X, 2) + Math.Pow(r.Location.Y - center.Y, 2)));

    private static double GetMostDistantFromCenterToRectangles(IEnumerable<Rectangle> rectangles, Point center)
    {
        var mostDistantToLeftTopRectangles = rectangles
            .Select(r => Math.Max(Math.Abs(r.Location.X - center.X),
                Math.Abs(r.Location.Y - center.Y)))
            .Max();
        var mostDistantToRightTopRectangles = rectangles
            .Select(r => Math.Max(Math.Abs(r.Location.X + r.Width - center.X),
                Math.Abs(r.Location.Y - center.Y)))
            .Max();
        var mostDistantToLeftBottomRectangles = rectangles
            .Select(r => Math.Max(Math.Abs(r.Location.X - center.X),
                Math.Abs(r.Location.Y - r.Height - center.Y)))
            .Max();
        var mostDistantToRightBottomRectangles = rectangles
            .Select(r => Math.Max(Math.Abs(r.Location.X + r.Width - center.X),
                Math.Abs(r.Location.Y - r.Height - center.Y)))
            .Max();

        var mostDistant = Max(mostDistantToLeftTopRectangles, mostDistantToRightTopRectangles,
            mostDistantToLeftBottomRectangles, mostDistantToRightBottomRectangles);
        
        return Math.Sqrt(Math.Pow(mostDistant, 2) +
            Math.Pow(mostDistant, 2));
    }

    private static double GetAreaOfCircle(double radius) => Math.PI * Math.Pow(radius, 2);

    private static int GetAreaOfRectangles(IEnumerable<Rectangle> rectangles) =>
        rectangles.Sum(r => r.Width * r.Height);

    private static double GetDensityCoefficient(
        double areaOfRectangles,
        double areaOfCircle,
        double averageDistance,
        double radius) =>
        areaOfRectangles / areaOfCircle * (1 - averageDistance / radius);

    private static int GetMaxX(IEnumerable<Rectangle> rectangles) => rectangles.Max(r => r.Location.X);

    private static int GetMaxY(IEnumerable<Rectangle> rectangles) => rectangles.Max(r => r.Location.Y);

    private static int GetMinX(IEnumerable<Rectangle> rectangles) => rectangles.Min(r => r.Location.X);

    private static int GetMinY(IEnumerable<Rectangle> rectangles) => rectangles.Min(r => r.Location.Y);

    private static int Max(int p1, int p2, int p3, int p4) => Math.Max(Math.Max(p1, p2), Math.Max(p3, p4));
}