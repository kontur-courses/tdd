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
        public void Should_SetCenter_InCounstryctor_ByConstructorParameter()
        {
            var center = new Point(-10, 10);
            var spiral = new ArchimedeanSpiral(center);
            var point = spiral.GetDiscretePoints().First();
            point.Should().Be(center);
        }

        [Test]
        public void Should_SetCenter_InEmptyConstructor_AsEmptyPoint()
        {
            var spiral = new ArchimedeanSpiral();
            var point = spiral.GetDiscretePoints().First();
            point.Should().Be(Point.Empty);
        }
    }
}
