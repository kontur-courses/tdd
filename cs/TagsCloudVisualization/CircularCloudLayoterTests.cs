using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayoterTests
    {
        [Test]
        public void ShouldCreateCircularCloud()
        {
            var center = new Point(15, 20);
            Action creating = () => new CircularCloudLayouter(center);
            creating.Should().NotThrow();
        }

        [Test]
        public void ShouldCreateCenterCorrectly()
        {
            var center = new Point(10, 30);
            var layouter =  new CircularCloudLayouter(center);
            layouter.Center.Should().Be(new Point(10,30));
        }

        [Test]
        public void ShouldPutOneRectangleInRightLocation()
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            rectangle.Location.Should().Be(new Point(5, 5));
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void ShouldPutManyRectangles(int numberOfRactangles)
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < numberOfRactangles; i ++)
                layouter.PutNextRectangle(rectangleSize);
            layouter.RectangleList.Count.Should().Be(numberOfRactangles);
        }

        [TestCase(-5,10)]
        [TestCase(5,-5)]
        public void ShouldNotPutInvalidRectangle(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            Action put = () => layouter.PutNextRectangle(rectangleSize);
            put.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void RectanglesShouldNotIntersect()
        {
            var rectangleSize = new Size(2, 2);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 10; i++)
            {
                layouter.PutNextRectangle(rectangleSize);
            }
            for (int i = 0; i < layouter.RectangleList.Count; i++)
            {
                var currentRectangle = layouter.RectangleList[i];
                var act = layouter.RectangleList
                    .Where(rectangle => rectangle != currentRectangle)
                    .Any(rectangle => rectangle.IntersectsWith(currentRectangle));
                act.Should().BeFalse();
            }
        }
    }
}
