using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class ArchimedeanSpiralTests
    {
        [TestCase(1, 1, TestName = "Positive coordinates")]
        [TestCase(0, 0, TestName = "Zero coordinates")]
        [TestCase(-1, -1, TestName = "Negative coordinates")]
        [TestCase(1, -1, TestName = "Mixed coordinates")]
        public void ArchimedeanSpiral_DoesNotThrowException_On(int x, int y)
        {
            var center = new Point(x, y);
            Action action = () => new ArchimedeanSpiral(center);
            action.Should().NotThrow();
        }
    }
}