using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        [TestCase(0, 0)]
        [TestCase(3, 5)]
        [TestCase(13, 8)]
        [TestCase(-10, -7)]
        [TestCase(-9, -15)]
        public void Constructor_DoesNotThrow(int x, int y)
        {
            Action action = () => new CircularCloudLayouter(Point.Empty);
            action.Should().NotThrow();
        }

        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void PutNextRectangle_ThrowsOnNonPositiveSizes(int width, int height)
        {
            var circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
            var rectangleSize = new Size(width, height);

            circularCloudLayouter
                .Invoking(ccl => ccl.PutNextRectangle(rectangleSize))
                .Should().Throw<ArgumentException>()
                .WithMessage("rectangleSize is not correct rectangle size");
        }

        [TestCase(35, 17)]
        [TestCase(24, 47)]
        [TestCase(57, 57)]
        public void PutNextRectangle_RectangleHasCorrectSize(int width, int height)
        {
            var circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
            var rectangleSize = new Size(width, height);

            var rectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);

            rectangle.Size.Should().Be(rectangleSize);
        }

        [TestCase(0, 0, 4, 15)]
        [TestCase(30, 200, 5, 5)]
        public void PutNextRectangle_FirstRectangleContainsCenterPoint(int centerX, int centerY, int width, int height)
        {
            var center = new Point(centerX, centerY);
            var circularCloudLayouter = new CircularCloudLayouter(center);
            var rectangleSize = new Size(width, height);

            var rectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);

            rectangle.Contains(center).Should().BeTrue();
        }

        [TestCase(20, 20, 20, 20, 2)]
        [TestCase(30, 15, 30, 15, 2)]
        [TestCase(7, 10, 7, 10, 2)]
        [TestCase(20, 20, 20, 20, 20)]
        [TestCase(7, 10, 7, 10, 20)]
        [TestCase(25, 15, 25, 15, 20)]
        [Repeat(10)]
        [TestCase(5, 5, 10, 10, 2)]
        [TestCase(5, 5, 10, 10, 200)]
        public void PutNextRectangle_RectanglesOfRandomSizesDoNotIntersect(int minWidth, int minHeight, int maxWith,
            int maxHeight, int count)
        {
            var circularCloudLayouter = new CircularCloudLayouter(Point.Empty);

            var rectangles = PutRandomRectanglesUsingLayouter(count, circularCloudLayouter,
                new Size(minWidth, minHeight), new Size(maxWith, maxHeight));

            TestExecutionContext.CurrentContext.CurrentTest.Properties.Set("rectangles", rectangles);

            var intersectingRectangles =
                rectangles.Where(r1 => rectangles.Any(r2 => r2 != r1 && r2.IntersectsWith(r1))).ToList();
            TestExecutionContext.CurrentContext.CurrentTest.Properties.Set("intersectingRectangles",
                intersectingRectangles);
            var rectanglesIntersect = intersectingRectangles.Any();
            rectanglesIntersect.Should().BeFalse();
        }

        [TestCase(50, 50, 50, 50, 10, 0.45)]
        [TestCase(30, 10, 30, 10, 10, 0.45)]
        [TestCase(10, 30, 10, 30, 10, 0.45)]
        [TestCase(40, 40, 40, 40, 100, 0.65)]
        [TestCase(30, 10, 30, 10, 100, 0.65)]
        [TestCase(10, 30, 10, 30, 100, 0.65)]
        [Retry(10)]
        [TestCase(5, 5, 10, 10, 10, 0.5)]
        [TestCase(5, 5, 10, 10, 100, 0.65)]
        public void TagsCloudOfRandomSizedRectanglesIsDense(int minWidth, int minHeight, int maxWith, int maxHeight,
            int count, double requiredDensity)
        {
            var center = Point.Empty;
            var circularCloudLayouter = new CircularCloudLayouter(center);

            var rectangles = PutRandomRectanglesUsingLayouter(count, circularCloudLayouter,
                new Size(minWidth, minHeight), new Size(maxWith, maxHeight));

            TestExecutionContext.CurrentContext.CurrentTest.Properties.Set("rectangles", rectangles);

            var radius = GetRadiusOfCircleIncludingAllRectangles(rectangles, center);
            var circleArea = radius * radius * Math.PI;
            var area = rectangles.Sum(rectangle => rectangle.Width * rectangle.Height);
            (area / circleArea).Should().BeGreaterThan(requiredDensity);
        }

        [TestCase(50, 50, 50, 50, 10, 9)]
        [TestCase(30, 10, 30, 10, 10, 9)]
        [TestCase(60, 20, 60, 20, 10, 9)]
        [TestCase(30, 10, 30, 10, 1000, 950)]
        [TestCase(60, 20, 60, 20, 1000, 950)]
        [Retry(5)]
        [TestCase(5, 5, 10, 10, 10, 7)]
        public void TagsCloudOfRandomSizedRectanglesShapeIsCloseToCircle(int minWidth, int minHeight, int maxWith,
            int maxHeight, int count, int rectanglesInCircleRequired)
        {
            var center = Point.Empty;
            var circularCloudLayouter = new CircularCloudLayouter(center);

            var rectangles = PutRandomRectanglesUsingLayouter(count, circularCloudLayouter,
                new Size(minWidth, minHeight), new Size(maxWith, maxHeight));

            TestExecutionContext.CurrentContext.CurrentTest.Properties.Set("rectangles", rectangles);

            var radius = GetRadiusOfCircleIncludingAllRectangles(rectangles, center);
            var rectanglesInCircleCount = rectangles.Count(rectangle => IsRectangleInCircle(rectangle, center, radius));
            rectanglesInCircleCount.Should().BeGreaterOrEqualTo(rectanglesInCircleRequired);
        }

        [TearDown]
        public void TearDown()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome;
            if (outcome != ResultState.Error && outcome != ResultState.Failure) return;
            if (!(TestContext.CurrentContext.Test.Properties.Get("rectangles") is List<Rectangle> rectangles)) return;
            var tagsCloudImage = new TagsCloudImage(1920, 1080);
            tagsCloudImage.AddRectangles(rectangles, Color.Black, 1f);

            if (TestContext.CurrentContext.Test.Properties.Get("intersectingRectangles") is List<Rectangle>
                intersectingRectangles)
            {
                tagsCloudImage.AddRectangles(intersectingRectangles, Color.Red, 1f);
            }

            var fileName = TestContext.CurrentContext.Test.Name + "failed.png";
            var exactPath = Path.GetFullPath(fileName);
            tagsCloudImage.GetBitmap().Save(exactPath);
            Console.WriteLine("Tag cloud visualization saved to file {0}", exactPath);
        }

        private static List<Rectangle> PutRectanglesUsingLayouter(int rectanglesCount, CircularCloudLayouter layouter,
            Size rectangleSize)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < rectanglesCount; ++i)
                rectangles.Add(layouter.PutNextRectangle(rectangleSize));
            return rectangles;
        }

        private static List<Rectangle> PutRandomRectanglesUsingLayouter(int rectanglesCount,
            CircularCloudLayouter layouter, Size minSize, Size maxSize)
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < rectanglesCount; ++i)
                rectangles.Add(layouter.PutNextRectangle(
                    new Size(random.Next(minSize.Width, maxSize.Width), random.Next(minSize.Height, maxSize.Height))
                ));
            return rectangles;
        }


        #region Helper functions

        private static int GetRadiusOfCircleIncludingAllRectangles(IReadOnlyCollection<Rectangle> rectangles,
            Point center)
        {
            var left = rectangles.Aggregate(center.X,
                (leftmost, rectangle) => rectangle.Left < leftmost ? rectangle.Left : leftmost);
            var right = rectangles.Aggregate(center.X,
                (rightmost, rectangle) => rectangle.Right > rightmost ? rectangle.Right : rightmost);

            var top = rectangles.Aggregate(center.Y,
                (topmost, rectangle) => rectangle.Top < topmost ? rectangle.Top : topmost);
            var bottom = rectangles.Aggregate(center.Y,
                (bottommost, rectangle) => rectangle.Bottom > bottommost ? rectangle.Bottom : bottommost);

            var radius = 0;

            if (Math.Abs(left - center.X) > radius)
                radius = Math.Abs(left - center.X);
            if (Math.Abs(right - center.X) > radius)
                radius = Math.Abs(right - center.X);
            if (Math.Abs(top - center.Y) > radius)
                radius = Math.Abs(top - center.Y);
            if (Math.Abs(bottom - center.Y) > radius)
                radius = Math.Abs(bottom - center.Y);

            return radius;
        }

        private static bool IsRectangleInCircle(Rectangle rectangle, Point circleCenter, int circleRadius)
        {
            return IsPointInCircle(new Point(rectangle.Left, rectangle.Top), circleCenter, circleRadius) &&
                   IsPointInCircle(new Point(rectangle.Left, rectangle.Bottom), circleCenter, circleRadius) &&
                   IsPointInCircle(new Point(rectangle.Right, rectangle.Top), circleCenter, circleRadius) &&
                   IsPointInCircle(new Point(rectangle.Right, rectangle.Bottom), circleCenter, circleRadius);
        }

        private static bool IsPointInCircle(Point point, Point circleCenter, int circleRadius)
        {
            var pointXRelative = point.X - circleCenter.X;
            var pointYRelative = point.Y - circleCenter.Y;
            return pointXRelative * pointXRelative + pointYRelative * pointYRelative <= circleRadius * circleRadius;
        }

        #endregion
    }
}