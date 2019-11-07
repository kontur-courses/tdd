using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloud_Tests
{

    public class CircularCloudLayouter_Tests
    {
        public static CircularCloudLayouter cloudLayouter;
        [SetUp]
        public void Setup()
        {
            var center = new Point(0, 0);
            cloudLayouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void SizeOfCloud_ReturnsSameSizeAsParameterAfterOneRectangle()
        {
            var size = new Size(10, 5);

            cloudLayouter.PutNextRectangle(size);
            var currentSize = cloudLayouter.SizeOfCloud;

            currentSize
                .Should()
                .Be(size);
        }


        [TestFixture]
        public class RectanglesAfterPutting
        {
            [SetUp]
            public void Setup()
            {
                var center = new Point(0, 0);
                cloudLayouter = new CircularCloudLayouter(center);
            }
            public static IEnumerable TestCases
            {
                get
                {
                    var testName = "ThrowsException_When";
                    yield return new TestCaseData(new Size(0, 10)).SetName(testName + "ZeroWidth");
                    yield return new TestCaseData(new Size(10, 0)).SetName(testName + "ZeroHeight");
                    yield return new TestCaseData(Size.Empty).SetName(testName + "EmptySize");
                }
            }
            //Тест находит после того как запустил остальные, приходится запускать в ручную
            [TestCase(5, 5)]
            [TestCase(20, 20)]
            public void Has_SameSizeAsParameter(int width, int height)
            {
                var size = new Size(width, height);

                var rectangleSize = cloudLayouter.PutNextRectangle(size).Size;

                rectangleSize
                    .Should()
                    .Be(size);
            }

            [Test]
            public void Contains_TheSameSizesAfterFewRectangles()
            {

                var sizes = new List<Size>();
                for (var width = 5; width < 50; width += 2)
                    for (var height = 5; height < 50; height += 2)
                        sizes.Add(new Size(width, height));

                sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));
                var rectangles = cloudLayouter.Rectangles.Select(rect => new Size(rect.Width, rect.Height));

                rectangles
                    .Should()
                    .BeEquivalentTo(sizes);
            }

            [TestCase(10, 10)]
            [TestCase(50, 50)]
            public void DoNot_IntersectEachOther(int widthMax, int heightMax)
            {
                var sizes = new List<Size>();
                for (var width = 5; width < widthMax; width += 2)
                    for (var height = 5; height < heightMax; height += 2)
                        sizes.Add(new Size(width, height));
                var rectangles = cloudLayouter.Rectangles;

                sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));
                var isAnyIntersects = rectangles.Any(r1 => rectangles.Any(r2 => (r1 != r2 && r1.IntersectsWith(r2))));

                isAnyIntersects.Should().BeFalse();
            }

            //С тест кейс соурс не отображается название основного теста, не нашел как исправить
            [TestCaseSource(nameof(TestCases))]
            public void PutRectangle_ThrowsException(Size size)
            {
                Action rectAdding = () => cloudLayouter.PutNextRectangle(size);

                rectAdding
                    .Should()
                    .Throw<ArgumentException>();
            }
        }
    }
}
