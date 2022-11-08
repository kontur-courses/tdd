using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudShould
    {
        private static IEnumerable<TestCaseData> SourceLists
        {
            get
            {
                yield return new TestCaseData(new Size[]
                {
                    new Size(100, 100),
                    new Size(50, 100),
                    new Size(50, 50),
                    new Size(20, 30)
                });

                yield return new TestCaseData(new Size[]
                {
                    new Size(100, 100),
                    new Size(200, 200),
                    new Size(100, 50),
                    new Size(5, 5)
                });

                yield return new TestCaseData(new Size[]
                {
                    new Size(100, 100),
                    new Size(100, 100),
                    new Size(100, 100),
                    new Size(100, 100),
                    new Size(100, 100),
                    new Size(100, 100),
                    new Size(100, 100),
                });
            }
        }

        [Test]
        public void CircularCloudLayouter_CreateNewLayouter_ShouldInitComposerAndSpiral()
        {
            var center = Point.Empty;

            var layouter = new CircularCloudLayouter(center);

            layouter.Composer.Should().NotBeNull();
            layouter.Composer.Spiral.Should().NotBeNull();
        }

        [Test]
        public void PutNextRectangle_AddSingleRectInCenter_RectLocationInCenter()
        {
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);
            var rectSize = new Size(100, 100);

            var rect = layouter.PutNextRectangle(rectSize);
            var rectCenter = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            rectCenter.Should().Be(center);
        }

        [TestCaseSource(nameof(SourceLists))]
        public void PutNextRectangle_AddManyRect_RectsShouldBeAdded(Size[] addRects)
        {
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);

            var expectedList = new List<Rectangle>();

            foreach (var size in addRects)
            {
                var rect = layouter.PutNextRectangle(size);
                expectedList.Add(new Rectangle(rect.Location, size));
            }

            layouter.Composer.Rectangles.Count.Should().Be(addRects.Length);
        }

        [TestCaseSource(nameof(SourceLists))]
        public void PutNextRectangle_AddManyRect_RectsShouldNotIntersect(Size[] addRects)
        {
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);

            var expectedList = new List<Rectangle>();

            foreach (var size in addRects)
            {
                var rect = layouter.PutNextRectangle(size);
                expectedList.Add(new Rectangle(rect.Location, size));
            }

            IsRectanglesIntersect(layouter.Composer.Rectangles).Should().BeFalse();
        }

        [TestCase(0, 0)]
        [TestCase(100, 0)]
        [TestCase(0, 100)]
        [TestCase(0, -100)]
        public void PutNextRectangle_AddWrongSize_RectNotAdded(int width, int height)
        {
            var sizeToAdd = new Size(width, height);
            var center = Point.Empty;
            var layouter = new CircularCloudLayouter(center);

            var nextRect = layouter.PutNextRectangle(sizeToAdd);

            layouter.Composer.Rectangles.Count.Should().Be(0);
            nextRect.Should().Be(Rectangle.Empty);
        }

        public bool IsRectanglesIntersect(List<Rectangle> rects)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                for (int j = i + 1; j < rects.Count; j++)
                {
                    var a = rects[i];
                    var b = rects[j];

                    if (a.IntersectsWith(b))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}