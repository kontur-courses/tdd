using System.Drawing;
using TagsCloudVisualization.Models;
using FluentAssertions;
namespace CircularCloudLayouterTests
{
    [TestFixture]
    public class CircularCloudLayouterUnitTests
    {

        [Test]
        public void PutNextRectangle_ReturnsRightRectangle_AtCenter()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(100, 50);

            var rectangle = layouter.PutNextRectangle(size);

            var expectedRectangle = new Rectangle(-50, -25,
                                                  size.Width, size.Height);

            rectangle.Should()
                .BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_DoNotIntersect_WhenFirstRectangleIsAlreadyPlaced()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var firstRectangleSize = new Size(100, 50);
            var secondRectangleSize = new Size(200, 100);

            var firstRectangle = layouter.PutNextRectangle(firstRectangleSize);
            var secondRectangle = layouter.PutNextRectangle(secondRectangleSize);

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsCorrectRectangle_WhenUsingNotStandardCenterForLayouter()
        {
            var center = new Point(100, 100);
            var layouter = new CircularCloudLayouter(center);
            var rectangleSize = new Size(100, 50);

            var rectangle = layouter.PutNextRectangle(rectangleSize);

            var expectedRectangle = new Rectangle(50, 75,
                                            rectangleSize.Width, rectangleSize.Height);

            rectangle.Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_ReturnsNonIntersectingRectangles_WhenManyRectanglesPlaced()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var sizes = new[]
            {
                new Size(100, 50),
                new Size(60, 30),
                new Size(80, 40),
                new Size(40, 20),
                new Size(120, 60)
            };

            var rectangles = sizes
                            .Select(s => layouter.PutNextRectangle(s))
                            .ToList();

            for (var i = 0; i < rectangles.Count; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleWithRequestedSize_WhenDifferentSizesAreRequested()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var sizes = new[]
            {
                new Size(100, 50),
                new Size(60, 30),
                new Size(80, 40)
            };

            var rectangles = sizes
                            .Select(size => layouter.PutNextRectangle(size))
                            .ToList();

            for (var i = 0; i < sizes.Length; i++)
            {
                rectangles[i].Size.Should().Be(sizes[i]);
            }
        }
    }
}
