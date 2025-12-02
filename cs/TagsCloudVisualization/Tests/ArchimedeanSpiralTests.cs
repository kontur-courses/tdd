using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class ArchimedeanSpiralTests
{
    private ISpiral spiral;
    
    [SetUp]
    public void SetUp()
    {
        spiral = new ArchimedeanSpiral(default, Math.PI * 2, 10);
    }
    
    [Test]
    [Description("Check that the point counting is correct")]
    public void GetNextPoint_ReturnsPoint_WhenGettingSeveralPoints()
    {
        spiral.GetNextPoint().Should().Be(new Point());
        spiral.GetNextPoint().Should().Be(new Point(63, 0));
        spiral.GetNextPoint().Should().Be(new Point(126, 0));
    }
}