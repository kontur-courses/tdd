using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using FluentAssertions.Extensions;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private Cloud cloud;
        private readonly PointF layouterCenter = new PointF(400, 400);

        [SetUp]
        public void InitialiseLayouter()
        {
            layouter = new CircularCloudLayouter(layouterCenter);
            cloud = layouter.Cloud;
        }

        [Test]
        public void LayouterShould_BeEmpty_AfterCreation()
        {
            cloud.Rectangles.Should()
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
            cloud.Rectangles.Should()
                .HaveCount(1);
        }

        [Test]
        public void LayouterShould_AddFirstRectangleAtCenter()
        {
            layouter.PutNextRectangle(new Size(5, 5));
            cloud.Rectangles.First()
                .GetCenter()
                .Should()
                .Be(layouterCenter);
        }

        [TestCase(5)]
        [TestCase(20)]
        public void LayouterShould_AddSeveralRectanglesToList(int count)
        {
            var sizes = PutSeveralRectangles(count);
            var rectSizes = cloud.Rectangles
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
            var rectangles = cloud.Rectangles;
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
            cloud.Rectangles.Where(r => !boundingRect.Contains(r))
                .Should()
                .BeEmpty();
        }

        [TestCase(5, 20)]
        [TestCase(20, 50)]
        [TestCase(10, 200)]
        public void LayouterShould_PlaceRectangles_CloseToCircularForm(int min, int max)
        {
            var count = 400;
            PutSeveralRectangles(count, min, max);
            var vectorLayouterCenter = layouterCenter.ToVector();
            var radius = count * (max + min) / 2 * 0.05;
            cloud.Rectangles.ToList()
                .ForEach(r =>
                vectorLayouterCenter.GetDistanceTo(r.GetCenter())
                .Should().BeLessThan(radius));
        }

        [TearDown]
        public void SaveLayout()
        {
            if (TestContext.CurrentContext.Result.FailCount == 0
            || cloud.Rectangles.Count == 0)
                return;

            var name = TestContext.CurrentContext.Test.Name;
            var path = Path.GetFullPath($"..\\..\\..\\images\\{name}.jpg");
            var message = $"Test {name} down!\n" +
                $"Tag cloud visualization saved to file {path}";

            cloud.DefaultVisualize(path);
            TestContext.WriteLine(message);
        }

        /// <summary>
        /// Я решил сделать визуализацию через тесты, чтобы не выделять отдельный проект
        /// и можно было удобно настраивать параметры
        /// </summary>
        //[TestCase("..\\..\\..\\images\\1.jpg", 200, 10, 20)]
        //[TestCase("..\\..\\..\\images\\2.jpg", 250, 15, 25)]
        //[TestCase("..\\..\\..\\images\\3.jpg", 500, 10, 16)]
        //[TestCase("..\\..\\..\\images\\4.jpg", 250, 5, 50)]
        public void Visualize(string filename, int count, int min, int max)
        {
            PutSeveralRectangles(count, min, max);
            cloud.DefaultVisualize(filename);
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