using FluentAssertions;
using SkiaSharp;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private const double minimumRelativeDensity = .5;
    private const double maximumCenterOffset = 60;
    private const int pictureBorderSize = 20;
    private const int lineWidth = 5;

    private Rectangle[]? testedTagCloud;

    private static IEnumerable<Size> GetWrongSizes()
    {
        var values = new[] { -1, 0, 1 };
        return values
            .SelectMany(t => values
                .Select(f => new Size(t, f)))
            .Where(t => t.Width != 1 || t.Height != 1);
    }

    private static IEnumerable<Size> GetRandomizedTestSizes()
    {
        return
        [
            new Size(684, 76),
            new Size(564, 94),
            new Size(666, 74),
            new Size(297, 66),
            new Size(121, 22),
            new Size(123, 82),
            new Size(640, 80),
            new Size(222, 74),
            new Size(138, 92),
            new Size(205, 82),
            new Size(476, 56),
            new Size(544, 68),
            new Size(96, 32),
            new Size(84, 28),
            new Size(216, 72),
            new Size(272, 34),
            new Size(36, 36),
            new Size(80, 32),
            new Size(574, 82),
            new Size(540, 72),
            new Size(396, 44),
            new Size(407, 74),
            new Size(180, 36),
            new Size(250, 100),
            new Size(287, 82),
            new Size(410, 82),
            new Size(94, 94),
            new Size(66, 44),
            new Size(595, 70),
            new Size(270, 30),
            new Size(224, 56),
            new Size(114, 38),
            new Size(252, 84),
            new Size(90, 90),
            new Size(555, 74),
            new Size(156, 52),
            new Size(448, 64),
            new Size(266, 38),
            new Size(940, 94),
            new Size(560, 56),
            new Size(51, 34),
            new Size(84, 24),
            new Size(576, 64),
            new Size(165, 66),
            new Size(648, 72),
            new Size(40, 20),
            new Size(282, 94),
            new Size(544, 68),
            new Size(132, 22),
            new Size(330, 66)
        ];
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Failed)
            return;

        var image = DrawTagsCloud(testedTagCloud);

        var directory = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "FailedTestVisualizations"));
        var path = Path.Combine(directory.FullName, $"{TestContext.CurrentContext.Test.Name}-{Path.GetRandomFileName()}.png");
        using var file = File.Create(path);
        image.Encode().SaveTo(file);

        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    [TestCaseSource(nameof(GetWrongSizes))]
    public void PutNextRectangle_Should_ThrowArgumentExceptionIfSizeWidthOrHeightLessOrEqualZero(
        Size rectangleSize)
    {
        var layouter = new CircularCloudLayouter(new Point());

        var act = () => layouter.PutNextRectangle(rectangleSize);

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_Should_ReturnRectangleWithSameSize()
    {
        var layouter = new CircularCloudLayouter(new Point());
        var sizes = GetRandomizedTestSizes().ToArray();

        var rectangles = CreateAndStoreTagCloud(layouter, sizes);

        rectangles.Select(t => t.Size).Should().Equal(sizes);
    }

    [Test]
    public void Rectangles_ShouldNot_Intersect()
    {
        var layouter = new CircularCloudLayouter(new Point());
        var sizes = GetRandomizedTestSizes();

        var rectangles = CreateAndStoreTagCloud(layouter, sizes);

        for (var i = 0; i < rectangles.Length; i++)
            for (var j = i + 1; j < rectangles.Length; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
    }

    [Test]
    public void CloudDensity_Should_BeGreatOrEqualMinimumRelativeDensity_And_BeCloseToCircle()
    {
        var layouter = new CircularCloudLayouter(new Point());
        var sizes = GetRandomizedTestSizes();

        var rectangles = CreateAndStoreTagCloud(layouter, sizes);
        var boundingRectangle = GetBoundingRectangle(rectangles);
        var cloudRadius = (boundingRectangle.Width + boundingRectangle.Height) / 2.0 / 2.0;
        var actualRelativeDensity =
            rectangles.Sum(t => t.Width * t.Height) / (Math.PI * cloudRadius * cloudRadius);

        actualRelativeDensity.Should().BeGreaterThanOrEqualTo(minimumRelativeDensity);
    }

    [Test]
    public void CloudCenterOffset_Should_BeLessOrEqualMaximumCenterOffset()
    {
        var center = new Point(10, 3);
        var layouter = new CircularCloudLayouter(center);
        var sizes = GetRandomizedTestSizes();

        var rectangles = CreateAndStoreTagCloud(layouter, sizes);
        var boundingRectangle = GetBoundingRectangle(rectangles);
        var actualCenter = boundingRectangle.Location + boundingRectangle.Size / 2;
        var actualCenterOffset = Math.Sqrt(
            (center.X - actualCenter.X) * (center.X - actualCenter.X)
            + (center.Y - actualCenter.Y) * (center.Y - actualCenter.Y));

        actualCenterOffset.Should().BeLessThanOrEqualTo(maximumCenterOffset);
    }

    private Rectangle[] CreateAndStoreTagCloud(CircularCloudLayouter layouter, IEnumerable<Size> sizes)
    {
        testedTagCloud = sizes.Select(layouter.PutNextRectangle).ToArray();
        return testedTagCloud;
    }

    private static Rectangle GetBoundingRectangle(IEnumerable<Rectangle> rects)
    {
        var right = int.MinValue;
        var top = int.MaxValue;
        var left = int.MaxValue;
        var bottom = int.MinValue;

        foreach (var rectangle in rects)
        {
            right = int.Max(right, rectangle.Right);
            top = int.Min(top, rectangle.Top);
            left = int.Min(left, rectangle.Left);
            bottom = int.Max(bottom, rectangle.Bottom);
        }

        var width = right - left;
        var height = bottom - top;
        return new(left, top, width, height);
    }

    private static SKImage DrawTagsCloud(Rectangle[] rectangles)
    {
        var boundingRectangle = GetBoundingRectangle(rectangles);
        var pictureOrigin = boundingRectangle.Location - new Size(pictureBorderSize, pictureBorderSize);

        var imageInfo = new SKImageInfo(
            width: boundingRectangle.Width + 2 * pictureBorderSize,
            height: boundingRectangle.Height + 2 * pictureBorderSize,
            colorType: SKColorType.Rgb888x,
            alphaType: SKAlphaType.Opaque);
        using var surface = SKSurface.Create(imageInfo);
        var canvas = surface.Canvas;

        canvas.Clear(SKColor.Parse("#000000"));
        DrawRectangles(rectangles, pictureOrigin, canvas);

        return surface.Snapshot();
    }

    private static void DrawRectangles(Rectangle[] rectangles, Point pictureOrigin, SKCanvas canvas)
    {
        var rand = new Random();
        foreach (var rectangle in rectangles)
        {
            var color = new byte[3];
            rand.NextBytes(color);
            var lineColor = new SKColor(
                red: color[0],
                green: color[1],
                blue: color[2]);

            var paint = new SKPaint
            {
                Color = lineColor,
                StrokeWidth = lineWidth,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke
            };

            canvas.DrawRect(
                rectangle.X - pictureOrigin.X,
                rectangle.Y - pictureOrigin.Y,
                rectangle.Width,
                rectangle.Height,
                paint);
        }
    }
}
