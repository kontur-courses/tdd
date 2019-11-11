using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class ArchimedeanSpiral_Should
    {
        [Test]
        public void ArchimedeanSpiral_GetCurrentX_ShouldReturnZeroAtStart()
        {
            new ArchimedeanSpiral(new Point(0, 0)).GetNewPointLazy().GetEnumerator().Current.X.Should().Be(0);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnZeroAtStart()
        {
            new ArchimedeanSpiral(new Point(0, 0)).GetNewPointLazy().GetEnumerator().Current.Y.Should().Be(0);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentX_ShouldReturnCorrectX()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            spiral.GetNewPointLazy().First().X.Should().Be(7);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnCorrectY()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            spiral.GetNewPointLazy().First().Y.Should().Be(3);
        }
    }
}