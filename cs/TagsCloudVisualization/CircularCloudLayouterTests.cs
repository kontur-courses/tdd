using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private Point center = new Point(500, 500);

        [SetUp]
        public void SetLayouter()
        {
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void PutNextRectangle_OnCenter_IfRectangleIsFirst()
        {
            var rectangle = PutRandomRectangles(1)[0];
            rectangle.Location.Should().BeEquivalentTo(new Point(
                center.X - rectangle.Width / 2, 
                center.Y - rectangle.Height / 2));
        }

        [Test]
        public void PutNextRectangle_NoIntersects_AfterPutting()
        {
            var rectangles = PutRandomRectangles(10);
            foreach (var rectangle in rectangles)
            {
                layouter.CurrentRectangles.Remove(rectangle);
                layouter.IntersectWithPreviousRectangles(rectangle).Should().BeFalse();
                layouter.CurrentRectangles.Add(rectangle);
            }
        }

        [Test]
        public void PutNextRectangle_AsCloseAsPossible_IfRectangleDoNotIntersectOther()
        {
            var rectangles = PutRandomRectangles(10);
            foreach (var rectangle in rectangles)
            {
                foreach (var direction in layouter.Directions)
                {
                    CheckDensity(rectangle, direction);
                }
            }
        }

        private void CheckDensity(Rectangle rectangle, Tuple<int, int> direction)
        {
            var tempRectangle = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
            tempRectangle.X = rectangle.X + direction.Item1 * layouter.Shift;
            tempRectangle.X = rectangle.Y + direction.Item2 * layouter.Shift;
            if (layouter.GetDistanceToCenter(tempRectangle) < layouter.GetDistanceToCenter(rectangle))
            {
                layouter.CurrentRectangles.Remove(rectangle);
                layouter.IntersectWithPreviousRectangles(tempRectangle).Should().BeTrue();
                layouter.CurrentRectangles.Add(rectangle);
            }
        }

        private List<Rectangle> PutRandomRectangles(int rectanglesCount)
        {
            var rectangles = new List<Rectangle>();
            var rnd = new Random();
            for (var i = 0; i < rectanglesCount; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(new Size(rnd.Next(50), rnd.Next(50))));
            }
            return rectangles;
        }
    }
}
