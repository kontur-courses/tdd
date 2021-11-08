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
        public void PutNextRectangle_RectanglesDoNotIntersect_WhenPutTwoRectangles()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var sizeFirstRectangle = new Size(2, 2);
            var sizeSecondRectangle = new Size(3, 1);

            var firstRectangle = cloud.PutNextRectangle(sizeFirstRectangle);
            var secondRectangle = cloud.PutNextRectangle(sizeSecondRectangle);
            var intersect = Rectangle.Intersect(firstRectangle, secondRectangle);

            intersect.Should().Be(new Rectangle(new Point(), new Size()));
        }

        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect_WhenPutManyRectangles()
        {
            var center = new Point(3, 8);
            var cloud = new CircularCloudLayouter(center);
            var rectangleSize = new Size(3, 2);
            var emptyRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 20; i++)
                cloud.PutNextRectangle(rectangleSize);

            foreach(var r1 in rectangles)
                foreach(var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;
                    Rectangle.Intersect(r1, r2).Should().Be(emptyRectangle);
                }
        }

        Point MoveRectangleToPoint(Rectangle rectangle, Point goal)
        {
            var vector = new Point(goal.X - rectangle.X, goal.Y - rectangle.Y);
            var newX = goal.X;
            var newY = goal.Y;
            if (vector.X != 0)
                newX = rectangle.X + vector.X / Math.Abs(vector.X);
            if (vector.Y != 0)
                newY = rectangle.Y + vector.Y / Math.Abs(vector.Y);
            return new Point(newX, newY);
        }

        [Test]
        public void PutNextRectangle_RectangleIsCloseToCenter_WhenPutTwoRectangles()
        {
            var center = new Point(-5, -5);
            var cloud = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(2, 2);

            var firstRectangle = cloud.PutNextRectangle(sizeRectangle);
            var secondRectangle = cloud.PutNextRectangle(sizeRectangle);
            var newLocation = MoveRectangleToPoint(secondRectangle, center);
            secondRectangle.Location = newLocation;

            Rectangle.Intersect(firstRectangle,secondRectangle).Should().NotBe(new Rectangle(new Point(), new Size()));
        }

        [Test]
        public void PutNextRectangle_RectanglesAreCloseToCenter_WhenPutManyRectangles()
        {
            var center = new Point(1, 3);
            var cloud = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(2, 2);
            var emptyRectangle = new Rectangle(new Point(), new Size());
            var rectangles = new List<Rectangle>();

            for(var i = 0; i < 10; i++)
                rectangles.Add(cloud.PutNextRectangle(sizeRectangle));

            foreach (var r1 in rectangles) 
            {
                if (r1.Location == center)
                    continue;
                var newR1 = new Rectangle(MoveRectangleToPoint(r1, center), r1.Size);
                var hasIntersect = false;
                foreach (var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;

                    var intersect = Rectangle.Intersect(newR1, r2);
                    if(!intersect.Equals(emptyRectangle))
                    {
                        hasIntersect = true;
                        break;
                    }    
                }
                hasIntersect.Should().BeTrue();
            }
        }
    }
}