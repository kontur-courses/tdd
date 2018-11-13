using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private List<Rectangle> result = new List<Rectangle>();
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(400, 400);
            layouter = new CircularCloudLayouter(center);
            result = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            var path = $"C:\\Users\\{Environment.UserName}\\Desktop\\{TestContext.CurrentContext.Test.Name}";
            var visualizer = new Visualizer(Color.Blue, Color.Black, Color.Yellow);
            var bitmap = visualizer.RenderToBitmap(result, center.X * 2, center.Y * 2);
            ImageSaver.SaveBitmapToFile(path, bitmap);
            Console.WriteLine("Tag cloud visualization saved to file {0}.bmp", path);
        }

        [TestCase(0, 0, 10, 6, Description = "Zero center")]
        [TestCase(-5, 4, 10, 6, Description = "Non zero center")]
        [TestCase(2, 2, 7, 3, Description = "Odd width and height")]
        public void PutFirstRectangle_InCenter(int x, int y, int width, int height)
        {
            var layouter = new CircularCloudLayouter(new Point(x, y));
            layouter.PutNextRectangle(new Size(width, height)).Location
                .IsSameOrEqualTo(new Point(x - width / 2, y - height / 2));
        }

        [TestCase(new[] { 5, 5, 5, 5 }, new[] { -5, -5, 5, 5 }, ExpectedResult = false)]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { 2, 2, 5, 5 }, ExpectedResult = true, Description = "Internal rectangle")]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { -2, -2, 5, 5 }, ExpectedResult = true)]
        [TestCase(new[] { -2, -2, 5, 5 }, new[] { 0, 0, 10, 10 }, ExpectedResult = true)]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { 0, 20, 10, 10 }, ExpectedResult = false, Description = "Y-Axis is parallel")]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { -10, 20, 10, 10 }, ExpectedResult = false)]
        public bool CheckCollision_BetweenTwoRectangles(int[] rect, int[] other)
        {
            return Geometry.IsRectangleIntersection(
                new Rectangle(rect[0], rect[1], rect[2], rect[3]),
                new Rectangle(other[0], other[1], other[2], other[3]));
        }

        [TestCase(40, 40, 15, 15, Description = "Equal rectangles")]
        [TestCase(10, 30, 10, 30, Description = "Square-like rectangles")]
        [TestCase(30, 50, 10, 20)]
        public void CloudOccupiesMoreThanHalfOfCircleArea(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                result.Add(layouter.PutNextRectangle(new Size(
                    rnd.Next(minWidth, maxWidth), rnd.Next(minHeight, maxHeight))));
            }

            var maxRadius = 0.0;
            var occupiedArea = 0.0;
            foreach (var rectangle in result)
            {
                occupiedArea += rectangle.Width * rectangle.Height;
                var radius = Geometry.GetMaxDistanceToRectangle(center, rectangle);
                if (radius > maxRadius)
                    maxRadius = radius;
            }

            (occupiedArea / (maxRadius * maxRadius * Math.PI)).Should().BeGreaterThan(0.5);
        }

        [TestCase(40, 40, 15, 15, Description = "Equal rectangles")]
        [TestCase(10, 30, 10, 30, Description = "Square-like rectangles")]
        [TestCase(30, 50, 10, 20)]
        public void ResultRectanglesDoNotIntersect(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                result.Add(layouter.PutNextRectangle(new Size(
                    rnd.Next(minWidth, maxWidth), rnd.Next(minHeight, maxHeight))));
            }

            var rest = result;
            foreach (var rectangle in result)
            {
                rest = rest.Skip(1).ToList();
                foreach (var other in rest)
                {
                    Geometry.IsRectangleIntersection(rectangle, other).Should().BeFalse();
                }
            }
        }
    }
}
