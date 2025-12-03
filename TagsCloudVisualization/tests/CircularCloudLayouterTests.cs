using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;


namespace TagCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            center = new Point(500, 500);
            layouter = new CircularCloudLayouter(center, new Spiral(center));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                VisualizationPrinter.Print(TestContext.CurrentContext.Test.Name, layouter.Rectangles, center);
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleOfRequestedSize()
        {
            var size = new Size(30, 20);
            var rect = layouter.PutNextRectangle(size);
            rect.Size.Should().Be(size);
        }

        [Test]
        public void PutNextRectangle_IsCentered_WhenPutsFirstRectagle()
        {
            var size = new Size(50, 30);
            var rect = layouter.PutNextRectangle(size);
            var rectCenter = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            rectCenter.Should().Be(center);
        }

        [Test]
        public void PutManyRectangles_DontIntersect()
        {
            var rnd = new Random(0);
            var sizes = Enumerable.Range(0, 100).Select(i => new Size(rnd.Next(20, 60), rnd.Next(10, 40))).ToArray();
            foreach (var s in sizes)
                layouter.PutNextRectangle(s);
            var rects = layouter.Rectangles;
            for (int i = 0; i < rects.Count; i++)
                for (int j = i + 1; j < rects.Count; j++)
                    rects[i].IntersectsWith(rects[j]).Should().BeFalse();
        }

        [Test]
        public void PutManyRectangles_CloudIsApproximatelyCircular()
        {
            var rnd = new Random(1);
            var sizes = Enumerable.Range(0, 150).Select(i => new Size(rnd.Next(10, 50), rnd.Next(8, 40))).ToArray();
            foreach (var s in sizes)
                layouter.PutNextRectangle(s);
            var rects = layouter.Rectangles;
            var left = rects.Min(r => r.Left);
            var right = rects.Max(r => r.Right);
            var top = rects.Min(r => r.Top);
            var bottom = rects.Max(r => r.Bottom);
            var width = right - left;
            var height = bottom - top;
            var ratio = width / (double)height;
            ratio.Should().BeInRange(0.6, 1.7);
        }
    }
}
