using FluentAssertions;
using SixLabors.ImageSharp;
using TagCloud.CoordinatesProviders;

namespace TagCloudTests;

public class ArchimedesSpiralTests
{
    private ArchimedesSpiral archimedesSpiral;
    
    [Test]
    public void GetNextPoint_ShouldReturnPointInCenter_WhenCalledOnce()
    {
        var center = new PointF(0, 0);
        archimedesSpiral = new ArchimedesSpiral(center, 1, 1);
        
        var point = archimedesSpiral.GetPoints().Take(1).ToList();

        point.First().Should().Be(center);
    }

    [Test]
    public void GetNextPoint_TwoPointsShouldNotBeSame()
    {
        var center = new PointF(0, 0);
        archimedesSpiral = new ArchimedesSpiral(center, 1, 1);
        
        var point = archimedesSpiral.GetPoints().Take(2).ToList();
        
        point.Last().Should().NotBe(point.First());
    }
}