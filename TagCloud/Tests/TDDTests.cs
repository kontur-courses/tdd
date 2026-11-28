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
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var rectangleSize = new Size(10, 10);
        
        // Act
        var rectangle = layouter.PutNextRectangle(rectangleSize);
        
        // Assert
        rectangle.Should().Be(new Rectangle(-5, -5, 10, 10));
    }
}