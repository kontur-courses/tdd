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
        public void NotThrow_WhenAddsRectangles()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(Point.Empty).PutNextRectangle(Size.Empty));
        }

        [Test]
        public void HasOneRectangleInCenter_WhenAddedOne()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);

            var rectangle = layouter.PutNextRectangle(new Size(2, 2));

            rectangle.Should().BeEquivalentTo(new Rectangle(-1, -1, 2, 2));
        }
    }
}
