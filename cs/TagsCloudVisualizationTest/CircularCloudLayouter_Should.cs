using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest
{
    class CircularCloudLayouter_Should
    {
        private Point center;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void ReturnRectangleWithSameSize()
        {
            var rectangleSize = new Size(10, 10);
            var placedRectangle = layouter.PutNextRectangle(rectangleSize);
            Assert.AreEqual(rectangleSize, placedRectangle.Size);
        }

        [Test]
        public void PlaceFirstRectangleCenterNearToCenter()
        {
            var rectangleSize = new Size(9, 9);
            var placedRectangle = layouter.PutNextRectangle(rectangleSize);
            var rectangleCenter = placedRectangle.GetCenter();

            Assert.True(Math.Abs(center.X - rectangleCenter.X) <= 1);
            Assert.True(Math.Abs(center.Y - rectangleCenter.Y) <= 1);
        }

        [Test]
        public void PlaceTwoRectangles_SoThatTheyDoNotIntersect()
        {
            var rectangleSize = new Size(10, 20);
            var firstPlacedRectangle = layouter.PutNextRectangle(rectangleSize);
            var secondPlacedRectangle = layouter.PutNextRectangle(rectangleSize);

            Assert.False(firstPlacedRectangle.IntersectsWith(secondPlacedRectangle));
        }

        [Test]
        public void PlaceManyRectangles_SoThatTheyDoNotIntersect()
        {
            var rectangleSize = new Size(10, 20);
            var placedRectangles = new List<Rectangle>();
            for (int i = 0; i < 50; i++)
            {
                var nextRectangle = layouter.PutNextRectangle(rectangleSize);
                Assert.False(nextRectangle.IntersectsWithAny(placedRectangles));
                placedRectangles.Add(nextRectangle);
            }
        }

        [Test]
        public void PlaceRectanglesWithGoodDensity()
        {
            var rectangleSize = new Size(10, 20);
            var placedRectangles = new List<Rectangle>();
            for (int i = 0; i < 50; i++)
            {
                var nextRectangle = layouter.PutNextRectangle(rectangleSize);
                placedRectangles.Add(nextRectangle);
            }

            Assert.True(CalculateDensity(placedRectangles) > 0.6);
        }

        private double CalculateDensity(List<Rectangle> rectangles)
        {
            var areaSum = rectangles.Select(r => r.Size.Width * r.Size.Height).Sum();
            var topBorder = rectangles.Select(r => r.Top).Min();
            var bottomBorder = rectangles.Select(r => r.Bottom).Max();
            var rightBorder = rectangles.Select(r => r.Right).Max();
            var leftBorder = rectangles.Select(r => r.Left).Min();

            var bigRectangleSize = (rightBorder - leftBorder) * (bottomBorder - topBorder);
            return (double) areaSum / bigRectangleSize;
        }
}
}
