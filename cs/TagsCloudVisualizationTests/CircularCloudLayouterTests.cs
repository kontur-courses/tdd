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
        private Point anchor;
        [SetUp]
        [Timeout(1000)]
        public void Setup()
        {
            anchor = new Point();
            sut = new CircularCloudLayouter(anchor);
        }

        private IEnumerable<Rectangle> PlaceRectangles(CircularCloudLayouter layouter, IEnumerable<Size> rectangleSizes)
        {
            return rectangleSizes.Select(layouter.PutNextRectangle);
        }
        
        public static IEnumerable<TestCaseData> DataLayouts()
        {
            (string dataName, Size[] sizes)[] testData = {
                ("Long thin tiles", new[]
                {
                    new Size(100, 1),
                    new Size(1, 100),
                }),
                ("Small tiles", new[]
                {
                    new Size(1, 1),
                    new Size(1, 1),
                }),
                ("Quadratic tiles", new []
                {
                    new Size(5, 5),
                    new Size(1,1),
                    new Size(2,2),
                    new Size(3,3),
                    new Size(4,4),
                })
                
            };

            return testData.Select(test
                => new TestCaseData(test.sizes) {TestName = test.dataName});
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

        [TestCaseSource(nameof(DataLayouts))]
        public void Cloud_IsCenteredByPoint(IEnumerable<Size> rectangleSizes)
        {
            var rectangles = PlaceRectangles(sut, rectangleSizes).ToList();
            
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
            Assert.That(deviation.X, Is.LessThan(rectangles.Max(rectangle => rectangle.Width)));
            Assert.That(deviation.Y, Is.LessThan(rectangles.Max(rectangle => rectangle.Height)));
        }

        [TestCaseSource(nameof(DataLayouts))]
        public void Cloud_RectanglesDoNotIntersect(IEnumerable<Size> rectangleSizes)
        {
            var rectangles = PlaceRectangles(sut, rectangleSizes).ToList();
            
            foreach (var rectangle1 in rectangles)
            foreach (var rectangle2 in rectangles
                .Where(rectangle2 => rectangle1 != rectangle2))
                Assert.False(rectangle1.IntersectsWith(rectangle2), 
                    $"{rectangle1}X{rectangle2}");
        }
        
        [TestCaseSource(nameof(DataLayouts))]
        public void Cloud_RectanglesAreArrangedCompactly(IEnumerable<Size> rectangleSizes)
        {
            var expected = Math.Max(
                rectangleSizes.Max(rectangle => rectangle.Width),
                rectangleSizes.Max(rectangle => rectangle.Height)
            );
            
            var rectangles = PlaceRectangles(sut, rectangleSizes).ToList();
            
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
            Assert.That(distanceAverage, Is.LessThanOrEqualTo(expected));
        }
    }
}