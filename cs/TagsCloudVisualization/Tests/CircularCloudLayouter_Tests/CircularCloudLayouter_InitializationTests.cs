using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests.CircularCloudLayouter_Tests
{
    internal class CircularCloudLayouter_InitializationTests
    {
        [TestCase(1000, 500)]
        [TestCase(10000, 2000)]
        [TestCase(-11000, 20)]
        [TestCase(6, -12)]
        [TestCase(-5, -500)]
        [TestCase(0, 0)]
        public void ShouldNotThrow_WithAnyArguments(int x, int y)
        {
            var point = new Point(x, y);
            Action act = () => new CircularCloudLayouter(point);
            act.Should().NotThrow();
        }
    }
}
