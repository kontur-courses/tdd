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

        //private readonly Random _seedRandom = new Random(new Random().Next(10,10));
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
                    var path = AppDomain.CurrentDomain.BaseDirectory + testName + "." + ImageFormat.Jpeg;
                    Console.WriteLine($"Tag cloud visualization saved to file {path}");
                    visualization.DrawAndSaveImage(new Size(5000, 5000), path, ImageFormat.Jpeg);
                }
            }
        }


        [TestCase(0, 1)]
        [TestCase(1,0)]
        public void ShouldThrowExceptionWhenPutRectangleWithZeroEdge(int width, int height)
        {
            var ceedRandom = new Random(new Random().Next(0));
            var rectangleSize = new Size(width, height);
            var layouterCenter = new Point(ceedRandom.Next(-100,100), ceedRandom.Next(-100,100));
            Layouter = new CircularCloudLayouter(layouterCenter);
            Action put = () => Layouter.PutNextRectangle(rectangleSize);
            put.Should().Throw<ArgumentException>();
        }

        [TestCase(-5, 10)]
        [TestCase(5, -5)]
        [TestCase(10, 10)]
        public void ShouldNotThrowIfPutRectangleWithCorrectSize(int width, int height)
        {
            var ceedRandom = new Random(new Random().Next(0));
            var rectangleSize = new Size(width, height);
            var layouterCenter = new Point(ceedRandom.Next(-100, 100), ceedRandom.Next(-100, 100));
            Layouter = new CircularCloudLayouter(layouterCenter);
            Action put = () => Layouter.PutNextRectangle(rectangleSize);
            put.Should().NotThrow();
        }

        [TestCase(20, 10)]
        [TestCase(20, -10)]
        [TestCase(-15, -5)]
        [TestCase(-15, 5)]
        public void ShouldNotIntersectWithRectangles(int width, int height)
        {
            var ceedRandom = new Random(new Random().Next(0));
            var layouterCenter = new Point(width, height);
            Layouter = new CircularCloudLayouter(layouterCenter);
            RectanglesList = new List<Rectangle>();

            var random = new Random();
            for (int i = 0; i < 300; i++)
            {
                var rectangleSize = new Size(ceedRandom.Next(-100,100), ceedRandom.Next(-100,100));
                if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                {
                    i--;
                    continue;
                }

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
            var ceedRandom = new Random(new Random().Next(0));
            var rectangleSize = new Size(ceedRandom.Next(-100,100), ceedRandom.Next(-100,100));
            var layouterCenter = new Point(Size.Empty);
            while (layouterCenter.X == 0 || layouterCenter.Y == 0)
                layouterCenter = new Point(ceedRandom.Next(-100, 100), ceedRandom.Next(-100,100));
            Layouter = new CircularCloudLayouter(layouterCenter);
            var rectangle = Layouter.PutNextRectangle(rectangleSize);
            rectangle.Location.Should().Be(layouterCenter);
        }

        [TestCase(300)]
        [TestCase(500)]
        [TestCase(1000)]
        public void FormShouldBeCloserToCircleThanToSquareWhenManyRectangles(int number)
        {
            var ceedRandom = new Random(new Random().Next(0));
            var rectangleSize = new Size(new Random().Next(30, 100), new Random().Next(30, 100));
            var layouterCenter = new Point(ceedRandom.Next(-100,100), ceedRandom.Next(-100,100));
            RectanglesList = new List<Rectangle>();
            Layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < number; i++)
            {
                if (rectangleSize.Height == 0 || rectangleSize.Width == 0)
                {
                    i--;
                    continue;
                }
                RectanglesList.Add(Layouter.PutNextRectangle(rectangleSize));
            }
            var sumArea = GetSumAreaOfRectangles(RectanglesList);
            var circleArea = GetCircleArea(GetCircleRadius(layouterCenter, RectanglesList));
            
            var enclosingRectangleArea = GetEnclosingRectangleArea(RectanglesList);
            var difCircleAndSum = sumArea/circleArea;
            var difSumAndEnclosingRectangle = sumArea/enclosingRectangleArea;

            difCircleAndSum.Should().BeGreaterThan(difSumAndEnclosingRectangle);
        }

        [Test]
        public void SameRectanglesShouldNotIntersect()
        {
            var ceedRandom = new Random(new Random().Next(0));
            var rectangleSize = new Size(ceedRandom.Next(-100,100), ceedRandom.Next(-100,100));
            var layouterCenter = new Point(ceedRandom.Next(-100,100), ceedRandom.Next(-100,100));
            RectanglesList = new List<Rectangle>();
            Layouter = new CircularCloudLayouter(layouterCenter);
            for (int i = 0; i < 300; i++)
            {
                if (rectangleSize.Height == 0 || rectangleSize.Width == 0)
                {
                    i--;
                    continue;
                }
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

        private double GetCircleRadius(Point layouterCenter, List<Rectangle> rectangles)
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

        private double GetSumAreaOfRectangles(List<Rectangle> rectangles)
        {
            double result = 0;
            foreach (var rectangle in rectangles)
                result += Math.Abs(rectangle.Height) * Math.Abs(rectangle.Width);
            return result;
        }

        //private int GetNewRandom() => _seedRandom.Next();

        private double GetEnclosingRectangleArea(List<Rectangle> rectangles)
        {
            var xMax = int.MinValue;
            var xMin = int.MaxValue;
            var yMin = int.MaxValue;
            var yMax = int.MinValue;
            foreach (var rectangle in rectangles)
            {
                foreach (var node in rectangle.GetRectangleNodes())
                {
                    if (node.X > xMax)
                        xMax = node.X;
                    if (node.Y > yMax)
                        yMax = node.Y;
                    if (node.X < xMin)
                        xMin = node.X;
                    if (node.Y < yMin)
                        yMin = node.Y;
                }
            }
            return (xMax - xMin) * (yMax - yMin);
        }

        private double GetCircleArea(double circleRadius)
        {
            if (circleRadius <= 0)
                throw new ArgumentException();
            var area = Math.PI * Math.Pow(circleRadius, 2);
            return area;
        }
    }
}
