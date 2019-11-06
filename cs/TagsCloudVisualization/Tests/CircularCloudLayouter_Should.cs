using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter defaultLayouter;

        [SetUp]
        public void SetUp()
        {
            defaultLayouter = new CircularCloudLayouter(Point.Empty, new Size(1000, 1000));
        }


        [TestCaseSource(nameof(PutNextRectangleShouldReturnInCenterTestCases))]
        public Point PutNextRectangle_ShouldReturnInCenter_WhenPutOnlyOne(Point center, Size size)
        {
            var layouter = new CircularCloudLayouter(center, new Size(1000, 1000));

            var rectangle = layouter.PutNextRectangle(size);

            return rectangle.Location;
        }

        public static IEnumerable PutNextRectangleShouldReturnInCenterTestCases
        {
            get
            {
                yield return new TestCaseData(Point.Empty, new Size(2, 2)).Returns(new Point(-1, 1))
                    .SetName("when center is default");
                yield return new TestCaseData(new Point(1, 1), new Size(1, 1)).Returns(new Point(1, 1))
                    .SetName("when center is shifted");
            }
        }

        [TestCaseSource(nameof(PutNextRectangleShouldThrowTestCases))]
        public void PutNextRectangle_ShouldThrow_OnIncorrectSize(Size size)
        {
            Action put = () => defaultLayouter.PutNextRectangle(size);

            put.ShouldThrowExactly<ArgumentException>();
        }

        public static IEnumerable PutNextRectangleShouldThrowTestCases
        {
            get
            {
                yield return new TestCaseData(Size.Empty).SetName("empty size");
                yield return new TestCaseData(new Size(-1, -1)).SetName("negative width and height");
                yield return new TestCaseData(new Size(-1, 0)).SetName("negative width and zero height");
            }
        }

        [TestCaseSource(nameof(PutNextRectangleShouldReturnDisjointTestCases))]
        public void PutNextRectangle_ShouldReturnDisjoint(Size[] sizes)
        {
            var rectangles = PutMultipleRectangles(sizes).ToArray();

            foreach (var rectangle in rectangles)
            {
                rectangles.Count(r => r == rectangle).Should().Be(1, $"rectangles should be unique");
                rectangles.Where(r => r != rectangle).Any(r => r.IntersectsWith(rectangle)).Should()
                    .BeFalse("rectangles should not intersect");
            }
        }

        public static IEnumerable PutNextRectangleShouldReturnDisjointTestCases
        {
            get
            {
                yield return new TestCaseData(new[] { new Size(1, 1), new Size(1, 1) }).SetName("when put two rectangles");
                yield return new TestCaseData(new[] { new Size(100, 100), new Size(500, 500), new Size(125, 125) })
                    .SetName("when put big rectangles");
                yield return new TestCaseData(Enumerable.Repeat(new Size(1, 1), 200).ToArray())
                    .SetName("when put many unit squares");
                yield return new TestCaseData(Enumerable.Range(1, 10).CartesianProduct(Enumerable.Range(1, 10))
                    .Select(p => new Size(p.Item1, p.Item2)).ToArray()).SetName(
                    "when put many different rectangles");
            }
        }

        [TestCaseSource(nameof(PutNextRectangleShouldThrowWhenRectangleCannotBePlacedTestCases))]
        public void PutNextRectangle_ShouldThrow_WhenRectangleCannotBePlaced(Size[] sizes)
        {
            Action put = () => PutMultipleRectangles(sizes).ToArray();

            put.ShouldThrowExactly<ArgumentException>();
        }

        public static IEnumerable PutNextRectangleShouldThrowWhenRectangleCannotBePlacedTestCases
        {
            get
            {
                yield return new TestCaseData(new[] {new Size(2000, 2000)}).SetName("one big rectangle");
                yield return new TestCaseData(new[]
                        {new Size(500, 500), new Size(600, 600), new Size(500, 500), new Size(500, 500),})
                    .SetName("some big rectangles");
                yield return new TestCaseData(Enumerable.Repeat(50, 600).Select(x => new Size(x, x)).ToArray()).SetName(
                    "many little rectangles");
            }
        }

        private IEnumerable<Rectangle> PutMultipleRectangles(params Size[] sizes)
        {
            return sizes.Select(size => defaultLayouter.PutNextRectangle(size));
        }
    }
}