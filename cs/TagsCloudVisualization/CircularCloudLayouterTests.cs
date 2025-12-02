using CloudLayouterVisualizer;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouterTests
{
    private const int CreatedRectanglesLimit = 200;

    private static readonly Size DefaultRectangleSize = new(30, 20);
    private static readonly Size MinRectangleSize = new(1, 1);
    private static readonly Point DefaultCenter = Point.Empty;

    private static readonly Size SmallRectangleSize =
        new(DefaultRectangleSize.Width / 2, DefaultRectangleSize.Height / 2);

    private static readonly Size LargeRectangleSize =
        new(DefaultRectangleSize.Width * 2, DefaultRectangleSize.Height * 2);

    private const double ExpectedMinDensity = 0.5;
    private const double MaxAllowedCenterOffsetRatio = 0.1;
    private const double MinAllowedAspectRatio = 0.75;
    private const double MaxAllowedAspectRatio = 1.25;

    private const string FailureImagesDirName = "TagCloudFailures";
    private const string FailureImagesFileExtension = "png";
    private const string FileNameTimestampFormat = "yyyyMMdd_HHmmssfff";
    private const string GuidFormat = "N";
    private const int VisualizationScale = 5;
    private const int VisualizationPadding = 10;

    private CircularCloudLayouter? _layouter;

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            return;

        SaveFailureVisualization();
    }

    private static Rectangle GetBoundingRectangle(Rectangle[] rectangles)
    {
        var left = rectangles.Min(rect => rect.Left);
        var top = rectangles.Min(rect => rect.Top);
        var right = rectangles.Max(rect => rect.Right);
        var bottom = rectangles.Max(rect => rect.Bottom);

        return Rectangle.FromLTRB(left, top, right, bottom);
    }

    private static Point GetRectangleCenter(Rectangle rectangle)
    {
        var centerX = rectangle.Left + rectangle.Width / 2;
        var centerY = rectangle.Top + rectangle.Height / 2;
        return new Point(centerX, centerY);
    }

    private Rectangle[] CreateRectangles(Point center, int count, Size[]? sizes = null)
    {
        _layouter = new CircularCloudLayouter(center);
        var rectangles = new Rectangle[count];

        sizes ??=
        [
            new Size(DefaultRectangleSize.Width / 2, DefaultRectangleSize.Height / 2),
            DefaultRectangleSize,
            new Size(DefaultRectangleSize.Width * 2, DefaultRectangleSize.Height * 2)
        ];

        for (var i = 0; i < count; i++)
        {
            var size = sizes[i % sizes.Length];
            rectangles[i] = _layouter.PutNextRectangle(size);
        }

        return rectangles;
    }

    private static void AssertNoIntersections(Rectangle[] rectangles)
    {
        for (var i = 0; i < rectangles.Length; i++)
            for (var j = i + 1; j < rectangles.Length; j++)
            {
                rectangles[i]
                    .IntersectsWith(rectangles[j])
                    .Should()
                    .BeFalse($"прямоугольники {rectangles[i]} и {rectangles[j]} пересекаются");
            }
    }

    private void SaveFailureVisualization()
    {
        try
        {
            var ctx = TestContext.CurrentContext;

            if (_layouter == null || _layouter.CreatedRectangles == null || _layouter.CreatedRectangles.Count == 0)
            {
                TestContext.Out.WriteLine("Тест завершился с ошибкой до создания прямоугольников. Визуализация облака не была сохранена");
                return;
            }

            var outDir = Path.Combine(ctx.WorkDirectory, FailureImagesDirName);
            Directory.CreateDirectory(outDir);

            var fileName = $"{DateTime.UtcNow.ToString(FileNameTimestampFormat)}_{Guid.NewGuid().ToString(GuidFormat)}.{FailureImagesFileExtension}";
            var filePath = Path.Combine(outDir, fileName);

            new CloudVisualizer([.. _layouter.CreatedRectangles], filePath, VisualizationScale, VisualizationPadding).Visualize();

            TestContext.Out.WriteLine($"Визуализация облака сохранена в файл: {filePath} (тест: {ctx.Test.Name})");
        }
        catch (Exception e)
        {
            TestContext.Out.WriteLine($"Не удалось сохранить визуализацию облака: {e.Message}");
        }
    }

    [TestCase(0, 0)]
    [TestCase(100, 50)]
    [TestCase(-50, 75)]
    public void PutNextRectangle_FirstRectangle_ShouldBeInCenter(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        _layouter = new CircularCloudLayouter(center);

        var rectangle = _layouter.PutNextRectangle(DefaultRectangleSize);
        var rectangleCenter = GetRectangleCenter(rectangle);

        rectangleCenter.Should().Be(center);
    }

    [TestCase(0, 0)]
    [TestCase(150, -100)]
    public void PutNextRectangle_ManyRectangles_DoNotIntersect(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var rectangles = CreateRectangles(center, CreatedRectanglesLimit);

        AssertNoIntersections(rectangles);
    }

    [Test(Description = "Облако должно быть достаточно плотным")]
    public void PutNextRectangle_ManyRectangles_ShouldBeDense()
    {
        var rectangles = CreateRectangles(DefaultCenter, CreatedRectanglesLimit);

        var totalArea = rectangles.Sum(rect => rect.Width * rect.Height);

        var boundingRectangle = GetBoundingRectangle(rectangles);
        var boundingArea = boundingRectangle.Width * boundingRectangle.Height;

        var density = (double)totalArea / boundingArea;

        density.Should().BeGreaterThan(ExpectedMinDensity);
    }

    [Test(Description = "Облако растет симметрично относительно центра")]
    public void PutNextRectangle_ManyRectangles_CloudSurroundsCenter()
    {
        var center = DefaultCenter;
        var rectangles = CreateRectangles(center, CreatedRectanglesLimit);

        var boundingRectangle = GetBoundingRectangle(rectangles);

        boundingRectangle.Contains(center).Should().BeTrue();

        var centers = rectangles.Select(GetRectangleCenter).ToArray();

        var anyOnLeft = centers.Any(p => p.X <= center.X);
        var anyOnRight = centers.Any(p => p.X >= center.X);
        var anyOnTop = centers.Any(p => p.Y <= center.Y);
        var anyOnBottom = centers.Any(p => p.Y >= center.Y);

        anyOnLeft.Should().BeTrue("облако должно быть слева от центра");
        anyOnRight.Should().BeTrue("облако должно быть справа от центра");
        anyOnTop.Should().BeTrue("облако должно быть выше центра");
        anyOnBottom.Should().BeTrue("облако должно быть ниже центра");
    }

    [Test(Description = "Облако получается примерно круглой формы")]
    public void PutNextRectangle_ManyRectangles_ShapeHasCircularAspectRatio()
    {
        var center = DefaultCenter;
        var rectangles = CreateRectangles(center, CreatedRectanglesLimit);

        var boundingRectangle = GetBoundingRectangle(rectangles);

        var aspectRatio = (double)boundingRectangle.Width / boundingRectangle.Height;

        aspectRatio.Should().BeInRange(MinAllowedAspectRatio, MaxAllowedAspectRatio,
            "облако должно иметь близкое к квадрату соотношение сторон ограничивающего прямоугольника");

        var centers = rectangles.Select(GetRectangleCenter).ToArray();
        const int sectorCount = 8;
        var occupiedSectors = new bool[sectorCount];

        foreach (var point in centers)
        {
            var dx = point.X - center.X;
            var dy = point.Y - center.Y;

            if (dx == 0 && dy == 0)
            {
                occupiedSectors[0] = true;
                continue;
            }

            var angle = Math.Atan2(dy, dx);
            var normalized = (angle + 2 * Math.PI) % (2 * Math.PI);
            var index = (int)(normalized / (2 * Math.PI) * sectorCount);
            if (index >= sectorCount)
                index = sectorCount - 1;

            occupiedSectors[index] = true;
        }

        occupiedSectors.Count(s => s)
            .Should()
            .Be(sectorCount, "прямоугольники должны равномерно распределяться по окружности вокруг центра");
    }

    [Test(Description = "Центр облака не сильно смещается относительно заданного центра")]
    public void PutNextRectangle_ManyRectangles_CenterDoesNotShiftMuch()
    {
        var center = DefaultCenter;
        var rectangles = CreateRectangles(center, CreatedRectanglesLimit);

        var boundingRectangle = GetBoundingRectangle(rectangles);
        var boundingCenter = GetRectangleCenter(boundingRectangle);

        var dx = boundingCenter.X - center.X;
        var dy = boundingCenter.Y - center.Y;

        var distanceFromRequiredCenter = Math.Sqrt(dx * dx + dy * dy);

        var maxSize = Math.Max(boundingRectangle.Width, boundingRectangle.Height);

        var centerOffsetRatio = distanceFromRequiredCenter / maxSize;

        centerOffsetRatio.Should().BeLessThanOrEqualTo(MaxAllowedCenterOffsetRatio);
    }

    [Test(Description = "Прямоугольники разных размеров, сначала маленькие, затем большие, не пересекаются")]
    public void PutNextRectangle_SmallThenLargeRectangles_DoNotIntersect()
    {
        var center = DefaultCenter;

        var sizes = Enumerable
            .Range(0, CreatedRectanglesLimit)
            .Select(i => i < CreatedRectanglesLimit / 2 ? SmallRectangleSize : LargeRectangleSize)
            .ToArray();

        var rectangles = CreateRectangles(center, CreatedRectanglesLimit, sizes);

        AssertNoIntersections(rectangles);
    }

    [Test(Description = "Прямоугольники разных размеров, сначала большие, затем маленькие, не пересекаются")]
    public void PutNextRectangle_LargeThenSmallRectangles_DoNotIntersect()
    {
        var center = DefaultCenter;

        var sizes = Enumerable
            .Range(0, CreatedRectanglesLimit)
            .Select(i => i < CreatedRectanglesLimit / 2 ? SmallRectangleSize : LargeRectangleSize)
            .ToArray();

        var rectangles = CreateRectangles(center, CreatedRectanglesLimit, sizes);

        AssertNoIntersections(rectangles);
    }

    [Test(Description = "Прямоугольники разных размеров, большие и маленькие, не пересекаются")]
    public void PutNextRectangle_RectanglesWithDifferentSizes_DoNotIntersect()
    {
        var center = DefaultCenter;

        var sizes = Enumerable
            .Range(0, CreatedRectanglesLimit)
            .Select(i => i % 2 == 0 ? SmallRectangleSize : LargeRectangleSize)
            .ToArray();

        var rectangles = CreateRectangles(center, CreatedRectanglesLimit, sizes);

        AssertNoIntersections(rectangles);
    }

    [Test(Description = "Пограничный тест: прямоугольник минимального размера корректно размещается в центре")]
    public void PutNextRectangle_MinSizeRectangle_ShouldBePlacedInCenter()
    {
        var center = new Point(10, -20);
        _layouter = new CircularCloudLayouter(center);

        var rectangle = _layouter.PutNextRectangle(MinRectangleSize);

        var rectangleCenter = GetRectangleCenter(rectangle);

        rectangleCenter.Should().Be(center);
    }

    [Test(Description = "Пограничный тест: много прямоугольников минимального размера не пересекаются")]
    public void PutNextRectangle_ManyMinSizeRectangles_DoNotIntersect()
    {
        var center = new Point(1000, -1000);

        var sizes = Enumerable
            .Repeat(MinRectangleSize, CreatedRectanglesLimit)
            .ToArray();

        var rectangles = CreateRectangles(center, CreatedRectanglesLimit, sizes);

        AssertNoIntersections(rectangles);
    }
}
