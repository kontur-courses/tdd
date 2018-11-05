using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayoutTests
    {
        private Point startPoint;
        private ICircularCloudLayout circularCloudLayouter;
        private ICollection<Rectangle> rectangles;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            startPoint = new Point();
            circularCloudLayouter = new CircularCloudLayout(startPoint);
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void DoAfterAnyTest()
        {
            if (TestContext.CurrentContext.Result.FailCount != 0)
            {
                var savePath = TestContext.CurrentContext.TestDirectory
                    + $"\\test_failed_{TestContext.CurrentContext.Test.Name}.bmp";

                new Visualizator(new Size(5000, 5000), rectangles)
                    .Generate()
                    .Save(savePath);
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsSameSizeRectangleAtStartPoint_OnFirstRectangle()
        {
            var size = new Size(100, 100);
            var expected = new Rectangle(startPoint, size);

            var result = circularCloudLayouter.PutNextRectangle(size);

            result
                .Equals(expected)
                .Should()
                .BeTrue();
        }

        [Test]
        public void PutNextRectangle_ReturnsNotIntersectedRectangle_OnCorrectSizes()
        {
            var random = new Random();

            for (var i = 0; i < 100; i++)
            {
                var size = new Size(random.Next(1, 100), random.Next(1, 100));
                var generatedRectangle = circularCloudLayouter.PutNextRectangle(size);

                foreach (var rectangle in rectangles)
                {
                    generatedRectangle.IntersectsWith(rectangle)
                        .Should()
                        .BeFalse(
                            "generated rectangle {0} mustn't intersect with already added rectangle {1}",
                            generatedRectangle,
                            rectangle);
                }

                rectangles.Add(generatedRectangle);
            }
        }

        #region InvalidSizes

        public static IEnumerable<Size> OnInvalidSizes()
        {
            yield return new Size(0, 0);
            yield return new Size(0, 1);
            yield return new Size(1, 0);
            yield return new Size(-1, 1);
            yield return new Size(1, -1);
            yield return new Size(-1, -1);
        }

        #endregion

        [TestCaseSource(nameof(OnInvalidSizes))]
        public void PutNextRectangle_ThrowsException_OnInvalidSize(Size size)
        {
            Action action = () => circularCloudLayouter.PutNextRectangle(size);

            action
                .Should()
                .Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_GeneratesCircleOfRectangles_OnCorrectSizes()
        {
            GenerateRectangles(200);
            var orderedXCoordinates = GetOrderedUnsignedXCoordinates();
            var orderedYCoordinates = GetOrderedUnsignedYCoordinates();
            var radiuses = new[]
                {
                    orderedXCoordinates.First(),
                    orderedXCoordinates.Last(),
                    orderedYCoordinates.Last(),
                    orderedYCoordinates.Last()
                }
                .ToArray();

            foreach (var rad1 in radiuses)
            {
                foreach (var rad2 in radiuses)
                {
                    ((double)rad1 / rad2)
                        .Should()
                        .BeGreaterOrEqualTo(0.7, "radiuses must be similar");
                }
            }
        }

        [Test]
        public void PutNextRectangle_GeneratesTightCloudOfRectangles_OnCorrectSizes()
        {
            GenerateRectangles(200);
            var orderedXCoordinates = GetOrderedUnsignedXCoordinates();
            var orderedYCoordinates = GetOrderedUnsignedYCoordinates();

            var maxRadius = Math.Max(
                Math.Max(orderedXCoordinates.First(), orderedXCoordinates.Last()),
                Math.Max(orderedYCoordinates.First(), orderedYCoordinates.Last()));

            var circleSquare = Math.PI * maxRadius * maxRadius;

            var squareOfRectangles =
                rectangles.Aggregate(
                    0.0,
                    (square, rectangle) => square + rectangle.Width * rectangle.Height);

            squareOfRectangles.Should()
                .BeGreaterOrEqualTo(circleSquare * 0.5);
        }

        private void GenerateRectangles(int amount)
        {
            var random = new Random();

            for (var i = 0; i < amount; i++)
            {
                var size = new Size(random.Next(1, 100), random.Next(1, 100));
                var rectangle = circularCloudLayouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }
        }

        private int[] GetOrderedUnsignedXCoordinates()
        {
            return rectangles
                .Select(rectangle => rectangle.X > startPoint.X ? rectangle.X + rectangle.Width : rectangle.X)
                .OrderBy(x => x)
                .Select(Math.Abs)
                .Select(x => x - startPoint.X)
                .ToArray();
        }

        private int[] GetOrderedUnsignedYCoordinates()
        {
            return rectangles
                .Select(rectangle => rectangle.Y < startPoint.Y ? rectangle.Y + rectangle.Height : rectangle.Y)
                .OrderBy(y => y)
                .Select(Math.Abs)
                .Select(y => y - startPoint.Y)
                .ToArray();
        }
    }
}