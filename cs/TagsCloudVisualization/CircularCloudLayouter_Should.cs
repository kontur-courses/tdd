using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                var imagePath = RectDrawer.DrawRectangles(circularCloudLayouter.Rectangles.ToArray());
                if (imagePath != null)
                    TestContext.Out.WriteLine("Result layout has been saved to " + Path.GetFullPath(imagePath));
            }
        }

        [Test]
        public void Create()
        {
        }

        [Test]
        public void NotThrow_WhenPutsRectangles()
        {
            Assert.DoesNotThrow(() => circularCloudLayouter.PutNextRectangle(Size.Empty));
        }

        [Test]
        public void HasOneRectangleInCenter_WhenPutsOne()
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(2, 2));

            rectangle.Should().BeEquivalentTo(new Rectangle(-1, -1, 2, 2));
        }

        [Test]
        public void RectanglesDoNotIntersect_WhenPutsTwo()
        {
            var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(5, 5));
            var rectangle2 = circularCloudLayouter.PutNextRectangle(new Size(2, 2));

            rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
        }
    }
}
