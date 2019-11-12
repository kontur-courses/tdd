using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TagCloud.Tests
{
    internal class TestingDenseLayoutOn100RandomRectangles : OnFailDrawer
    {
        private static bool IsSymmetricOverAxis(IReadOnlyCollection<Rectangle> rectangles, 
            Func<Rectangle, int> coordinateSelector,
            Func<Rectangle, int> sideSelector)
        {
            var min = rectangles.Min(coordinateSelector);
            var max = rectangles.Select(rect => coordinateSelector(rect) + sideSelector(rect)).Max();

            return Math.Abs(Math.Abs(max) - Math.Abs(min)) < rectangles.Sum(sideSelector) / rectangles.Count;
        }

        [Test]
        public void Should_DenselyPlaceRectangles_SymmetricByCoordinates()
        {
            // This test also checks that figure inscribes in square
            var rectangles = RandomTestRectangles.ToList();

            IsSymmetricOverAxis(rectangles, rect => rect.X, rect => rect.Width)
                .Should()
                .BeTrue("Figure don't symmetric over x axis");
            IsSymmetricOverAxis(rectangles, rect => rect.Y, rect => rect.Height)
                .Should()
                .BeTrue("Figure don't symmetric over y axis");
        }

        private static int ScaledCircleArea(int radius, int decimalMultiplier)
            => radius * radius * (int) (Math.PI * decimalMultiplier);

        private static int GetSuitableAbsCoordinateByAxis(IReadOnlyCollection<Rectangle> rectangles,
            Func<Rectangle, int> coordinateSelector,
            Func<Rectangle, int> sideSelector,
            Func<int, int, int> compareSelector)
        {
            return compareSelector(
                Math.Abs(rectangles.Min(coordinateSelector)), 
                rectangles.Max(rect => coordinateSelector(rect) + sideSelector(rect)));
        }

        [Test]
        public void Should_DenselyPlaceRectangles_InCircleArea()
        {
            var rectangles = RandomTestRectangles.ToList();

            var innerCircleRadius = Math.Min(
                GetSuitableAbsCoordinateByAxis(rectangles, rect => rect.X, rect => rect.Width, Math.Min),
                GetSuitableAbsCoordinateByAxis(rectangles, rect => rect.Y, rect => rect.Height, Math.Min));
            var actualRectanglesAres = rectangles.Sum(rect => rect.Width * rect.Height);

            ScaledCircleArea(innerCircleRadius, 100)
                .Should()
                .BeGreaterThan(100 * actualRectanglesAres, "Figure area should overlap inner circle area");
        }

        [Test]
        public void Should_DenselyPlaceRectangles_InShapeLikeSquare()
        {
            var rectangles = RandomTestRectangles.ToList();

            var innerSquareHalfSide = Math.Max(
                    GetSuitableAbsCoordinateByAxis(rectangles, rect => rect.X, rect => rect.Width, Math.Min),
                    GetSuitableAbsCoordinateByAxis(rectangles, rect => rect.Y, rect => rect.Height, Math.Min));
            
            var square = new Rectangle(
                -innerSquareHalfSide, 
                -innerSquareHalfSide, 
                2 * innerSquareHalfSide, 
                2 * innerSquareHalfSide);

            // check if we don't have outliers
            rectangles
                .All(rect => square.IntersectsWith(rect))
                .Should()
                .BeTrue("Inner square should intersect with every rectangle");
        }

        private IEnumerable<Rectangle> RandomTestRectangles =>
            Enumerable
                .Range(0, 100)
                .Select(num => new Size(Random.Next(1, 30), Random.Next(1, 30)))
                .Select(size => cloudLayouter.PutNextRectangle(size));

        private static readonly Random Random = new Random();
    }
}