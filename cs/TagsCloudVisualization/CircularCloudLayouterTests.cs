using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [Test]
        public void CircularCloudLayouter_Initialize_Params()
        {
            var center = new Point(1, 2);
            var tagsCloud = new CircularCloudLayouter(center);

            Assert.AreEqual(0, tagsCloud.Radius);
            Assert.AreEqual(0, tagsCloud.Angle);
            Assert.AreEqual(0, tagsCloud.WordPositions.Count);
            Assert.AreEqual(center, tagsCloud.Center);
        }

        [TestCaseSource(nameof(PutNextRectangleIncorrectArguments))]
        public void PutNextRectangle_With_Incorrect_Arguments(Size rectangleSize, CircularCloudLayouter tagsCloud)
        {
            Assert.Throws<ArgumentException>(() => tagsCloud.PutNextRectangle(rectangleSize));
        }

        [Test]
        public void PutNextRectangleFirstRectanglePositionEqualCenter()
        {
            var tagsCloud = new CircularCloudLayouter(new Point(4, 2));
            tagsCloud.PutNextRectangle(new Size(3, 1));
            Assert.AreEqual(tagsCloud.Center, tagsCloud.WordPositions[0].Location);
        }

        [TestCaseSource(nameof(CheckIntersectionCaseData))]
        public bool CheckIntersectionTest(Size size1, Rectangle rectangle)
        {
            var tagsCloud = new CircularCloudLayouter(new Point(0, 0));
            tagsCloud.PutNextRectangle(size1);
            return tagsCloud.CheckIntersection(rectangle);
        }

        [TestCaseSource(nameof(RectangleCompressionCaseData))]
        public Point RectangleCompression(CircularCloudLayouter cloud, Rectangle rectangle)
        {
            return cloud.RectangleCompression(rectangle).Location;
        }

        private static IEnumerable<TestCaseData> PutNextRectangleIncorrectArguments()
        {
            var tagsCloud = new CircularCloudLayouter(new Point());

            yield return new TestCaseData(new Size(-1, 1), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Width_Is_Negative");

            yield return new TestCaseData(new Size(1, -1), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Height_Is_Negative");

            yield return new TestCaseData(new Size(0, 1), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Width_Is_Zero");

            yield return new TestCaseData(new Size(1, 0), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Height_Is_Zero");
        }

        private static IEnumerable<TestCaseData> CheckIntersectionCaseData()
        {
            yield return new TestCaseData(new Size(4, 2), new Rectangle(new Point(1, 1), new Size(2, 2)))
                .SetName(
                    "CheckIntersection_Return_True_When_Rectangle_Intersection_With_Any_Rectangle_In_CircularCloudLayouter")
                .Returns(true);
            yield return new TestCaseData(new Size(1, 1), new Rectangle(new Point(1, 0), new Size(1, 1)))
                .SetName(
                    "CheckIntersection_Return_False_When_Rectangle_Have_Common_Side_With_Another_Rectangle")
                .Returns(false);
            yield return new TestCaseData(new Size(4, 2), new Rectangle(new Point(4, 4), new Size(2, 2)))
                .SetName(
                    "CheckIntersection_Return_True_When_Rectangle_Intersection_With_Any_Rectangle_In_CircularCloudLayouter")
                .Returns(false);
        }

        private static IEnumerable<TestCaseData> RectangleCompressionCaseData()
        {
            var cloudEmpty = new CircularCloudLayouter(new Point());
            var cloudWithElements = new CircularCloudLayouter(new Point());
            cloudWithElements.PutNextRectangle(new Size(1, 1));
            var rectangle = new Rectangle(new Point(5, 5), new Size(1, 1));

            yield return new TestCaseData(cloudEmpty, rectangle)
                .SetName("RectangleCompression_When_Cloud_Is_Empty_Set_Rectangle_Position_On_Center")
                .Returns(cloudEmpty.Center);
            yield return new TestCaseData(cloudWithElements, rectangle)
                .SetName("RectangleCompression_When_Cloud_Has_Rectangles_Set_Rectangle_Position_Closer_To_Center")
                .Returns(new Point(0, 1));
        }
    }
}