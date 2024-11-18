using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class TestsSpiral
{

    ArchimedeanSpiral currentSpiral;
    [SetUp]
    public void SetUp()
    {
        currentSpiral = new ArchimedeanSpiral(new Point(0, 0));
    }

    [Test]
    public void Spiral_ThrowingWhenRadiusNonPositive()
    {
        Action action = new Action(() => new ArchimedeanSpiral(new Point(0, 0), -9));
        action.Should().Throw<ArgumentOutOfRangeException>().Which.Message.Should().Contain("radius step must be positive");
    }

    [Test]
    public void GetNextPoint_CenterPointSetting()
    {
        currentSpiral.GetNextPoint().Should().Be(new Point(0, 0));
    }

    [Test]
    public void GetNextPoint_SeveralPointSetting()
    {
        for (int i = 0; i < 180; i++)
        {
            currentSpiral.GetNextPoint();
        }
        currentSpiral.GetNextPoint().Should().Be(new Point((int)-Math.PI, 0));
    }

}