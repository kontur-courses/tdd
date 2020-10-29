using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class ArchimedeanSpiralTests
    {
        [TestCase(0, 0, 0, TestName = "Density is zero")]
        [TestCase(0, 0, -1.0, TestName = "Density is negative")]
        public void ThrowException_When(int x, int y, double density)
        {
            var point = new Point(x, y);
            Func<ArchimedeanSpiral> putRectangle = () => new ArchimedeanSpiral(point, density);

            putRectangle.Should().Throw<ArgumentException>();
        }
    }
}