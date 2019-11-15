using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloud;

namespace TagCloud_Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private static CircularCloudLayouter cloudLayouter;
        [SetUp]
        public void Setup()
        {
            var center = new Point(0, 0);
            cloudLayouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void SizeOfCloud_ReturnsSameSize_AtPositiveSize()
        {
            var size = new Size(10, 5);

            cloudLayouter.PutNextRectangle(size);
            var currentSize = cloudLayouter.SizeOfCloud;

            currentSize.Should().Be(size);
        }

        [TestCase(10, 10)]
        [TestCase(50, 50)]
        public void LayouterRectangles_NotIntersect_WhileAdding(int widthMax, int heightMax)
        {
            var sizes = new List<Size>();
            for (var width = 5; width < widthMax; width += 2)
            {
                for (var height = 5; height < heightMax; height += 2)
                {
                    sizes.Add(new Size(width, height));
                }
            }
            var rectangles = cloudLayouter.Rectangles;

            sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));
            var isAnyIntersects = rectangles.Any(r1 => rectangles.Any(r2 => (r1 != r2 && r1.IntersectsWith(r2))));

            isAnyIntersects.Should().BeFalse();
        }

        [Test]
        public void Layouter_ContainsTheSameSizes_AtAddingRectanglesList()
        {

            var sizes = new List<Size>();
            for (var width = 5; width < 50; width += 2)
            for (var height = 5; height < 50; height += 2)
                sizes.Add(new Size(width, height));

            sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));
            var rectangles = cloudLayouter.Rectangles.Select(rect => new Size(rect.Width, rect.Height));

            rectangles.Should().BeEquivalentTo(sizes);
        }

        [TestCase(5, 5)]
        [TestCase(20, 20)]
        public void PutNextRectangle_ReturnSameSizeRectangle_AtPositiveSizes(int width, int height)
        {
            var size = new Size(width, height);

            var rectangleSize = cloudLayouter.PutNextRectangle(size).Size;

            rectangleSize.Should().Be(size);
        }

        private static IEnumerable<Size> TestCases
        {
            get
            {
                yield return new Size(0, 10);
                yield return new Size(10, 0);
                yield return Size.Empty;
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PutNextRectangle_ThrowsException_OnW(Size size)
        {
            Action rectAdding = () => cloudLayouter.PutNextRectangle(size);

            rectAdding.Should().Throw<ArgumentException>();
        }


        [TearDown]
        public void TearDown()
        {

            var result = TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed) ? "Failed" : "Successful";
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            var testFullName = TestContext.CurrentContext.Test.Name;
            var savePath = projectDirectory + "\\Images\\" + result + "Tests\\" + testFullName + ".bmp";
            Console.WriteLine("Tag cloud visualization saved to file " + savePath);
            TagDrawer.Draw(savePath, cloudLayouter);
        }


    }
}
