using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void Create()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new CircularCloudLayouter(Point.Empty);
        }

        [Test]
        public void NotThrow_WhenPutsRectangles()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(Point.Empty).PutNextRectangle(Size.Empty));
        }

        [Test]
        public void HasOneRectangleInCenter_WhenPutsOne()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);

            var rectangle = layouter.PutNextRectangle(new Size(2, 2));

            rectangle.Should().BeEquivalentTo(new Rectangle(-1, -1, 2, 2));
        }

        [Test]
        public void RectanglesDoNotIntersect_WhenPutsTwo()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);

            var rectangle1 = layouter.PutNextRectangle(new Size(5, 5));
            var rectangle2 = layouter.PutNextRectangle(new Size(2, 2));

            rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
        }
    }
}
