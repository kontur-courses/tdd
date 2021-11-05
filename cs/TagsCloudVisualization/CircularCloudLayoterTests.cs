using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            rectangle.Location.Should().Be(layouterCenter);
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void ShouldPutTheExactNumberOfRectangles(int numberOfRactangles)
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < numberOfRactangles; i ++)
                layouter.PutNextRectangle(rectangleSize);
            layouter.RectangleList.Count.Should().Be(numberOfRactangles);
        }

        public List<Point> CheckForDuplicates(List<Point> array)
        {
            var duplicates = array
                .GroupBy(p => p)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key).ToList();
            return duplicates;
        }

        [Test]
        public void ShouldNotPutOneRectangleTwice()
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 30; i++)
                layouter.PutNextRectangle(rectangleSize);
            var duplicates = layouter.RectangleList
                .GroupBy(p => p)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key).ToList();
            duplicates.Count.Should().Be(0);
        }


        [TestCase(-5,10)]
        [TestCase(5,-5)]
        [TestCase(0,0)]
        public void ShouldNotPutInvalidRectangle(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            Action put = () => layouter.PutNextRectangle(rectangleSize);
            put.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void SameRectanglesShouldNotIntersect()
        {
            var rectangleSize = new Size(2, 2);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 100; i++)
                layouter.PutNextRectangle(rectangleSize);
            foreach (var rectangle in layouter.RectangleList)
            {
                var act = layouter.RectangleList
                    .Where(r => r != rectangle)
                    .Any(r => r.IntersectsWith(rectangle));
                act.Should().BeFalse();
            }
        }

        [Test]
        public void RandomRectanglesShouldNotIntersect()
        {
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 100; i++)
            {
                var rectangleSize = new Size(new Random().Next(1, 20), new Random().Next(1, 20));
                layouter.PutNextRectangle(rectangleSize);
            }
            foreach (var rectangle in layouter.RectangleList)
            {
                var act = layouter.RectangleList
                    .Where(r => r != rectangle)
                    .Any(r => r.IntersectsWith(rectangle));
                act.Should().BeFalse();
            }
        }
    }
}
