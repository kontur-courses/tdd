using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;

namespace TagsCloudVisualizer
{
    [TestFixture]
    class CircularCloudLayouter_Tests
    {
        [Test]
        public void PutNewRectangle_WithTwoRandomRectanglesRunning1000Times_MustNotIntersect()
        {
            for (int i = 0; i < 1000; i++)
            {
                var random = new Random();
                var size1 = new Size(random.Next(1, 10), random.Next(1, 10));
                var size2 = new Size(random.Next(1, 10), random.Next(1, 10));
                var CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(random.Next(-4, 4), random.Next(-4, 4))));
                var rect1 = CCL.PutNewRectangle(size1);
                var rect2 = CCL.PutNewRectangle(size2);
                rect1.IntersectsWith(rect2).Should().BeFalse();
            }
        }
        public double GetDiagonalLength(Rectangle rect1)
        {
            var leftBottomCorner1 = new Point(rect1.Left, rect1.Bottom);
            var rightUpperCorner1 = new Point(rect1.Right, rect1.Top);
            return leftBottomCorner1.GetDistanceTo(rightUpperCorner1);
        }
        public double GetMaximalTightDistanceBetweenRectangles(Rectangle rect1, Rectangle rect2)
        {
            var rect1diagonal = GetDiagonalLength(rect1);
            var rect2diagonal = GetDiagonalLength(rect2);
            return rect1diagonal + rect2diagonal;
        }
        [Test]
        public void PutNewRectangle_WithTwoRandomRectanglesRunning10000Times_MustLocatedTightly()
        {
            for (int i = 0; i < 10000; i++)
            {
                var random = new Random();
                var size1 = new Size(random.Next(1, 30), random.Next(1, 30));
                var size2 = new Size(random.Next(1, 30), random.Next(1, 30));
                var CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(random.Next(-4, 4), random.Next(-4, 4))));
                var rect1 = CCL.PutNewRectangle(size1);
                var rect2 = CCL.PutNewRectangle(size2);
                var distanceBetweenCenters = rect1.Location.GetDistanceTo(rect2.Location);
                var maxDistance = GetMaximalTightDistanceBetweenRectangles(rect1, rect2);
                distanceBetweenCenters.Should().BeLessThanOrEqualTo(maxDistance);
            }
        }
        [Test]
        public void PutNewRectangle_WithFourRandomRectanglesRunning10000Times_MustLocatedTightly()
        {
            for (int i = 0; i < 10000; i++)
            {
                var random = new Random();
                var CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(0, 0)));
                var size1 = new Size(random.Next(2, 15), random.Next(2, 15));
                var rect1 = CCL.PutNewRectangle(size1);
                for (int j = 0; j < 3; j++)
                {
                    var size = new Size(random.Next(2, 15), random.Next(2, 15));
                    var rectJ = CCL.PutNewRectangle(size);
                    var distanceBetweenCenters = rect1.Location.GetDistanceTo(rectJ.Location);
                    var maxDistance = GetMaximalTightDistanceBetweenRectangles(rect1, rectJ);
                    distanceBetweenCenters.Should().BeLessThanOrEqualTo(maxDistance);
                }
            }
        }

        [Test]
        public void PutNewRectangle_WithNonPositiveSize_ShouldThrow()
        {
            var random = new Random();
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            var CCL = new CircularCloudLayouter(spiral);
            for (int i = 0; i < 1000; i++)
            {
                var size = new Size(random.Next(-4, 1), random.Next(-4, 1));
                Action t = () => CCL.PutNewRectangle(size);
                t.Should().Throw<ArgumentException>();
            }
        }
    }
}
