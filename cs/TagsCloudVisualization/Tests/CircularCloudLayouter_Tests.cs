using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void Constructor_AnyPoint_ShouldNotChangeParameters(int x, int y)
        {
            var location = new Point(x, y);

            var cloudLayouter = new CircularCloudLayouter(location);

            cloudLayouter.Center.Should().Be(location,
                "Constructor shouldn't change central point");
        }

        [TestCase(0, 0, 2, 2, -1, -1)]
        [TestCase(0, 0, 3, 3, -2, -2)]
        [TestCase(0, 0, 0, 0, 0, 0)]
        [TestCase(3, 4, 5, 7, 0, 0)]
        public void PutNextRectangle_FirstRectangle_ShouldPutOnCentre
            (int cX, int cY, int width, int height, int expX, int expY)
        {
            var center = new Point(cX, cY);
            var cloud = new CircularCloudLayouter(center);
            var size = new Size(width, height);
            var expectedLocation =
                new Point(expX, expY);

            var actualRectangle = cloud.PutNextRectangle(size);

            actualRectangle.Location.Should().Be(expectedLocation,
                "The first rectangle should be in the centre");
        }

        [Test]
        public void PutNextRectangle_NextRectangles_ShouldNotHaveIntersectionsSimpleCase()
        {
            var centre = new Point(0, 0);
            var cloudLayouter = new CircularCloudLayouter(centre);
            var size = new Size(3, 2);
            var expLocation1 = new Point(-2, -3);
            var expLocation2 = new Point(-2, 1);

            PutManiRectangles(cloudLayouter, size, 6);


            cloudLayouter.GetRectangles()[1].Location.Should().Be(expLocation1);
            cloudLayouter.GetRectangles()[2].Location.Should().Be(expLocation2);
            
            Assert.IsFalse(cloudLayouter.GetRectangles()
                .SelectMany(x => cloudLayouter.GetRectangles()
                    .Select(y => (x, y))).Where(x => x.x != x.y)
                .Any(pair=>pair.x.IntersectsWith(pair.y)));
        }

        private void PutManiRectangles(CircularCloudLayouter cloudLayouter, Size size, int count)
        {
            for (; count > 0; count--)
                cloudLayouter.PutNextRectangle(size);
        }
    }
}