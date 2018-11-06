using System;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest
{
    class ArchimedeanSpiral_Should
    {
        [Test]
        public void ReturnCenter_ForTheFirstCall_WhenCenterIsZeroPoint()
        {
            ReturnCenter_ForTheFirstCall(new PointF(0, 0));
        }

        [Test]
        public void ReturnCenter_ForTheFirstCall_WhenCenterNonZeroPoint()
        {
            ReturnCenter_ForTheFirstCall(new PointF(10, 20));
        }

        private void ReturnCenter_ForTheFirstCall(PointF center)
        {
            var spiral = new ArchimedeanSpiralGenerator(center, 10, 1);
            var firstPoint = spiral.GetNextPoint();
            AssertPointsAlmostEqual(center, firstPoint);
        }

        [Test]
        public void ReturnPointsWithDistanceEqualToStep_AfterFullTurn()
        {
            var step = 10f;
            var spiral = new ArchimedeanSpiralGenerator(new PointF(0, 0), step, 2 * (float) Math.PI);
            var first = spiral.GetNextPoint();
            var second = spiral.GetNextPoint();
            var distance = Math.Sqrt(
                (first.X - second.X) * (first.X - second.X) +
                (first.Y - second.Y) * (first.Y - second.Y));
            Assert.AreEqual(step, distance, 0.01);
        }

        private void AssertPointsAlmostEqual(PointF expected, PointF actual)
        {
            const double eps = 0.01; 
            Assert.AreEqual(expected.X, actual.X, eps);
            Assert.AreEqual(expected.Y, actual.Y, eps);
        }
    }
}
