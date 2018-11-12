using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        private static readonly Point Center = Point.Empty;

        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(Center);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure) return;
            var imagePath = RectDrawer.DrawRectangles(Center, circularCloudLayouter.Rectangles.ToArray());
            if (imagePath != null)
                TestContext.Out.WriteLine("Result layout has been saved to " + Path.GetFullPath(imagePath));
        }

        [Test]
        public void HasOneRectangleInCenter_WhenPutsOne()
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(2, 2));

            rectangle.Should().BeEquivalentTo(new Rectangle(-1, -1, 2, 2));
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(100)]
        public void RectanglesDoNotIntersect_WhenPutsSeveral(int rectangleAmount)
        {
            var rectangles = new Rectangle[rectangleAmount];
            var random = new Random();
            for (var i = 0; i < rectangleAmount; i++)
            {
                var randomSize = new Size(random.Next(1, 100), random.Next(1, 100));
                rectangles[i] = circularCloudLayouter.PutNextRectangle(randomSize);
            }

            for (var i = 0; i < rectangleAmount; i++)
            for (var j = i + 1; j < rectangleAmount; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }
    }
}
