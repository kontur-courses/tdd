using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private const float Epsilon = 1e-5f;
        
        [Test]
        public void PutRectangles_WithoutIntersections()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            var rectangles = Generators.RectanglesRandomSizeGenerator()
                .Select(r => layouter.PutNextRectangle(r))
                .ToList(); // To avoid multiple enumeration

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
            var layouter = new CircularCloudLayouter(center);

            var rectangleSize = new SizeF(50, 50);

            var rectangle = layouter.PutNextRectangle(rectangleSize);

            rectangle.X.Should().BeApproximately(center.X - rectangleSize.Width / 2, Epsilon);
            rectangle.Y.Should().BeApproximately(center.Y - rectangleSize.Height / 2, Epsilon);
        }

        [Test]
        public void PutNextRectangle_AverageRandomSizeRectanglesCloudDensity_Between65And85Percent()
        {
            var avgDensity = Generators.CenterGenerator()
                .Select(center => GetCloudDensity(center, Generators.RectanglesRandomSizeGenerator()))
                .Average();

            avgDensity.Should().BeApproximately(0.75, 0.1);
        }

        [Test]
        public void PutNextRectangle_AverageSameSizeRectanglesCloudDensity_Between70And90Percents()
        {
            var avgDensity = Generators.CenterGenerator()
                .Select(center => 
                    GetCloudDensity(center, 
                        Enumerable.Repeat(Generators.RectanglesRandomSizeGenerator().Take(1).Single(), 100)))
                .Average();
            
            avgDensity.Should().BeApproximately(0.8, 0.1);
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