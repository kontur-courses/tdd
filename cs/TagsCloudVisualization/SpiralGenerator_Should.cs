using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization;

public class SpiralGenerator_Should
{
    [Test]
    public void ThrowArgumentException_WhenNegativeRadiusLambda()
    {
        var spiralGenCreation = () => new SpiralGenerator(radiusDelta: -1);
        spiralGenCreation.Should().Throw<ArgumentException>();
    }

    [TestCase(0, 0, 0, 0)]
    [TestCase(5, 4, 10, 3.0)]
    [TestCase(-5, -2, 50, 3.14)]
    public void GetCenterPoint_WhenCalledFirstTime(int centerX, int centerY, int radiusLambda, double angleLambda)
    {
        var centerPoint = new Point(centerX, centerY);
        var spiral = new SpiralGenerator(centerPoint, radiusLambda, angleLambda);
        spiral.GetNextPoint().Should().NotBeNull().And.Be(new Point(centerX, centerY));
    }

    [TestCase(0, 0, 1)]
    [TestCase(5, 4, 10)]
    [TestCase(-5, -2, 50)]
    public void GetPointIncreasesRadiusByLambdaAndResetsAngle_AfterFullSpins(int centerX, int centerY, int radiusLambda)
    {
        var centerPoint = new Point(centerX, centerY);
        var spiral = new SpiralGenerator(centerPoint, radiusLambda, Math.PI);
        for (var i = 0; i < 4; i++)
        {
            spiral.GetNextPoint();
        }

        spiral.Radius.Should().Be(2 * radiusLambda);
        spiral.Angle.Should().Be(0);
    }

    [Test]
    public void GetPointCalculatesCorrectCoordinatesAndAngle()
    {
        var centerPoint = new Point(5, 6);
        var spiral = new SpiralGenerator(centerPoint, 4, Math.PI / 4);

        //Skips first circle
        for (var i = 0; i < 8; i++)
        {
            spiral.GetNextPoint();
        }

        (Point coordinates, double angle)[] expectedPoints =
        {
            (new Point(9, 6), 0),
            (new Point(8, 9), Math.PI/4),
            (new Point(5, 10), Math.PI/2),
            (new Point(2, 9), Math.PI*3/4),
            (new Point(1, 6), Math.PI),
            (new Point(2, 3), Math.PI*5/4),
        };

        foreach (var (coordinates, angle) in expectedPoints)
        {
            spiral.Angle.Should().Be(angle);
            var actualPoint = spiral.GetNextPoint();
            actualPoint.Should().Be(coordinates);
        }
    }
}