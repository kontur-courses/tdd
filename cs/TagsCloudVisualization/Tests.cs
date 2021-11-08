using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Tests
    {
        private static Point[] centralPoints =
        {
            new Point(0, 0),
            new Point(1, 5),
            new Point(-1, 5),
            new Point(1, -5),
            new Point(-1, -5)
        };

        private static Size[] sizes =
        {
            new Size(1, 1),
            new Size(2, 2),
            new Size(5, 5),
            new Size(2, 4),
            new Size(3, 1)
        };

        [TestCaseSource(nameof(centralPoints))]
        public void Constructor_should_initialize_without_exceptions(Point centre)
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(centre),
                "Location can to have positive and negative coordinates");
        }

        [TestCaseSource(nameof(centralPoints))]
        public void PutNextRectangle_should_put_first_rectangle_on_centre(Point center)
        {
            var cloud = new CircularCloudLayouter(center);

            var size = new Size(4, 4);
            var x = center.X - 2;
            var y = center.Y + 2;
            var expectedLocation = new Point(x, y);

            var actual = cloud.PutNextRectangle(size);

            actual.Location.Should().Be(expectedLocation);
        }

        [Test]
        public void AfterPutNextRectangle_should_not_any_intersecting_rectangles()
        {
            var cloud = new CircularCloudLayouter();

            cloud.PutNextRectangles(sizes);

            var intersectingRectanglePairs =
                GetIntersectingRectanglePairs(cloud.GetRectangles())
                    .ToArray();

            intersectingRectanglePairs.Should()
                .BeEmpty($"Rectangles should not intersect\n" +
                         $"{GetStringOfIntersectingRectangles(intersectingRectanglePairs)}");
        }

        private IEnumerable<(Rectangle rect1, Rectangle rect2)> GetIntersectingRectanglePairs
            (IEnumerable<Rectangle> rectangles)
        {
            var i = 0;

            var rectanglePairs = rectangles
                .SelectMany(r1 =>
                {
                    i++;
                    return rectangles
                        .Skip(i)
                        .Select(r2 => (r1, r2))
                        .Where(pair => pair.r1.IntersectsWith(pair.r2));
                });

            return rectanglePairs;
        }

        private string GetStringOfIntersectingRectangles(IEnumerable<(Rectangle, Rectangle)> rectanglePairs)
        {
            return string.Join("\n", rectanglePairs
                .Where(pair => pair.Item1.IntersectsWith(pair.Item2))
                .Select(pair => $"'{pair.Item1}' intersects with '{pair.Item2}'"));
        }
    }
}