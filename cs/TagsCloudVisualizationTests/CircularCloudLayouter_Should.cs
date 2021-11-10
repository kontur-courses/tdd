using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void NotThrow_InConstructor()
        {
            var startPoint = new Point();
            Action lambda = () => new SimpleCircularCloudLayouter(startPoint);
            lambda.Should().NotThrow();
        }

        [Test]
        public void ReturnRectangleWithCenterInStartPosition_OnFirstIteration()
        {
            var startPoint = new Point(100, 200);
            var size = new Size(500, 300);
            var expectedRectangle = new Rectangle(startPoint - size / 2, size);
            var actualRectangle = new SimpleCircularCloudLayouter(startPoint).PutNextRectangle(size);
            actualRectangle.Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void TestIntersection()
        {
            var rect1 = new Rectangle(100, 200, 50, 50);
            var rect2 = new Rectangle(50, 150, 50, 50);
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnRectanglesAroundCenter_WhenSquaresWithSizeGiven()
        {
            var center = new Point();
            var layouter = new SimpleCircularCloudLayouter(center);
            var size = new Size(2, 2);
            var startPoint = center - size / 2;
            var horizontalOffset = new Size(size.Width, 0);
            var verticalOffset = new Size(0, size.Height);
            var expected = new List<Rectangle>()
            {
                new Rectangle(startPoint, size),
                new Rectangle(startPoint + horizontalOffset, size),
                new Rectangle(startPoint + horizontalOffset - verticalOffset, size),
                new Rectangle(startPoint + horizontalOffset + verticalOffset, size),
                new Rectangle(startPoint - horizontalOffset, size),
                new Rectangle(startPoint - horizontalOffset + verticalOffset, size),
                new Rectangle(startPoint - horizontalOffset - verticalOffset, size),
                new Rectangle(startPoint - verticalOffset, size),
                new Rectangle(startPoint + verticalOffset, size)
            };

            var actual = Enumerable
                .Range(1, expected.Count)
                .Select(x => layouter.PutNextRectangle(size))
                .ToList();
            actual.Should().BeEquivalentTo(expected,
                config =>
                    config.WithoutStrictOrdering());
        }

        [Test]
        public void PutNextRectangle_ReturnNotIntersectedRectangles_AfterMultipliedIterations()
        {
            var startPoint = new Point(100, 200);
            var random = new Random(12345);
            var layouter = new SimpleCircularCloudLayouter(startPoint);
            var rectanglesCount = 50;
            var rectangles = new List<Rectangle>(rectanglesCount);
            for (var i = 0; i < rectanglesCount; i++)
            {
                var rectangleSize = new Size(random.Next(1, 100), random.Next(1, 100));
                rectangles.Add(layouter.PutNextRectangle(rectangleSize));
            }
            
            for (int i = 0; i < rectanglesCount; i++)
            {
                for (int j = i + 1; j < rectanglesCount; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void PutNextRectangle_ReturnRectanglesWithTightDistribution()
        {
            var startPoint = new Point(100, 200);
            var layouter = new SimpleCircularCloudLayouter(startPoint);
            var allowedMaxDistance = 1000;
            var sizes = new List<Size>()
            {
                new Size(100, 100),
                new Size(50, 100),
                new Size(100, 50),
                new Size(100, 200),
                new Size(200, 100),
                new Size(150, 150),
                new Size(300, 300),
                new Size(400, 400)
            };

            var rectangles = sizes.Select(x => layouter.PutNextRectangle(x)).ToList();
            var maxDistance = 0d;
            foreach (var rect1 in rectangles)
            {
                foreach (var rect2 in rectangles)
                {
                    var deltaLocation = rect1.Location - (Size) rect2.Location;
                    maxDistance = Math.Max(maxDistance,
                        Math.Sqrt(deltaLocation.X * deltaLocation.X) + Math.Sqrt(deltaLocation.Y * deltaLocation.Y));
                }
            }

            maxDistance.Should().BeLessThan(allowedMaxDistance);
        }

    }
}