using System.Drawing;
using Core;
using FluentAssertions;

namespace Tests;

public class TDDTests
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
            "Первый прямоугольник должен всставляться ровно в центр");
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
        for (var j = i + 1; j < rectangles.Length; j++)
        {
            rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse(
                "прямоугольники {0} и {1} не должны перекрываться", i, j);
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
        
        // Act
        var rectangle1 = layouter.PutNextRectangle(size);
        var rectangle2 = layouter.PutNextRectangle(size);
        
        // Assert
        var distance = GetGapBetweenRectangles(rectangle1, rectangle2);

        distance.Should().BeLessThanOrEqualTo(2,
            "зазор между первыми двумя прямоугольниками не должен превышать 2 пикселей");
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