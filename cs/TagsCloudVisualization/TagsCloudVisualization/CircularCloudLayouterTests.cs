using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void InitLayouter()
        {
            layouter = new CircularCloudLayouter();
        }

        [Test]
        public void PutNextRectangle_Throws_WhenSizeIsInvalid()
        {
            Action act = () => layouter.PutNextRectangle(new Size(-1, -1));

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleFromSpiralPointsGenerator()
        {
            var pointsGenerator = new SpiralPointsGenerator();

            for (var i = 0; i < 100; i++)
            {
                var rectangle = layouter.PutNextRectangle(new Size(1, 1));
                pointsGenerator.GetSpiralPoints().Should().Contain(p => p.Equals(rectangle.Location));
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangle_WhichNotIntersectsWithPreviousRectangles()
        {
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 100; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(new Size(1, 1)));
            }

            for (var i = 0; i < rectangles.Count; i++)
            {
                for (var j = 0; j < rectangles.Count && j != i; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().Be(false);
                }
            }
        }
    }
}