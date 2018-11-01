using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        [Test]
        public void CenterAreNotChangingAfterInitialization()
        {
            var expectedCenter = new Point(0, 0);

            var actualCenter = new CircularCloudLayouter(expectedCenter).Center;

            actualCenter.ShouldBeEquivalentTo(expectedCenter);
        }

        [TestCase(0, 0, 0, 0, TestName = "CenterOfCloudAreEquivalentToLeftUpperBoundOfFirstRectangleOnZeroSize")]
        [TestCase(0, 0, 2, 2, TestName = "CenterOfCloudAreEquivalentToLeftUpperBoundOfFirstRectangleOnSquareSize")]
        [TestCase(0, 0, 2, 4, TestName = "CenterOfCloudAreEquivalentToLeftUpperBoundOfFirstRectangleOnRectangleSize")]
        public void FirstRectangleAreInCenterOfTheCloud(int centerX, int centerY, int width, int height)
        {
            var cloud = new CircularCloudLayouter(new Point(centerX, centerY));

            var cloudCenter = cloud.Center;
            var firstRectangle = cloud.PutNextRectangle(new Size(width, height));

            var rectangleLocation = firstRectangle.Location;
            var rectangleCenter = new Point(rectangleLocation.X + width / 2, rectangleLocation.Y + height / 2);

            rectangleCenter.ShouldBeEquivalentTo(cloudCenter);
        }

        public double GetDistanceFromRectangleToCloud(Rectangle rectangle, CircularCloudLayouter cloud)
        {
            return Math.Sqrt((GetCenterOfRectangle(rectangle).X - cloud.Center.X) *
                   (GetCenterOfRectangle(rectangle).X - cloud.Center.X) +
                   (GetCenterOfRectangle(rectangle).Y - cloud.Center.Y) *
                   (GetCenterOfRectangle(rectangle).Y - cloud.Center.Y));
        }

        public Point GetCenterOfRectangle(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        private IEnumerable TestCasesGeneratorFor_NextRectangleMustByFartherOfCenter()
        {
            yield return new TestCaseData(new []{new Point(2,2), new Point(2, 2)}).SetName("CenterOfCloudAreEquivalentToLeftUpperBoundOfFirstRectangleOnRectangleSize");
        }

        [Test, TestCaseSource(nameof(TestCasesGeneratorFor_NextRectangleMustByFartherOfCenter))]
        public void NextRectangleMustByFartherOfCenter(IEnumerable<Size> rectanglesSizes)
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var lastDistance = 0.0;

            foreach (var rectangleSize in rectanglesSizes)
            {
                var rectangle = cloud.PutNextRectangle(rectangleSize);
                var distance = GetDistanceFromRectangleToCloud(rectangle, cloud);
                distance.Should().BeGreaterOrEqualTo(lastDistance);

                lastDistance = distance;
            }
            
        }
    }
}
