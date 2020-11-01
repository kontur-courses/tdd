using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class ArchimedeanSpiralTests
    {
        private Point Center { get; set; }
        private ArchimedeanSpiral Spiral { get; set; }

        [SetUp]
        public void SetUp()
        {
            Center = new Point(500, 500);
            Spiral = new ArchimedeanSpiral(Center, 0.2, 0);
        }

        [Test]
        public void GetNextPoint_ReturnStartPoint_OnFirstRequest()
        {
            Spiral.GetNextPoint().Should().Be(Center);
        }

        [Test]
        public void GetNextPoint_ReturnDifferentPoints_OnMultipleRequest()
        {
            var points = new List<Point>();

            for (int i = 0; i < 100; i++)
            {
                points.Add(Spiral.GetNextPoint());
            }

            points.First().Should().NotBe(points.Last());
        }

        [Test]
        public void GetNextPoint_ReturnNotNull_WhenCalled()
        {
            Spiral.GetNextPoint().Should().NotBeNull();
        }
    }
}