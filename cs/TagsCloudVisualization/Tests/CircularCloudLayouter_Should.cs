using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void PutNextRectangle_ShouldPutRectangleInCenter_WhenOneRectangle()
        {
            var center = new Point(0, 0);
            var size = new Size(2, 1);
            var layouter = new CircularCloudLayouter(center);

            var rectangle = layouter.PutNextRectangle(size);

            rectangle.Should()
                .Match<Rectangle>(r =>
                    r.Top == 0 && r.Left == 0
                    || r.Top == 0 && r.Right == 0
                    || r.Bottom == 0 && r.Left == 0
                    || r.Bottom == 0 && r.Right == 0);

        }

        [Test]
        public void PutNextRectangle_RectanglesShouldNotIntersect_WhenTwoRectangles()
        {
            var center = new Point(0, 0);
            var firstSize = new Size(2, 1);
            var secondSize = new Size(3, 4);
            var layouter = new CircularCloudLayouter(center);

            var firstRectangle = layouter.PutNextRectangle(firstSize);
            var secondRectangle = layouter.PutNextRectangle(secondSize);

            GeometryUtils.RectanglesAreIntersected(firstRectangle, secondRectangle)
                .Should()
                .BeFalse();
        }

        [TestCase(2, TestName = "2 rectangles")]
        [TestCase(3, TestName = "3 rectangles")]
        [TestCase(5, TestName = "5 rectangles")]
        [TestCase(10, TestName = "10 rectangles")]
        [TestCase(50, TestName = "50 rectangles")]
        public void PutNextRectangle_RectanglesShouldNotIntersect_WhenManyRectangles(int rectanglesCount)
        {
            var center = new Point(0, 0);
            var size = new Size(2, 1);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = Enumerable.Range(0, rectanglesCount)
                .Select(n => layouter.PutNextRectangle(size)).ToList();

            rectangles
                .SelectMany((x, i) => rectangles.Skip(i + 1), GeometryUtils.RectanglesAreIntersected)
                .Should()
                .AllBeEquivalentTo(false);
        }
    }
}