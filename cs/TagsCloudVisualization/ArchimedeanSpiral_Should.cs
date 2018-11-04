using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class ArchimedeanSpiral_Should
    {
        [Test]
        public void ReturnCenter_ForTheFirstCall()
        {
            var spiral = ArchimedeanSpiralGenerator.GetArchimedeanSpiralGenerator(new PointF(0, 0), 10, 1);
            var firstPoint = spiral.First();
            AssertPointsAlmostEqual(new PointF(0, 0), firstPoint);
        }

        [Test]
        public void ReturnPointsWithDistanceEqualToStep_AfterFullTurn()
        {
            var step = 10f;
            var spiral = ArchimedeanSpiralGenerator.GetArchimedeanSpiralGenerator(new PointF(0, 0), step, 2 * (float) Math.PI);
            var points = spiral.Take(2).ToArray();
            var distance = Math.Sqrt(
                (points[0].X - points[1].X) * (points[0].X - points[1].X) +
                (points[0].Y - points[1].Y) * (points[0].Y - points[1].Y));
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
