using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter tagsCloud;
        private SpiralDistribution distribution;
        private CloudLayouterDrawer drawer;

        [SetUp]
        public void SetUp()
        {
            center = new Point();
            distribution = new SpiralDistribution(center);
            tagsCloud = new CircularCloudLayouter(center, distribution);
            drawer = new CloudLayouterDrawer(10);
        }

       [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var fileName =$"{TestContext.CurrentContext.Test.FullName}.png";
                drawer.DrawCloud(fileName, tagsCloud.WordPositions);
                Console.WriteLine($"Tag cloud visualization saved to file /images/{fileName}");
            }
        }
      
        [Test]
        public void CircularCloudLayouter_Initialize_Params()
        {
            Assert.AreEqual(0, tagsCloud.WordPositions.Count);
            Assert.AreEqual(center, tagsCloud.Center);
            Assert.AreEqual(distribution, tagsCloud.Distribution);
        }

        [Test]
        public void CircularCloudLayouter_Initialize_Throws_ArgumentException_When_Distribution_Have_Different_Center()
        {
            var center = new Point(1, 5);
            var centerDistribution = new Point(2, 4);
            var distribution = new SpiralDistribution(centerDistribution);
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(center, distribution));
        }

        [TestCaseSource(nameof(PutNextRectangleIncorrectArguments))]
        public void PutNextRectangle_ThrowsArgumentException_WhenIncorrectArguments(Size rectangleSize,
            CircularCloudLayouter tagsCloud)
        {
            Assert.Throws<ArgumentException>(() => tagsCloud.PutNextRectangle(rectangleSize));
        }

        [Test]
        public void PutNextRectangle_Should_Place_First_On_Center()
        {
            tagsCloud.PutNextRectangle(new Size(3, 1));
            Assert.AreEqual(tagsCloud.Center, tagsCloud.WordPositions[0].Location);
        }

        [TestCaseSource(nameof(CheckIntersectionCaseData))]
        public bool CheckIntersectionTest(Size rectangleSize, Rectangle rectangle)
        {
            tagsCloud.PutNextRectangle(rectangleSize);
            return tagsCloud.CheckIntersection(rectangle);
        }

        [TestCaseSource(nameof(RectangleCompressionCaseData))]
        public Point RectangleCompression(CircularCloudLayouter cloud, Rectangle rectangle)
        {
            return cloud.ComperessRectangle(rectangle).Location;
        }


        private static IEnumerable<TestCaseData> PutNextRectangleIncorrectArguments()
        {
            var center = new Point();
            var distribution = new SpiralDistribution(center);
            var tagsCloud = new CircularCloudLayouter(center, distribution);

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
            var center = new Point();
            var distributionEmpty = new SpiralDistribution(center);
            var cloudEmpty = new CircularCloudLayouter(center, distributionEmpty);
            var distributionWithElements = new SpiralDistribution(center);
            var cloudWithElements = new CircularCloudLayouter(center, distributionWithElements);
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