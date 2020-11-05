using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [SetUp]
        public void SetUp()
        {
            center = new Point(10, 10);
            layouter = new CircularCloudLayouter(center);
            placedRectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var bitmap = CircularCloudDrawer.GetBitmap(placedRectangles, center);
            var fileName = $"Test_{TestContext.CurrentContext.Test.Name}_failed.jpg";
            BitmapSaver.Save(bitmap, fileName);
            TestContext.WriteLine(
                $"Tag cloud visualization saved to file {BitmapSaver.GetRelativePath(fileName)}");
        }

        private CircularCloudLayouter layouter;
        private Point center;
        private List<Rectangle> placedRectangles;

        [Test]
        public void PutRectangleWithCorrectSize()
        {
            var size = new Size(5, 5);
            
            placedRectangles.Add(layouter.PutNextRectangle(size));
            
            placedRectangles[0].Size.Should().Be(size);
        }

        [Test]
        public void PutFirstRectangleInCenter_WhenRectangleHeightIsEven()
        {
            GetRectangleCenter(layouter.PutNextRectangle(new Size(1, 2)))
                .Should().Be(center);
        }

        [Test]
        public void PutFirstRectangleInCenter_WhenRectangleWidthIsOddAndMoreThan1()
        {
            GetRectangleCenter(layouter.PutNextRectangle(new Size(3, 1)))
                .Should().Be(center);
        }

        [Test]
        public void PutFirstRectangleInCenter_WhenRectangleHeightIsOddAndMoreThan1()
        {
            GetRectangleCenter(layouter.PutNextRectangle(new Size(1, 3)))
                .Should().Be(center);
        }

        [Test]
        public void PutFirstRectangleInCenter_WhenRectangleHeightAndWidthMoreThan1()
        {
            GetRectangleCenter(layouter.PutNextRectangle(new Size(5, 8)))
                .Should().Be(center);
        }

        [Test]
        public void PutNotIntersectedRectangles_WhenRectanglesSquareEquals1()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(1, 1));
            var secondRectangle = layouter.PutNextRectangle(new Size(1, 1));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNotIntersectedRectangles_WhenRectanglesSquareMoreThan1()
        {
            var rectanglesSizes = new[]
            {
                new Size(5, 5),
                new Size(2, 8),
                new Size(7, 3),
                new Size(1, 1),
                new Size(3, 4),
                new Size(2, 2)
            };

            var placedRectangles = 
                rectanglesSizes.Select(rectangleSize => layouter.PutNextRectangle(rectangleSize)).ToList();

            for (var i = 0; i < rectanglesSizes.Length; i++)
            for (var j = i + 1; j < rectanglesSizes.Length; j++)
                placedRectangles[i].IntersectsWith(placedRectangles[j]).Should().BeFalse();
        }

        [Test]
        public void PutRectanglesInCircle_WhenRectanglesSquareEquals1()
        {
            var rectanglesSizes = new Size[25];
            for (var i = 0; i < rectanglesSizes.Length; i++) rectanglesSizes[i] = new Size(1, 1);

            var placedRectangles = 
                rectanglesSizes.Select(rectangleSize => layouter.PutNextRectangle(rectangleSize)).ToList();

            foreach (var placedRectangle in placedRectangles)
                GetDistanceToCenter(placedRectangle).Should().BeLessOrEqualTo(Math.Sqrt(8));
        }

        [Test]
        public void PutRectanglesInCircle_WhenRectanglesSquareEquals2()
        {
            var rectanglesSizes = new Size[8];
            for (var i = 0; i < rectanglesSizes.Length; i++) rectanglesSizes[i] = new Size(2, 2);

            var placedRectangles = 
                rectanglesSizes.Select(rectangleSize => layouter.PutNextRectangle(rectangleSize)).ToList();

            foreach (var placedRectangle in placedRectangles)
                GetDistanceToCenter(placedRectangle).Should().BeLessOrEqualTo(Math.Sqrt(8));
        }


        [Test]
        public void ThrowArgumentException_WhenSizeIsEmpty()
        {
            Action action = () => layouter.PutNextRectangle(new Size(0, 0));

            action.Should().Throw<ArgumentException>();
        }

        private Point GetRectangleCenter(Rectangle rectangle)
        {
            var x = rectangle.Left + rectangle.Width / 2;
            var y = rectangle.Top + rectangle.Height / 2;
            return new Point(x, y);
        }

        private double GetDistanceToCenter(Rectangle rectangle)
        {
            var rectangleCenter = GetRectangleCenter(rectangle);
            return Math.Sqrt((rectangleCenter.X - center.X) * (rectangleCenter.X - center.X)
                             + (rectangleCenter.Y - center.Y) * (rectangleCenter.Y - center.Y));
        }
    }
}