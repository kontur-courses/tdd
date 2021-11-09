using NUnit.Framework;
using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using System;
using System.Collections.Generic;

namespace TagsCloudTest
{
    public class CircularCloudTest
    {
        [Test]
        public void PutNextRectangle_RectangleWithCenterInCloudCenter_WhenPutFirstRectangle()
        {
            var center = new Point(5, 5);
            var cloud = new CircularCloudLayouter(center);

            var rectangle = cloud.PutNextRectangle(new Size(3,1));

            rectangle.Location.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_ArgumentException_WhenPutRectangleWithNegativeSize()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            Action action = () => cloud.PutNextRectangle(new Size(-2, 0));

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(1000)]
        [TestCase(100)]
        [TestCase(10)]
        [TestCase(2)]
        public void PutNextRectangle_RectanglesDoNotIntersect_WhenPutRectanglesOfTheSameSize(int rectanglesCount)
        {
            var center = new Point(3, 8);
            var cloud = new CircularCloudLayouter(center);
            var rectanglesSize = new Size(3, 2);
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < rectanglesCount; i++)
                cloud.PutNextRectangle(rectanglesSize);

            foreach(var r1 in rectangles)
                foreach(var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;
                    r1.IntersectsWith(r2).Should().BeFalse();
                }
        }

        [TestCase(1000)]
        [TestCase(100)]
        [TestCase(20)]
        [TestCase(2)]
        public void PutNextRectangle_RectanglesDoNotIntersect_WhenPutRectanglesOfRandomSize(int rectanglesCount)
        {
            var rnd = new Random();
            var center = new Point(3, 8);
            var cloud = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < rectanglesCount; i++)
            {
                var width = rnd.Next(1, 1000);
                var height = rnd.Next(1, 1000);
                cloud.PutNextRectangle(new Size(width,height));
            }

            foreach (var r1 in rectangles)
                foreach (var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;
                    r1.IntersectsWith(r2).Should().BeFalse();
                }
        }

        [TestCase(2)]
        [TestCase(200)]
        [TestCase(2000)]
        public void PutNextRectangle_RectanglesAreCloseToCenter_WhenPutRectanglesOfTheSameSize(int rectanglesCount)
        {
            var center = new Point(1, 3);
            var cloud = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(2, 2);
            var rectangles = new List<Rectangle>();

            for(var i = 0; i < rectanglesCount; i++)
                rectangles.Add(cloud.PutNextRectangle(sizeRectangle));

            foreach (var r1 in rectangles) 
            {
                if (r1.Location == center)
                    continue;
                var newR1 = new Rectangle(ShiftRectangleToGoalByDelta(r1, center), r1.Size);
                var hasIntersect = false;
                foreach (var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;

                    if(newR1.IntersectsWith(r2))
                    {
                        hasIntersect = true;
                        break;
                    }    
                }
                hasIntersect.Should().BeTrue();
            }
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        public void PutNextRectangle_RectanglesAreCloseToCenter_WhenPutRectanglesOfRandomSize(int rectanglesCount)
        {
            var rnd = new Random();
            var center = new Point(1, 3);
            var cloud = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < rectanglesCount; i++)
            {
                var width = rnd.Next(1, 1000);
                var height = rnd.Next(1, 1000);
                rectangles.Add(cloud.PutNextRectangle(new Size(width, height)));
            }

            foreach (var r1 in rectangles)
            {
                if (r1.Location == center)
                    continue;
                var newR1 = new Rectangle(ShiftRectangleToGoalByDelta(r1, center), r1.Size);
                var hasIntersect = false;
                foreach (var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;

                    if (newR1.IntersectsWith(r2))
                    {
                        hasIntersect = true;
                        break;
                    }
                }
                hasIntersect.Should().BeTrue();
            }
        }

        private Point ShiftRectangleToGoalByDelta(Rectangle rectangle, Point goal)
        {
            var vector = new Point(goal.X - rectangle.Location.X, goal.Y - rectangle.Location.Y);
            var newX = rectangle.Location.X + Math.Sign(vector.X);
            var newY = rectangle.Location.Y + Math.Sign(vector.Y);
            return new Point(newX, newY);
        }
    }
}