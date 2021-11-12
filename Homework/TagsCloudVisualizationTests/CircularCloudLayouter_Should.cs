using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private const float Epsilon = 1e-5f;
        
        [Test]
        [TestCaseSource(typeof(Generators), nameof(Generators.CenterGenerator))]
        public void PutRectangles_WithoutIntersections_WithCustomCenter(Point center)
        {
            var layouter = new CircularCloudLayouter(center);

            var rectangles = Generators.RectanglesSizeGenerator()
                .Select(r => layouter.PutNextRectangle(r));

            rectangles
                .SelectMany(firstRect => rectangles
                    .Select(secondRect => firstRect != secondRect && firstRect.IntersectsWith(secondRect)))
                .Should()
                .NotContain(true);
        }

        [Test]
        public void PutNextRectangle_ThrowsArgumentException_WhenSizeIsNegative()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            Action act = () => layouter.PutNextRectangle(new SizeF(-1, -1));

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCaseSource(typeof(Generators), nameof(Generators.CenterGenerator))]
        public void PutNextRectangle_PutFirstRectangleAtCenter_WithCustomCenter(Point center)
        {
            foreach (var rectangleSize in Generators.RectanglesSizeGenerator())
            {
                var layouter = new CircularCloudLayouter(center);

                var rectangle = layouter.PutNextRectangle(rectangleSize);

                rectangle.X.Should().BeApproximately(center.X - rectangleSize.Width / 2, Epsilon);
                rectangle.Y.Should().BeApproximately(center.Y - rectangleSize.Height / 2, Epsilon);
            }
        }

        [Test]
        public void PutNextRectangle_AverageCloudDensity_GreaterThan70Percent()
        {
            var avgDensity = Generators.CenterGenerator()
                .Select(center => GetCloudDensity(center, Generators.RectanglesSizeGenerator()))
                .Average();

            avgDensity.Should().BeGreaterThan(0.7);
        }

        private double GetCloudDensity(Point center, IEnumerable<SizeF> rectanglesSizes)
        {
            var layouter = new CircularCloudLayouter(center);

            var rectanglesArea = 0.0f;

            var lastRectangle = new RectangleF();
            
            foreach (var rectangleSize in rectanglesSizes)
            {
                lastRectangle = layouter.PutNextRectangle(rectangleSize);

                rectanglesArea += lastRectangle.GetArea();
            }

            var floatCenter = new PointF(center.X, center.Y);

            var circleRadius = floatCenter.DistanceTo(lastRectangle.Location);
            var circleArea = circleRadius * circleRadius * Math.PI;

            var density = rectanglesArea / circleArea;

            return density;
        }
    }
}