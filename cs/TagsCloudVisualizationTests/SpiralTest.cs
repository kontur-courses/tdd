using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.TagsCloudVisualizationTests;

public class SpiralTest
{
    [Test]
    public void GetNextPointOnSpiral_ShouldReturnPointsOnSpiral()
    {
        Spiral spiral = new Spiral(new Point(CloudLayouterConst.CloudCentreX, CloudLayouterConst.CloudCentreY));
        List<Point> expected =
        [
            new Point(500, 500), new Point(500, 500), new Point(499, 501), new Point(497, 500), new Point(497, 496),
            new Point(501, 495), new Point(505, 498)
        ];
        List<Point> points = [];
        for (int i = 0; i < 700; i++)
        {
            var point = spiral.GetNextPointOnSpiral();
            if (i % 100 == 0)
                points.Add(point);
        }

        points.Should().BeEquivalentTo(expected);
    }
}