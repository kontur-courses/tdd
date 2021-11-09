using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Test
{
    class ArchimedeanSpiral_Test
    {
        [Test]
        public void GetDiscretePoints_Should_Unwind()
        {
            var spiral = new ArchimedeanSpiral();
            var points = new List<Point>();
            using (new AssertionScope())
            {
                foreach (var p in spiral.GetDiscretePoints().Take(100))
                    points.Add(p);

                var lastFurtherThanFirst = Math.Abs(points.First().X) < Math.Abs(points.Last().X)
                    && Math.Abs(points.First().Y) < Math.Abs(points.Last().Y);

                var pairs = points.Zip(points.Skip(1), (f, s) => (f, s));
                pairs.All(pair => Math.Abs(pair.f.X) <= Math.Abs(pair.s.X)
                               && Math.Abs(pair.f.Y) <= Math.Abs(pair.s.Y)).Should().BeTrue();
            }
        }

        [Test]
        public void Should_SetCenter_InCounstryctor_ByConstructorParameter()
        {
            var center = new Point(-10, 10);
            var spiral = new ArchimedeanSpiral(center);
            var point = spiral.GetDiscretePoints().Take(1).First();
            point.Should().Be(center);
        }

        [Test]
        public void Should_SetCenter_InEmptyCounstryctor_AsEptyPoint()
        {
            var spiral = new ArchimedeanSpiral();
            var point = spiral.GetDiscretePoints().Take(1).First();
            point.Should().Be(Point.Empty);
        }
    }
}
