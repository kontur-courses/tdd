using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions.Extensions;
using TagsCloudVisualization;

namespace TagsCloudVisualisationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private readonly PointF layouterCenter = new PointF(400, 400);

        [SetUp]
        public void InitialiseLayouter()
        {
            layouter = new CircularCloudLayouter(layouterCenter);
        }

        [Test]
        public void LayouterShould_BeEmpty_AfterCreation()
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
        public void LayouterShould_ThrowException_WhenPutRectangle_WithNotPositiveSize
            (int width, int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() =>
                layouter.PutNextRectangle(size));
        }

        [Test]
        public void LayouterShould_AddOneRectangleToList()
        {
            layouter.PutNextRectangle(new Size(5, 5));
            layouter.Rectangles.Should()
                .HaveCount(1);
        }

        [Test]
        public void LayouterShould_AddFirstRectangleAtCenter()
        {
            layouter.PutNextRectangle(new Size(5, 5));
            layouter.Rectangles.First()
                .GetCenter()
                .Should()
                .Be(layouterCenter);
        }

        [TestCase(5)]
        [TestCase(20)]
        public void LayouterShould_AddSeveralRectanglesToList(int count)
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
        public void LayouterShould_AddSeveralRectangles_WithoutIntersections(int count)
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
        public void LayouterShould_PutManyRectangles_FastEnough()
        {
            Action action = () => PutSeveralRectangles(1000);
            GC.Collect();
            action.ExecutionTime().Should().BeLessThan(1.Seconds());
        }

        [Test]
        public void LayouterShould_PlaceRectangles_CompactEnough()
        {
            PutSeveralRectangles(100);
            var boundingRect = RectangleFExtensions.GetRectangleByCenter
                (new Size(1000, 1000), layouterCenter);
            layouter.Rectangles.Where(r => !boundingRect.Contains(r))
                .Should()
                .BeEmpty();
        }

        [Test]
        public void LayouterShould_PlaceRectangles_CloseToCircularForm()
        {
            PutSeveralRectangles(400, 18);
            var center = this.layouterCenter.ToVector();
            var radius = 250;
            layouter.Rectangles.ToList()
                .ForEach(r =>
                center.GetDistanceTo(r.GetCenter())
                .Should().BeLessThan(radius));
        }

        [TearDown]
        public void SaveLayout()
        {
            if (TestContext.CurrentContext.Result.FailCount == 0
            || layouter.Rectangles.Count == 0)
                return;

            var name = TestContext.CurrentContext.Test.Name;
            var path = $"..\\..\\..\\{name}.jpg";
            var message = $"Test {name} down!\n" +
                $"Tag cloud visualization saved to file {path}";

            layouter.Visualise(path);
            TestContext.WriteLine(message);
        }

        /// <summary>
        /// Я решил сделать визуализацию через тесты, чтобы не выделять отдельный проект
        /// и можно было удобно настраивать параметры
        /// </summary>
        //[TestCase("..\\..\\..\\1.jpg", 200, 10, 20)]
        //[TestCase("..\\..\\..\\2.jpg", 250, 15, 25)]
        //[TestCase("..\\..\\..\\3.jpg", 500, 10, 16)]
        //[TestCase("..\\..\\..\\4.jpg", 250, 5, 50)]
        public void Visualise(string filename, int count, int min, int max)
        {
            PutSeveralRectangles(count, min, max);
            layouter.Visualise(filename);
        }

        private List<Size> GetRandomSizes(int count, int min, int max, int seed = 0)
        {
            var rnd = new Random(seed);
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
