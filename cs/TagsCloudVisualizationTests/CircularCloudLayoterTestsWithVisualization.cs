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
    public class TestsCircularCloudLayouterWithVisualization
    {
        private CircularCloudLayouter _layouter;
        private readonly Random _random = new Random();
        private List<Rectangle> RectanglesList
        { get; set; }

        [TearDown]
        public void VisualizeError()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                //var visualization = new Visualization(_layouter.AllRectangles().ToList(), new Pen(Color.White, 3));
                var visualization = new Visualization(RectanglesList, new Pen(Color.White, 3));
                var testName = TestContext.CurrentContext.Test.Name;
                var path = AppDomain.CurrentDomain.BaseDirectory + "\\" + testName + "." + ImageFormat.Jpeg;
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
                visualization.DrawAndSaveImage(new Size(5000, 5000), path, ImageFormat.Jpeg);
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
            _layouter = new CircularCloudLayouter(layouterCenter);
            Action put = () => _layouter.PutNextRectangle(rectangleSize);
            put.Should().NotThrow();
        }

        [TestCase(20, 10)]
        [TestCase(20, -10)]
        [TestCase(-15, -5)]
        [TestCase(-15, 5)]
        public void ShouldNotIntersectWithRectangles(int width, int height) // список
        {
            var layouterCenter = new Point(width, height);
            _layouter = new CircularCloudLayouter(layouterCenter);
            RectanglesList = new List<Rectangle>();

            var random = new Random();
            for (int i = 0; i < 300; i++)
            {
                var rectangleSize = new Size(random.Next(-50, 50), random.Next(-50, 50));
                RectanglesList.Add(_layouter.PutNextRectangle(rectangleSize));
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
            _layouter = new CircularCloudLayouter(layouterCenter);
            var rectangle = _layouter.PutNextRectangle(rectangleSize);
            rectangle.Location.Should().Be(layouterCenter);
        }

        [Test]
        public void RectanglesShouldBeInCircle()
        {
            var rectangleSize = new Size(60, 120);
            var layouterCenter = new Point(2000, 1000);
            RectanglesList = new List<Rectangle>();
            _layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 300; i++)
                RectanglesList.Add(_layouter.PutNextRectangle(rectangleSize));
            var sumArea = GetSumAreaOfRectangles(RectanglesList);
            var circleArea = GetCircleArea(GetCircleRadius(layouterCenter, RectanglesList));
            var density = sumArea / circleArea;
            density.Should().BeLessThan(1);
        }

        [Test]
        public void ShouldBeCloserToCircleThanToSquare()
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            RectanglesList = new List<Rectangle>();
            _layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 100; i++)
                RectanglesList.Add(_layouter.PutNextRectangle(rectangleSize));
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
            _layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 300; i++)
                RectanglesList.Add(_layouter.PutNextRectangle(rectangleSize));
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
            return (yMax - yMin) * (xMax - xMin);
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
