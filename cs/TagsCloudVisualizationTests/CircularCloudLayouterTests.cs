using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization;
using TagsCloudVisualization.Infrastructure;

namespace TagsCloudVisualizationTests
{
    
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter sut;
        private List<Rectangle> rectangles;
        private Point anchor;
        [SetUp]
        [Timeout(1000)]
        public void Setup()
        {
            anchor = new Point();
            sut = new CircularCloudLayouter(anchor);
            
            var rectangleSizes = new[]
            {
                new Size(100, 1),
                new Size(1,100),
                new Size(30,20),
                new Size(1,1),
            };
            
            rectangles = rectangleSizes
                .Select(rectangleSize => sut.PutNextRectangle(rectangleSize))
                .ToList();
        }

        [Test]
        public void Constructor_Exists()
        {
            var center = new Point();
            
            var _ = new CircularCloudLayouter(center);
        }
        
        [Test]
        public void PutNextRectangle_Exists()
        {
            var center = new Point();
            var rectangleSize = new Size();
            var layouter = new CircularCloudLayouter(center);
            
            Rectangle _ = layouter.PutNextRectangle(rectangleSize);
        }

        [Test]
        public void Cloud_IsCenteredByPoint()
        {
            var expectedX = rectangles.Max(rectangle => rectangle.Width);
            var expectedY = rectangles.Max(rectangle => rectangle.Height);

            var centers = rectangles
                .Select(rectangle => rectangle.GetCenter())
                .ToList();
            var centersTotal = centers.Aggregate(
                Point.Empty, 
                (total, rectangleCenter) => total.Add(rectangleCenter));
            var averageCenter = new Point(centersTotal.X / centers.Count, centersTotal.Y / centers.Count);
            var deviation = new Point
            (
                Math.Abs(averageCenter.X - anchor.X),
                Math.Abs(averageCenter.Y - anchor.Y)
            );
            
            Assert.That(deviation.X, Is.LessThan(expectedX));
            Assert.That(deviation.Y, Is.LessThan(expectedY));
        }

        [Test]
        [Timeout(500)]
        public void Cloud_RectanglesDoNotIntersect()
        {
            foreach (var rectangle1 in rectangles)
            foreach (var rectangle2 in rectangles
                .Where(rectangle2 => rectangle1 != rectangle2))
                Assert.False(rectangle1.IntersectsWith(rectangle2), 
                    $"{rectangle1}X{rectangle2}");
        }

        [Test]
        public void Cloud_RectanglesAreArrangedCompactly()
        {
            var expected = Math.Max(
                rectangles.Max(rectangle => rectangle.Width),
                rectangles.Max(rectangle => rectangle.Height)
            );
            
            var distanceSum = 0d;
            foreach (var rectangle1 in rectangles)
            foreach (var rectangle2 in rectangles
                .Where(rectangle2 => rectangle1 != rectangle2))
            {
                var center1 = rectangle1.GetCenter();
                var center2 = rectangle2.GetCenter();
                distanceSum += center1.DistanceFrom(center2);
            }
            var distanceAverage = distanceSum / rectangles.Count;
            
            Assert.That(distanceAverage, Is.LessThan(expected));
        }
    }
}