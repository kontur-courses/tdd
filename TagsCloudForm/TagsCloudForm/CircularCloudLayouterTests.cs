using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {


        private double GetDistance(Rectangle rectangle, Point center)
        {
            var rectangleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            return Distance(rectangleCenter, center);
        }

        private double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        [Test]
        public void PutNextRectangle_PuttingOnce_RectangleCenterShouldBeInLayouterCenter()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rnd = new Random();
            var size = new Size(rnd.Next(5, 100), rnd.Next(5, 100));
            var expectedLocation = new Point(-(int)Math.Floor(size.Width / (double)2), -(int)Math.Floor(size.Height / (double)2));
            var rect = layouter.PutNextRectangle(size);
            rect.Location.Should().Be(expectedLocation);
        }

        [Test]
        public void PutNextRectangle_PuttingTwoRectangles_RectanglesShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rnd = new Random();
            var size1 = new Size(rnd.Next(5, 100), rnd.Next(5, 100));
            var size2 = new Size(rnd.Next(5, 100), rnd.Next(5, 100));
            var rect1 = layouter.PutNextRectangle(size1);
            var rect2 = layouter.PutNextRectangle(size2);
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_AddingThreeRectangles_ShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(new Size(10, 10));
            var rect2 = layouter.PutNextRectangle(new Size(10, 10));
            var rect3 = layouter.PutNextRectangle(new Size(10, 10));
            rect3.IntersectsWith(rect2).Should().BeFalse();
            rect3.IntersectsWith(rect1).Should().BeFalse();
        }


        [Test]
        public void PutNextRectangle_AddingSquareAndBiggerSquare_SquaresShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var squareSize = new Size(10, 10);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(squareSize);
            var rect2 = layouter.PutNextRectangle(new Size(20, 20));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_FiftyRectanglesInSafeMode_ShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var rnd = new Random();
            for (int i=0; i<50; i++)
            {
                var size = new Size(rnd.Next(5, 100), rnd.Next(5, 100));
                var rect = layouter.PutNextRectangle(size);
                rectangles.ForEach(a => rect.IntersectsWith(a).Should().BeFalse());
                rectangles.Add(rect);
            }
        }


        [Test]
        public void PutNextRectangle_TestFromRandomTester_RectanglesShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rects = new List<Rectangle>
            {
                layouter.PutNextRectangle(new Size(86, 79)),
                layouter.PutNextRectangle(new Size(21, 92)),
            };
            var testedRect = layouter.PutNextRectangle(new Size(97, 98));
            rects.ForEach(a => testedRect.IntersectsWith(a).Should().BeFalse());
        }

        [Test]
        public void PutNextRectangle_TestFromRandomTester2_LastRectangleShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rects = new List<Rectangle>{
            layouter.PutNextRectangle(new Size(57, 40)),
            layouter.PutNextRectangle(new Size(41, 24)),
            layouter.PutNextRectangle(new Size(34, 54)),
            layouter.PutNextRectangle(new Size(57, 48))
            };
            var testedRect = layouter.PutNextRectangle(new Size(90, 52));
            rects.ForEach(a => testedRect.IntersectsWith(a).Should().BeFalse());
        }

        [Test]
        public void PutNextRectangle_TestFromRandomTester3_LastRectangleShouldNotIntersectWithOthers()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rects = new List<Rectangle>();
            rects.Add(layouter.PutNextRectangle(new Size(73, 63)));
            var testedRect = layouter.PutNextRectangle(new Size(84, 23));
            rects.ForEach(a => testedRect.IntersectsWith(a).Should().BeFalse());
        }

        [Test]
        public void PutNextRectangle_TestFromRandomTester4_LastRectangleShouldNotIntersectWithOthers()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rects = new List<Rectangle> {
            layouter.PutNextRectangle(new Size(89, 26)),
            layouter.PutNextRectangle(new Size(30, 44)),
            layouter.PutNextRectangle(new Size(97, 84)),
            layouter.PutNextRectangle(new Size(28, 92)),
            };
            var testedRect = layouter.PutNextRectangle(new Size(60, 42));
            rects.ForEach(a => testedRect.IntersectsWith(a).Should().BeFalse());
        }

        [Test]
        public void PutNextRectangle_TestFromRandomTester5_LastRectangleShouldNotIntersectWithOthers()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rects = new List<Rectangle> {
            layouter.PutNextRectangle(new Size(42, 71)),
            layouter.PutNextRectangle(new Size(77, 53)),
            layouter.PutNextRectangle(new Size(24, 52)),
            layouter.PutNextRectangle(new Size(74, 36)),
            };
            var testedRect = layouter.PutNextRectangle(new Size(32, 46));
            rects.ForEach(a => testedRect.IntersectsWith(a).Should().BeFalse());
        }

        [Test]
        public void PutNextRectangle_TestFromRandomTester6_LastRectangleShouldNotIntersectWithOthers()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rects = new List<Rectangle> {
            layouter.PutNextRectangle(new Size(37, 75)),
            layouter.PutNextRectangle(new Size(80, 81)),
            layouter.PutNextRectangle(new Size(96, 26)),
            layouter.PutNextRectangle(new Size(51, 28)),
            };
            var testedRect = layouter.PutNextRectangle(new Size(89, 79));
            rects.ForEach(a => testedRect.IntersectsWith(a).Should().BeFalse());
        }

        [Test]
        public void PutNextRectangle_TestFromRandomTester7_LastRectangleShouldNotIntersectWithOthers()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rects = new List<Rectangle> {
            layouter.PutNextRectangle(new Size(10,10)),
            layouter.PutNextRectangle(new Size(40, 10)),
            layouter.PutNextRectangle(new Size(40, 10)),
            layouter.PutNextRectangle(new Size(20, 100))
        };
            var testedRect = layouter.PutNextRectangle(new Size(15, 15));
        rects.ForEach(a => testedRect.IntersectsWith(a).Should().BeFalse());
        }


        [Test]
        public void CircularCloudLayouter_AddingFiftyRandomRectangles_CheckDensity()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var rnd = new Random();
            for (int i = 0; i < 50; i++)
            {
                var size = new Size(rnd.Next(5, 100), rnd.Next(5, 100));
                var rect = layouter.PutNextRectangle(size);
                rectangles.Add(rect);
            }
            var cloudRadius = rectangles.Select(a => GetDistance(a, center)).OrderByDescending(a => a).First();
            var rectanglesArea = rectangles.Select(a => a.Width * a.Height).Sum();
            var cloudArea = Math.PI * cloudRadius * cloudRadius;
            (cloudArea/rectanglesArea).Should().BeLessThan(Math.PI/2);//площадь вписанного квадрата/площадь описанной окружности = Pi/2

        }

        [Test]
        public void PutNextRectangle_HundredRectangles_ShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                var size = new Size(rnd.Next(5, 100), rnd.Next(5, 100));
                var rect = layouter.PutNextRectangle(size);
                rectangles.ForEach(a => rect.IntersectsWith(a).Should().BeFalse());
                rectangles.Add(rect);
            }
        }

    }
}
