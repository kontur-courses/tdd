using System.Drawing;
using Core;
using FluentAssertions;

namespace Tests;

public class CircularCloudLayouterTests
{
    [Test]
    public void PutNextRectangle_CentersFirstRectangle()
    {
        // Arrange
        var center = new Point(100, 50);
        var layouter = new CircularCloudLayouter(center);
        var rectangleSize = new Size(40, 20);
        
        // Act
        var rectangle = layouter.PutNextRectangle(rectangleSize);
        
        // Assert
        rectangle.Should().Be(new Rectangle(80, 40, 40, 20),
            "первый прямоугольник должен располагаться ровно в центре");
    }
    
    [Test]
    public void PutNextRectangle_DoesNotIntersectAnyPlacedRectangle()
    {
        // Arrange
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var sizes = new[]
        {
            new Size(40, 20),
            new Size(20, 40),
            new Size(30, 10),
            new Size(15, 25)
        };

        // Act
        var rectangles = sizes.Select(layouter.PutNextRectangle).ToArray();

        // Assert
        for (var i = 0; i < rectangles.Length; i++)
        {
            for (var j = i + 1; j < rectangles.Length; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse(
                    "прямоугольники {0} и {1} не должны перекрываться", i, j);
            }
        }
    }
    
    [Test]
    public void PutNextRectangle_PreservesRequestedSizeForEachRectangle()
    {
        // Arrange
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var sizes = new[]
        {
            new Size(40, 20),
            new Size(20, 40),
            new Size(30, 10),
            new Size(15, 25)
        };

        // Act
        var rectangles = sizes.Select(layouter.PutNextRectangle).ToArray();

        // Assert
        rectangles.Select(rectangle => rectangle.Size)
            .Should().Equal(sizes,
                "добавление прямоугольников не должно менять их размер");
    }

    [Test]
    public void PutNextRectangle_PlacesSecondRectangleCloseToFirst()
    {
        // Arrange
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var size = new Size(20, 20);
        const double maxGapInPixels = 2;
        
        // Act
        var rectangle1 = layouter.PutNextRectangle(size);
        var rectangle2 = layouter.PutNextRectangle(size);
        
        // Assert
        var distance = GetGapBetweenRectangles(rectangle1, rectangle2);

        distance.Should().BeLessThanOrEqualTo(maxGapInPixels,
            "зазор между первыми двумя прямоугольниками не должен превышать {0} пикселей", maxGapInPixels);
    }
    
    [Test]
    public void PutNextRectangle_DistributesRectanglesAcrossAllFourQuadrants()
    {
        // Arrange
        var center = new Point(100, 100);
        var layouter = new CircularCloudLayouter(center);
        var size = new Size(20, 20);
        const int rectangleCount = 50;

        // Act
        var rectangles = Enumerable.Range(0, rectangleCount)
            .Select(_ => layouter.PutNextRectangle(size))
            .ToArray();

        // Assert
        var rectangleCenters = rectangles
            .Select(rectangle => new Point(
                rectangle.Left + rectangle.Width / 2,
                rectangle.Top + rectangle.Height / 2))
            .ToArray();

        rectangleCenters.Should().Contain(
            point => point.X < center.X && point.Y < center.Y,
            "хотя бы один прямоугольник должен быть слева сверху");

        rectangleCenters.Should().Contain(
            point => point.X > center.X && point.Y < center.Y,
            "хотя бы один прямоугольник должен быть справа сверху");

        rectangleCenters.Should().Contain(
            point => point.X < center.X && point.Y > center.Y,
            "хотя бы один прямоугольник должен быть слева снизу");

        rectangleCenters.Should().Contain(
            point => point.X > center.X && point.Y > center.Y,
            "хотя бы один прямоугольник должен быть справа снизу");
    }
    
    [Test]
    public void PutNextRectangle_FormsApproximatelyCircularCloud()
    {
        // Arrange
        var center = new Point(100, 100);
        var layouter = new CircularCloudLayouter(center);
        var size = new Size(20, 20);
        const int rectangleCount = 100;
        const double maxExtentRatio = 1.30;

        var directions = new (int X, int Y)[]
        {
            (1, 0), (-1, 0), (0, 1), (0, -1),
            (1, 1), (1, -1), (-1, 1), (-1, -1)
        };

        // Act
        var rectangles = Enumerable.Range(0, rectangleCount)
            .Select(_ => layouter.PutNextRectangle(size))
            .ToArray();

        // Assert
        var corners = rectangles.SelectMany(rectangle => new[]
        {
            new Point(rectangle.Left, rectangle.Top),
            new Point(rectangle.Right, rectangle.Top),
            new Point(rectangle.Left, rectangle.Bottom),
            new Point(rectangle.Right, rectangle.Bottom)
        }).ToArray();

        var extents = directions.Select(direction =>
        {
            var directionLength = Math.Sqrt(
                direction.X * direction.X + direction.Y * direction.Y);

            return corners.Max(corner =>
                ((corner.X - center.X) * direction.X +
                 (corner.Y - center.Y) * direction.Y) / directionLength);
        }).ToArray();

        extents.Max().Should().BeLessThanOrEqualTo(extents.Min() * maxExtentRatio,
            "вылеты облака от центра в восьми направлениях должны отличаться не более чем на {0:P0}%", (maxExtentRatio - 1) * 100);
    }
    
    [Test]
    public void PutNextRectangle_PacksRectanglesDensely()
    {
        // Arrange
        var center = new Point(100, 100);
        var layouter = new CircularCloudLayouter(center);
        var size = new Size(20, 20);
        const int rectangleCount = 100;
        const double minimumDensity = 0.50;

        // Act
        var rectangles = Enumerable.Range(0, rectangleCount)
            .Select(_ => layouter.PutNextRectangle(size))
            .ToArray();

        // Assert
        var radiusSquared = rectangles.Max(rectangle =>
        {
            // У самого дальнего угла максимальное смещение по обеим осям.
            var dx = Math.Max(Math.Abs((double)rectangle.Left - center.X),
                Math.Abs((double)rectangle.Right - center.X));
            var dy = Math.Max(Math.Abs((double)rectangle.Top - center.Y),
                Math.Abs((double)rectangle.Bottom - center.Y));

            return dx * dx + dy * dy;
        });

        var rectanglesArea = rectangles.Sum(rectangle =>
            (double)rectangle.Width * rectangle.Height);

        var enclosingCircleArea = Math.PI * radiusSquared;
        var density = rectanglesArea / enclosingCircleArea;

        density.Should().BeGreaterThanOrEqualTo(minimumDensity,
            "прямоугольники должны занимать хотя бы {0:P0} площади круга, в который помещается облако", minimumDensity);
    }

    private static double GetGapBetweenRectangles(Rectangle rect1, Rectangle rect2)
    {
        var horizontalGap = Math.Max(0, Math.Max(
            rect2.Left - rect1.Right,
            rect1.Left - rect2.Right));

        var verticalGap = Math.Max(0, Math.Max(
            rect2.Top - rect1.Bottom,
            rect1.Top - rect2.Bottom));

        return Math.Sqrt(
            horizontalGap * horizontalGap + verticalGap * verticalGap);
    }
}