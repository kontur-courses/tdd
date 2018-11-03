using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        [TestCase(0, 0, 0, 0, TestName = "CenterOfCloudAreEquivalentToLeftUpperBoundOfFirstRectangleOnZeroSize")]
        [TestCase(0, 0, 2, 2, TestName = "CenterOfCloudAreEquivalentToLeftUpperBoundOfFirstRectangleOnSquareSize")]
        [TestCase(0, 0, 2, 4, TestName = "CenterOfCloudAreEquivalentToLeftUpperBoundOfFirstRectangleOnRectangleSize")]
        public void FirstRectangleAreInCenterOfTheCloud(int centerX, int centerY, int width, int height)
        {
            var center = new Point(centerX, centerY);
            var cloud = new CircularCloudLayouter(center);

            var firstRectangle = cloud.PutNextRectangle(new Size(width, height));

            var rectangleLocation = firstRectangle.Location;
            var rectangleCenter = new Point(rectangleLocation.X + width / 2, rectangleLocation.Y + height / 2);

            rectangleCenter.ShouldBeEquivalentTo(center);
        }

        [Test]
        public void NextRectangleShouldByFartherOfCenter()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);

            var rectanglesSizes = new [] { new Size(2, 2), new Size(2, 2) };

            var lastDistance = 0.0;

            foreach (var rectangleSize in rectanglesSizes)
            {
                var rectangle = cloud.PutNextRectangle(rectangleSize);
                var distance = GetDistanceFromRectangleToPoint(rectangle, center);

                distance.Should().BeGreaterOrEqualTo(lastDistance);

                lastDistance = distance;
            }
        }

        [Test]
        public void RectanglesAreNotIntersectingAfterAdditionNew()
        {
            var rectanglesSizes = new [] { new Size(100, 100), new Size(100, 100) };
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = rectanglesSizes
                .Select(
                    rectangleSize => cloud.PutNextRectangle(rectangleSize))
                .ToList();

            foreach (var firstRectangleToCheck in rectangles)
                foreach (var secondRectangleToCheck in rectangles)
                {
                    if (firstRectangleToCheck == secondRectangleToCheck)
                        continue;
                    firstRectangleToCheck
                        .IntersectsWith(secondRectangleToCheck)
                        .Should().BeFalse();
                }
        }


        private double GetDistanceFromRectangleToPoint(Rectangle rectangle, Point point)
        {
            return Math.Sqrt((GetCenterOfRectangle(rectangle).X - point.X) *
                   (GetCenterOfRectangle(rectangle).X - point.X) +
                   (GetCenterOfRectangle(rectangle).Y - point.Y) *
                   (GetCenterOfRectangle(rectangle).Y - point.Y));
        }

        private Point GetCenterOfRectangle(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        //private IEnumerable TestCasesGeneratorFor_NextRectangleMustByFartherFromCenter()
        //{
        //    yield return new TestCaseData(new[] { new Size(2, 2), new Size(2, 2) })
        //        .SetName("AfterAdditionTwoEqualsRectangles");
        //}

        //[Test, TestCaseSource(nameof(TestCasesGeneratorFor_NextRectangleMustByFartherFromCenter))]
        //public void NextRectangleMustByFartherFromCenter(IEnumerable<Size> rectanglesSizes)
        //{
        //    var cloud = new CircularCloudLayouter(new Point(0, 0));
        //    var lastDistance = 0.0;

        //    foreach (var rectangleSize in rectanglesSizes)
        //    {
        //        var rectangle = cloud.PutNextRectangle(rectangleSize);
        //        var distance = GetDistanceFromRectangleToPoint(rectangle, cloud);
        //        distance.Should().BeGreaterOrEqualTo(lastDistance);

        //        lastDistance = distance;
        //    }
        //}
    }
}
