using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    [TestFixture]
    public class CloudVisualizer_Should
    {
        [TestCase(1, TestName = "For 1 rectangle")]
        [TestCase(10, TestName = "For 10 rectangles")]
        [TestCase(1000, TestName = "For 1000 rectangles")]
        public void CreateBitmap(int amountOfRectangles)
        {
            var visualizer = new CloudVisualizer();
            var rectangles = new Rectangle[amountOfRectangles];
            for (var i = 0; i < amountOfRectangles; i++)
                rectangles[i] = new Rectangle(0, 0, 10, 10);

            var result = visualizer.CreatePictureWithRectangles(rectangles);

            result.Should().NotBeNull();
        }

        [TestCase(false, TestName = "Then rectangles list is null")]
        [TestCase(true, TestName = "Then rectangles list is empty")]
        public void ThrowArgumentException(bool isArrayInitialized)
        {
            var visualizer = new CloudVisualizer();
            Action creation = ()
                => visualizer.CreatePictureWithRectangles(
                    isArrayInitialized
                        ? new Rectangle[0]
                        : null);

            creation.Should().Throw<ArgumentException>();
        }
    }
}