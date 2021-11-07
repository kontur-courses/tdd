using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions.Extensions;
using System.Numerics;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private CircularCloudLayouter layouter;
        private PointF center = new PointF(400, 400);

        [SetUp]
        public void InitialiseLayouter()
        {
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void BeEmptyAfterCreation()
        {
            layouter.Rectangles.Should()
                .BeEmpty();
        }

        [TestCase(5, 0)]
        [TestCase(0, 5)]
        [TestCase(0, 0)]
        [TestCase(-5, 5)]
        [TestCase(5, -5)]
        [TestCase(-5, -5)]
        public void ThrowException_WhenPutRectangle_WithNotPositiveSize
            (int width, int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() =>
                layouter.PutNextRectangle(size));
        }

        [Test]
        public void PutRectangleToList()
        {
            layouter.PutNextRectangle(new Size(5, 5));
            layouter.Rectangles.Should()
                .HaveCount(1);
        }

        [Test]
        public void AddFirstRectangleAtCenter()
        {
            layouter.PutNextRectangle(new Size(5, 5));
            layouter.Rectangles.First()
                .GetCenter()
                .Should()
                .Be(center);
        }

        [TestCase(5)]
        [TestCase(20)]
        public void AddSeveralRectangles(int count)
        {
            var sizes = PutSeveralRectangles(count);
            var rectSizes = layouter.Rectangles
                .Select(r => r.Size)
                .ToList();
            rectSizes.Should().HaveCount(sizes.Count);
            for (var i = 0; i < rectSizes.Count; i++)
                rectSizes[i].Should().Be((SizeF)sizes[i]);
        }

        [TestCase(5)]
        [TestCase(20)]
        public void AddSeveralRectangles_WithoutIntersections(int count)
        {
            PutSeveralRectangles(count);
            var rectangles = layouter.Rectangles;
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    rectangles[i].IntersectsWith(rectangles[j])
                        .Should()
                        .BeFalse();
        }

        [Test]
        public void WorkFastEnough()
        {
            var layouter2 = new CircularCloudLayouter(new PointF(100, 100));
            Action action = () => PutSeveralRectangles(500);
            GC.Collect();
            layouter2.PutNextRectangle(new Size(1, 1));
            action.ExecutionTime().Should().BeLessThan(3.Seconds());
        }

        [Test]
        public void PlaceRectanglesCompactEnough()
        {
            PutSeveralRectangles(500);
            var boundingRect = RectangleFExtensions.GetRectangleByCenter
                (new Size(2000, 2000), center);
            layouter.Rectangles.Where(r => !boundingRect.Contains(r))
                .Should()
                .BeEmpty();
        }

        [Test]
        public void PlaceRectanglesCloseToCircularForm()
        {
            PutSeveralRectangles(250, 18, 20);
            var center = this.center.ToVector();
            var radius = 400;
            layouter.Rectangles.ForEach(r =>
                center.GetDistanseTo(r.GetCenter())
                .Should().BeLessThan(radius));
        }

        private List<Size> GetRandomSizes(int count, int min, int max)
        {
            var rnd = new Random();
            var result = new List<Size>();
            for (var i = 0; i < count; i++)
                result.Add(new Size(rnd.Next(min, max), rnd.Next(min, max)));
            return result;
        }

        private List<Size> PutSeveralRectangles
            (int count, int min = 1, int max = 20)
        {
            var sizes = GetRandomSizes(count, min, max);
            foreach (var sz in sizes)
                layouter.PutNextRectangle(sz);
            return sizes;
        }
    }
}
