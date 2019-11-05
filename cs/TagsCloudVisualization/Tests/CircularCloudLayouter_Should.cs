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
            defaultLayouter = new CircularCloudLayouter(Point.Empty);
        }


        [Test]
        public void PutNextRectangle_ShouldReturnInCenter_WhenPutOnlyOne()
        {
            var rectangle = defaultLayouter.PutNextRectangle(new Size(2, 2));

            rectangle.Location.ShouldBeEquivalentTo(new Point(-1, 1));
        }

        [Test]
        public void PutNextRectangle_ShouldReturnInCenter_WhenCenterIsShifted()
        {
            var layouter = new CircularCloudLayouter(new Point(1, 1));

            var rectangle = layouter.PutNextRectangle(new Size(1, 1));

            rectangle.Location.ShouldBeEquivalentTo(new Point(1, 1));
        }

        [TestCaseSource(nameof(PutNextRectangleShouldThrowTestCases))]
        public void PutNextRectangle_ShouldThrow_OnIncorrectSize(Size size)
        {
            Action put = () => defaultLayouter.PutNextRectangle(size);

            put.ShouldThrowExactly<ArgumentException>($"size {size} is incorrect");
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
                yield return new TestCaseData(new[] { new Size(1000, 1000), new Size(500, 500), new Size(1250, 1250) })
                    .SetName("when put big rectangles");
                yield return new TestCaseData(Enumerable.Repeat(new Size(1, 1), 200).ToArray())
                    .SetName("when put many unit squares");
                yield return new TestCaseData(Enumerable.Range(1, 10).CartesianProduct(Enumerable.Range(1, 10))
                    .Select(p => new Size(p.Item1, p.Item2)).ToArray()).SetName(
                    "when put many different rectangles");
            }
        }

        private IEnumerable<Rectangle> PutMultipleRectangles(params Size[] sizes)
        {
            return sizes.Select(size => defaultLayouter.PutNextRectangle(size));
        }
    }
}