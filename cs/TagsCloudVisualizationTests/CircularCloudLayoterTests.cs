using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayoterTests
    {
        [Test]
        public void CloudLayouterConstructorShouldWorkCorrectly()
        {
            var center = new Point(15, 20);
            Action creating = () => new CircularCloudLayouter(center);
            creating.Should().NotThrow();
        }

        [Test]
        public void SingleRectangleInCenterPutCorrectly()
        {
            var rectangleSize = new Size(50, 60);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            rectangle.Location.Should().Be(layouterCenter);
        }

        [Test]
        public void RectanglesShouldBeInCircle()
        {
            var rectangleSize = new Size(60, 120);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 300; i++)
                layouter.PutNextRectangle(rectangleSize);
            var sumArea = GetSumAreaOfRectangles(layouter);
            var circleArea = GetCircleArea(GetCircleRadius(layouter));
            var density = sumArea / circleArea;
            density.Should().BeLessThan(1);
        }

        [Test]
        public void ShouldBeCloserToCircleThanToSquare()
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 100; i++)
                layouter.PutNextRectangle(rectangleSize);
            var sumArea = GetSumAreaOfRectangles(layouter);
            var circleArea = GetCircleArea(GetCircleRadius(layouter));
            var enclosingRectangleArea = GetEnclosingRectangleArea(layouter);
            var difCircleAndSum = sumArea/circleArea;
            var difSumAndEnclosingRectangle = sumArea/enclosingRectangleArea;
            difCircleAndSum.Should().BeLessThan(difSumAndEnclosingRectangle);
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
            for (int i = 0; i < 300; i++)
                layouter.PutNextRectangle(rectangleSize);
            foreach (var rectangle in layouter.GetRectangleList)
            {
                var act = layouter.GetRectangleList
                    .Where(r => r != rectangle)
                    .Any(r => r.IntersectsWith(rectangle));
                act.Should().BeFalse();
            }
        }

        [Test]
        public void ShouldNotIntersectWithRectangles()
        {
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            var random = new Random();
            for (int i = 0; i < 300; i++)
            {
                var rectangleSize = new Size(random.Next(1, 20), random.Next(1, 20));
                layouter.PutNextRectangle(rectangleSize);
            }
            foreach (var rectangle in layouter.GetRectangleList)
            {
                var act = layouter.GetRectangleList
                    .Where(r => r != rectangle)
                    .Any(r => r.IntersectsWith(rectangle));
                act.Should().BeFalse();
            }
        }
        public double GetCircleRadius(CircularCloudLayouter layouter)
        {
            var possibleRadii = new List<double>();
            foreach (var rectangle in layouter.GetRectangleList)
            {

                var rightUpNode = new Point(rectangle.X + rectangle.Width, rectangle.Y);
                var rightDownNode = new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                var leftDowNode = new Point(rectangle.X, rectangle.Y + rectangle.Height);
                var leftUpToCenter = Math.Sqrt(Math.Pow(rectangle.X - layouter.Center.X, 2) +
                                               Math.Pow(rectangle.Y - layouter.Center.Y, 2));
                var leftDownToCenter =
                    Math.Sqrt(Math.Pow(leftDowNode.X - layouter.Center.X, 2) + Math.Pow(leftDowNode.Y - layouter.Center.Y, 2));
                var rightUpToCenter = Math.Sqrt(Math.Pow(rightUpNode.X - layouter.Center.X, 2) + Math.Pow(rightUpNode.Y - layouter.Center.Y, 2));
                var rightDownToCenter =
                    Math.Sqrt(Math.Pow(rightDownNode.X - layouter.Center.X, 2) + Math.Pow(rightDownNode.Y - layouter.Center.Y, 2));
                possibleRadii.Add(new List<double> { rightDownToCenter, rightUpToCenter, leftDownToCenter, leftUpToCenter }.Max());
            }
            return possibleRadii.Max();
        }

        public double GetSumAreaOfRectangles(CircularCloudLayouter layouter)
        {
            double result = 0;
            foreach (var rectangle in layouter.GetRectangleList)
            {
                var rectangleArea = rectangle.Height * rectangle.Width;
                result += rectangleArea;
            }
            return result;
        }

        public double GetEnclosingRectangleArea(CircularCloudLayouter layouter)
        {
            var vertexes = new List<Point>();
            foreach (var rectangle in layouter.GetRectangleList)
            {
                vertexes.Add(new Point(rectangle.X, rectangle.Y + rectangle.Height));
                vertexes.Add(new Point(rectangle.X, rectangle.Y));
                vertexes.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y));
                vertexes.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
            }

            var xCords = from vertex in vertexes select vertex.X;
            var sortListXCords = xCords.ToList().OrderBy(x => x).ToList();
            var xMin = sortListXCords.First();
            var xMax = sortListXCords.Last();
            var yCords = from vertex in vertexes select vertex.Y;
            var sortListYCords = yCords.ToList().OrderBy(y => y).ToList();
            var yMin = sortListYCords.First();
            var yMax = sortListYCords.Last();
            var enclosingRectangle = new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
            return enclosingRectangle.Height * enclosingRectangle.Width;

        }

        public double GetCircleArea(double circleRadius)
        {
            if (circleRadius <= 0)
                throw new ArgumentException();
            var area = Math.PI * Math.Pow(circleRadius, 2);
            return area;
        }
    }
}
