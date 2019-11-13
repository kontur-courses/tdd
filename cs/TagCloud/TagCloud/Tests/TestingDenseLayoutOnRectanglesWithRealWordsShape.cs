using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagCloud.Layouter;

namespace TagCloud.Tests
{
    internal class TestingDenseLayoutOnRectanglesWithRealWordsShape : OnFailDrawer
    {
        private static bool IsSymmetricOverAxis(IReadOnlyCollection<Rectangle> rectangles,
            Func<Rectangle, int> coordinateSelector,
            Func<Rectangle, int> sideSelector)
        {
            var min = rectangles.Min(coordinateSelector);
            var max = rectangles.Select(rect => coordinateSelector(rect) + sideSelector(rect)).Max();

            return Math.Abs(Math.Abs(max) - Math.Abs(min)) < rectangles.Sum(sideSelector) / rectangles.Count;
        }

        [SetUp]
        public void SetUp()
        {
            foreach (var testSize in RandomTestSizes)
            {
                cloudLayouter.PutNextRectangle(testSize);
            }
        }

        [Test]
        public void Should_DenselyPlaceRectangles_SymmetricByCoordinates()
        {
            // This test also checks that figure inscribes in square
            var rectangles = cloudLayouter.GetAllRectangles().ToList();

            IsSymmetricOverAxis(rectangles, rect => rect.X, rect => rect.Width)
                .Should()
                .BeTrue("Figure don't symmetric over x axis");
            IsSymmetricOverAxis(rectangles, rect => rect.Y, rect => rect.Height)
                .Should()
                .BeTrue("Figure don't symmetric over y axis");
        }

        [Test]
        public void Should_DenselyLocateRectangles_InCircumscribedSquare()
        {
            var circumscribedSquare = cloudLayouter.GetCircumscribedSquare();
            var actualRectanglesAres = cloudLayouter.GetAllRectangles().Sum(rect => rect.Width * rect.Height);

            var denseCoefficient =
                (double) actualRectanglesAres / (circumscribedSquare.Width * circumscribedSquare.Width);
            denseCoefficient
                .Should()
                .BeGreaterThan(0.5);
        }

        [Test]
        public void Should_EquallyStretchFigure_InBothAxis()
        {
            var stretchCoefficient = (double) cloudLayouter.GetInscribedSquare().Width /
                                     cloudLayouter.GetCircumscribedSquare().Width;
            stretchCoefficient
                .Should()
                .BeGreaterOrEqualTo(0.8, "Figure stretched a lot in one axis");
        }

        [Test]
        public void Should_DenselyPlaceRectangles_InShapeLikeSquare()
        {
            var rectangles = cloudLayouter.GetAllRectangles().ToList();
            var inscribedSquare = cloudLayouter.GetInscribedSquare();

            // check if we don't have outliers
            rectangles
                .All(rect => inscribedSquare.IntersectsWith(rect))
                .Should()
                .BeTrue("Inner square should intersect with every rectangle");
        }

        private static readonly Random Random = new Random();

        private IEnumerable<Size> RandomTestSizes =>
            Enumerable
                .Range(0, 500)
                .Select(num => new Size(Random.Next(20, 40), Random.Next(8, 12)));
    }
}