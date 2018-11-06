using System;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest
{
    class ArchimedeanSpiral_Should
    {
        [Test]
        public void ReturnCenter_ForTheFirstCall()
        {
            var spiral = new ArchimedeanSpiralGenerator(new PointF(0, 0), 10, 1);
            var firstPoint = spiral.GetNextPoint();
            AssertPointsAlmostEqual(new PointF(0, 0), firstPoint);
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
