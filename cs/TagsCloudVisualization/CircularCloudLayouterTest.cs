using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTest
    {
        private CircularCloudLayouter cloud;

        [SetUp]
        public void SetUp() => cloud = new CircularCloudLayouter(new Point(0, 0));

        [TestCase(1, 1)]
        [TestCase(4, 4)]
        [TestCase(1, 3)]
        [TestCase(0, 0)]
        [TestCase(7, 4)]
        public void Test_PuFirstRectangleInCenterCloudZeroZero(int width, int height) 
        {
            var r = cloud.PutNextRectangle(new Size(width, height));
            Assert.IsTrue(Math.Abs(r.X) <= (width + 1) / 2 && Math.Abs(r.Y) <= (height + 1) / 2);
        }

        [TestCase(6, 5, 1, 1)]
        [TestCase(-6, 9, 4, 4)]
        [TestCase(9, - 1, 1, 3)]
        [TestCase(4, 0, 0, 0)]
        [TestCase(0, -10, 7, 4)]
        public void Test_PutFirstRectangleInCenterCloud(int x, int y, int width, int height) 
        {
            var r = new CircularCloudLayouter(new Point(x, y)).PutNextRectangle(new Size(width, height));
            Assert.IsTrue(Math.Abs(x - r.X) <= (width + 1) / 2 && Math.Abs(y - r.Y) <= (height + 1) / 2);
        }

        [Test]
        public void Test_PutSecondRectangleNextToFirst()
        {
            var random = new Random();
            var r1 = cloud.PutNextRectangle(new Size(random.Next(10), random.Next(10)));
            var r2 = cloud.PutNextRectangle(new Size(random.Next(10), random.Next(10)));
            Assert.IsTrue((r1.Top == r2.Bottom || r1.Bottom == r2.Top || 
                           r1.Right == r2.Left || r1.Left == r2.Right) &&
                           r2.Top >= r1.Top - r2.Height && r2.Bottom <= r1.Bottom + r2.Height &&
                           r2.Right <= r1.Right + r2.Width && r2.Left >= r1.Left - r2.Width);
        }

        [Test]
        public void Test_PutRectanglesWithoutIntersections()
        {
            var random = new Random();
            var rectangles = new HashSet<Rectangle>();
            for(var i = 0; i < 50; i++)
            {
                rectangles.Add(cloud.PutNextRectangle(new Size(random.Next(10), random.Next(10))));
            }
            var intersections = rectangles.SelectMany(r => rectangles.
                                                       Where(rec => rec != r).
                                                       Select(rec => Tuple.Create(r, rec)))
                                           .Select(t => t.Item1.IntersectsWith(t.Item2));
            Assert.IsFalse(intersections.Any(t => t));
        }

        [Test]
        public void Test_PutRectanglesInCircle()
        {
            var rectangles = new HashSet<Rectangle>();
            var square = 100;
            for(var i = 0; i < square; i++)
                rectangles.Add(cloud.PutNextRectangle(new Size(1, 1)));
            var radius = Math.Sqrt(square / Math.PI);
            var distance = GetMaxAverageDistanceToPoint(rectangles, cloud.center);
            Assert.IsTrue(distance <= radius);
        }

        private double GetDistancePoint(PointF p1, PointF p2) => 
            Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

        private PointF GetCenterRectangle(Rectangle rectangle) => 
            new PointF((rectangle.X + rectangle.Width) / 2, (rectangle.Y + rectangle.Height) / 2);

        private double GetAverageDistanceToPoint(Rectangle rectangle, Point point) =>
            GetDistancePoint(GetCenterRectangle(rectangle), point);
        private double GetMaxAverageDistanceToPoint(IEnumerable<Rectangle> rectangles, Point point) =>
            rectangles
            .Select(r => GetAverageDistanceToPoint(r, point))
            .Max();

        private Point[] GetTops(Rectangle rectangle) => new []{ 
            new Point(rectangle.Right, rectangle.Top), 
            new Point(rectangle.Right, rectangle.Bottom),
            new Point(rectangle.Left, rectangle.Top),
            new Point(rectangle.Left, rectangle.Bottom) };
    }
}
