using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Test_CircularCloudLayouter
    {
        private CircularCloudLayouter cloudLayouter;
        private Random random;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(new Point(450, 450));
            random = new Random();
        }

        [Test]
        public void PutNextRectangle_OnEmptySize_ShouldThrowArgExcept()
        {
            Action action = () => cloudLayouter.PutNextRectangle(Size.Empty);

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -1)]
        public void PutNextRectangle_OnNegativeSize_ShouldThrowArgExcept(int width, int height)
        {
            Action action = () => cloudLayouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(2, 1)]
        [TestCase(2, 6)]
        [TestCase(3, 3)]
        [TestCase(150, 3)]
        public void PutNextRectangle_ShouldReturnSomeRectangleWithRightSize(int width, int height)
        {
            cloudLayouter.PutNextRectangle(new Size(width, height)).Size.Should().Be(new Size(width, height));
        }

        [Test]
        public void PutNextRectangle_TwoRectangleShouldNotIntersect()
        {
            var rectangle1 = cloudLayouter.PutNextRectangle(new Size(12, 12));
            var rectangle2 = cloudLayouter.PutNextRectangle(new Size(7, 7));

            rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        public void PutNextRectangle_AnyTwoRectanglesNotHaveIntersect(int count)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < count; i++)
                rectangles.Add(cloudLayouter.PutNextRectangle(new Size(random.Next(100), random.Next(100))));

            for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        [TestCase("smallPicture.png", 20, 400)]
        [TestCase("middlePicture.png", 40, 500)]
        [TestCase("largePicture.png", 60, 700)]
        public void VisualizationSomeImage(string name, int sizeOfRectangle, int size)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(size, size));
            for (var i = 0; i < 1000; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(1, sizeOfRectangle), random.Next(1, sizeOfRectangle)));

            var bitMap = new Bitmap(size * 2, size * 2);
            bitMap = cloudLayouter.Visualization(bitMap);
            bitMap.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name));// Как сохранть правильно? Или такой подход сойдёт? 
        }
    }
}