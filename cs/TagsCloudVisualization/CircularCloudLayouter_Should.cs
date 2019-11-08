using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private static CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [TestCase(2, 2, ExpectedResult = "{X=-1,Y=-1}", TestName = "OnEvenSize")]
        [TestCase(1, 1, ExpectedResult = "{X=0,Y=0}", TestName = "OnOddSize")]
        public string PlaceFirstRectangle_InCenter(int width, int height)
        {
            var size = new Size(width, height);
            var rect = layouter.PutNextRectangle(size);
            return rect.Location.ToString();
        }

        [Test]
        public void PlaceRectangles_WithoutIntersection()
        {
            var rect1 = layouter.PutNextRectangle(new Size(1, 1));
            var rect2 = layouter.PutNextRectangle(new Size(1, 1));

            rect1.IntersectsWith(rect2).Should().Be(false);
        }

        private static IEnumerable LayouterTestCases
        {
            get
            {
                yield return new TestCaseData(new List<Size>
                {
                    new Size(2, 2),
                    new Size(2, 2),
                    new Size(2, 2),
                    new Size(2, 2),
                    new Size(2, 2),
                    new Size(2, 2),
                    new Size(2, 2)
                });
            }
        }

        [Test, TestCaseSource(nameof(LayouterTestCases))]
        public void PlaceRectangles_CloseToEachOther(IEnumerable<Size> rectSizes)
        {
            foreach (var size in rectSizes)
            {
                layouter.PutNextRectangle(size);
            }

            var circleArea = GetСircumscribedСircleArea(layouter);
            var layoutArea = GetArea(layouter);

            (layoutArea / circleArea).Should()
                .BeGreaterThan(0.6, $"layout area = {layoutArea}, circle area = {circleArea}");
        }

        /// <summary>
        /// Расчитывает площадь описаной вокруг всех прямоугольников окружности
        /// </summary>
        private static double GetСircumscribedСircleArea(CircularCloudLayouter layouter)
        {
            var maxRadius = layouter.Rectangles.SelectMany(GetCornerPoints)
                .Select(point => GetDistance(layouter.Center, point))
                .Max();
            return Math.PI * maxRadius * maxRadius;
        }

        private static IEnumerable<Point> GetCornerPoints(Rectangle rectangle)
        {
            yield return new Point(rectangle.Left, rectangle.Top);
            yield return new Point(rectangle.Left, rectangle.Bottom);
            yield return new Point(rectangle.Right, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Bottom);
        }

        private static int GetArea(CircularCloudLayouter layouter)
        {
            return layouter.Rectangles.Select(rectangle => rectangle.Height * rectangle.Width).Sum();
        }

        private static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}