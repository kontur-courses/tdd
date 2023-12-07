using System.Drawing;
using System.Runtime.ExceptionServices;
using TagCloud;

namespace TagCloudTests;

public class SpiralCloudShaper_Should
{
    [TestCase(-1, 1, TestName = "NegativeCoefficient")]
    [TestCase(1, -1, TestName = "NegativeDeltaAngle")]
    [TestCase(-1, -1, TestName = "NegativeDeltaAngleAndCoefficient")]
    public void Throw_OnCreationWith(double coefficient, double deltaAngle)
    {
        var center = new Point(0, 0);
        Assert.Throws<ArgumentException>(() => new SpiralCloudShaper(center, coefficient, deltaAngle));
    }
}