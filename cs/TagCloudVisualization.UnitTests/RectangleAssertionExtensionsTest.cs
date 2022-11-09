using System.Drawing;
using FluentAssertions;

namespace TagCloudVisualization.UnitTests;

[TestFixture]
public class RectangleAssertionExtensionsTest
{
    [Test]
    public void TouchesWith_True_TouchesRectangles()
    {
        var a = new Rectangle(0, 0, 10, 10);
        var b = new Rectangle(10, 0, 10, 10);
        
        a.TouchesWith(b).Should().BeTrue();
    }

    [Test]
    public void TouchesWith_False_NotTouchesRectangles()
    {
        var a = new Rectangle(0, 0, 10, 10);
        var b = new Rectangle(20, 0, 10, 10);
        
        a.TouchesWith(b).Should().BeFalse();
    }

    [Test]
    public void TouchesWith_False_IntersectsRectangles()
    {
        var a = new Rectangle(0, 0, 10, 10);
        var b = new Rectangle(9, 0, 10, 10);
        
        a.TouchesWith(b).Should().BeFalse();
    }
}