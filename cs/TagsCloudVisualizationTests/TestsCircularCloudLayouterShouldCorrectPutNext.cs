using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;


namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class TestsCircularCloudLayouterShouldCorrectPutNext
    {
        private CircularCloudLayouter Layouter
        { get; set; }
        private readonly Random _random = new Random();
        private List<Rectangle> RectanglesList
        { get; set; }

        [TearDown]
        public void VisualizeError()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                using (var visualization = new Visualization(RectanglesList, new Pen(Color.White, 3)))
                {
                    var testName = TestContext.CurrentContext.Test.Name;
                    var path = AppDomain.CurrentDomain.BaseDirectory + "\\" + testName + "." + ImageFormat.Jpeg;
                    Console.WriteLine($"Tag cloud visualization saved to file {path}");
                    visualization.DrawAndSaveImage(new Size(5000, 5000), path, ImageFormat.Jpeg);
                }
            }
        }

        [TestCase(-5, 10)]
        [TestCase(5, -5)]
        [TestCase(0, 0)]
        [TestCase(10, 10)]
        public void ShouldNotThrowIfPutRectangleWithCorrectSize(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            var layouterCenter = new Point(_random.Next(), _random.Next());
            Layouter = new CircularCloudLayouter(layouterCenter);
            Action put = () => Layouter.PutNextRectangle(rectangleSize);
            put.Should().NotThrow();
        }

        [TestCase(20, 10)]
        [TestCase(20, -10)]
        [TestCase(-15, -5)]
        [TestCase(-15, 5)]
        public void ShouldNotIntersectWithRectangles(int width, int height) // список
        {
            var layouterCenter = new Point(width, height);
            Layouter = new CircularCloudLayouter(layouterCenter);
            RectanglesList = new List<Rectangle>();

            var random = new Random();
            for (int i = 0; i < 300; i++)
            {
                var rectangleSize = new Size(random.Next(-50, 50), random.Next(-50, 50));
                RectanglesList.Add(Layouter.PutNextRectangle(rectangleSize));
            }
            foreach (var rectangle in RectanglesList)
            {
                var act = RectanglesList
                    .Where(r => r != rectangle)
                    .Any(r => r.IntersectsWith(rectangle));
                act.Should().BeFalse();
            }
        }

        [Test]
        public void SingleRectangleInCenterPutCorrectly()
        {
            var rectangleSize = new Size(_random.Next(-100, 100), _random.Next(-100, 100));
            var layouterCenter = new Point(20, 10);
            Layouter = new CircularCloudLayouter(layouterCenter);
            var rectangle = Layouter.PutNextRectangle(rectangleSize);
            rectangle.Location.Should().Be(layouterCenter);
        }
        /*
        [Test]
        public void RectanglesShouldBeInCircle()
        {
            var rectangleSize = new Size(60, 120);
            var layouterCenter = new Point(2000, 1000);
            RectanglesList = new List<Rectangle>();
            Layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 300; i++)
                RectanglesList.Add(Layouter.PutNextRectangle(rectangleSize));
            var sumArea = GetSumAreaOfRectangles(RectanglesList);
            var circleArea = GetCircleArea(GetCircleRadius(layouterCenter, RectanglesList));
            var density = sumArea / circleArea;
            density.Should().BeLessThan(1);
        }
        */

        [Test]
        public void ShouldBeCloserToCircleThanToSquare()
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            RectanglesList = new List<Rectangle>();
            Layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 100; i++)
                RectanglesList.Add(Layouter.PutNextRectangle(rectangleSize));
            var sumArea = GetSumAreaOfRectangles(RectanglesList);
            var circleArea = GetCircleArea(GetCircleRadius(layouterCenter, RectanglesList));
            var enclosingRectangleArea = GetEnclosingRectangleArea(RectanglesList);
            var difCircleAndSum = sumArea/circleArea;
            var difSumAndEnclosingRectangle = sumArea/enclosingRectangleArea;
            difCircleAndSum.Should().BeLessThan(difSumAndEnclosingRectangle);
        }


        [Test]
        public void SameRectanglesShouldNotIntersect()
        {
            var rectangleSize = new Size(2, 2);
            var layouterCenter = new Point(20, 10);
            RectanglesList = new List<Rectangle>();
            Layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 300; i++)
                RectanglesList.Add(Layouter.PutNextRectangle(rectangleSize));
            foreach (var rectangle in RectanglesList)
            {
                var act = RectanglesList
                    .Where(r => r != rectangle)
                    .Any(r => r.IntersectsWith(rectangle));
                act.Should().BeFalse();
            }
        }

        public double GetCircleRadius(Point layouterCenter, List<Rectangle> rectangles)
        {
            double maxRadius = 0;
            foreach (var rectangle in rectangles)
            {
                foreach (var node in rectangle.GetRectangleNodes())
                {
                    maxRadius = Math.Max(maxRadius, node.GetDistanceToPoint(layouterCenter));
                }
            }
            return maxRadius;
        }

        public double GetSumAreaOfRectangles(List<Rectangle> rectangles)
        {
            double result = 0;
            foreach (var rectangle in rectangles)
            {
                var rectangleArea = rectangle.Height * rectangle.Width;
                result += rectangleArea;
            }
            return result;
        }

        public double GetEnclosingRectangleArea(List<Rectangle> rectangles)
        {
            var vertexes = new List<Point>();
            foreach (var rectangle in rectangles)
            {
                foreach (var node in rectangle.GetRectangleNodes())
                {
                    vertexes.Add(node);
                }
            }
            var sortListXCords = vertexes.Select(vertex => vertex.X).ToList().OrderBy(x => x).ToList();
            var sortListYCords = vertexes.Select(vertex => vertex.Y).ToList().OrderBy(y => y).ToList();
            return (sortListYCords.Last() - sortListYCords.First()) * ( sortListXCords.Last() -sortListXCords.First());
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
