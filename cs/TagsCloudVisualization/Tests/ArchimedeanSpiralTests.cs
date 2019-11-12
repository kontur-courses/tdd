using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class ArchimedeanSpiralTests
    {
        private ArchimedeanSpiral archimedeanSpiral;

        [SetUp]
        public void SetUp()
        {
            archimedeanSpiral = new ArchimedeanSpiral(1);
        }

        [Test]
        public void GetNextPoint_ShouldBeCloseToOriginOnFirstCall()
        {
            Point.Round(archimedeanSpiral.GetNextPoint()).Should().Be(new Point(0, 0));
        }

        [TestCase(5, TestName = "WhenGet5Points")]
        [TestCase(25, TestName = "WhenGet25Points")]
        [TestCase(50, TestName = "WhenGet50Points")]
        public void GetNextPoint_ShouldReturnDifferentPoints(int count)
        {
            var points = new List<PointF>();
            for (var i = 0; i < count; i++)
                points.Add(archimedeanSpiral.GetNextPoint());
            IsDifferentPoints(points).Should().BeTrue();
        }

        private bool IsDifferentPoints(List<PointF> points)
        {
            for (var i = 0; i < points.Count; i++)
            {
                for (var j = i + 1; j < points.Count; j++)
                {
                    if (Math.Abs(points[i].X - points[j].X) < double.Epsilon
                        && Math.Abs(points[i].Y - points[j].Y) < double.Epsilon)
                        return false;
                }
            }

            return true;
        }
    }
}