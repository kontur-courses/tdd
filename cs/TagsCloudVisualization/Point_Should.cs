using System;
using System.Runtime.Remoting.Messaging;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Point_Should
    {
        private static Point _p1 = new Point(0, 0);
        private static Point _p2 = new Point(11, 0);
        private static Point _p3 = new Point(0, 10);
        private static Point _p4 = new Point(10, 11);
        private static Point _p5 = new Point(-10, -11);
        
        [Test]
        public void FirstDistance_ShouldBeGreater_ThenSecondDistance()
        {
            var d1 = Point.PowDistance(_p1, _p2);
            var d2 = Point.PowDistance(_p1, _p3);
            d1.Should().BeGreaterThan(d2);
        }
        
        [Test]
        public void FirstDistance_ShouldBeLess_ThenSecondDistance()
        {
            var d1 = Point.PowDistance(_p1, _p3);
            var d2 = Point.PowDistance(_p1, _p4);
            d1.Should().BeLessThan(d2);
        }
        
        [Test]
        public void FirstDistance_ShouldBeEquals_ThenSecondDistance()
        {
            var d1 = Point.PowDistance(_p1, _p5);
            var d2 = Point.PowDistance(_p1, _p4);
            d1.Should().IsSameOrEqualTo(d2);
        }

        [Test]
        public void Equals_ShouldBeTrue_WhenPointsAreEquals()
        {
            var p1 = new Point(142, 13);
            var p2 = new Point(142, 13);
            p1.Equals(p2).Should().BeTrue();
        }
        
        [Test]
        public void Equals_ShouldBeFalse_WhenPointsAreNotEquals()
        {
            var p1 = new Point(142, 13);
            var p2 = new Point(0, 13);
            p1.Equals(p2).Should().BeFalse();
        }
    }
}