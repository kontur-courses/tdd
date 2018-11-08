using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests
{
    [TestFixture]
    public class CloudVisualizer_Should
    {
        [TestCase(1, TestName = "For 1 rectangle")]
        [TestCase(100, TestName = "For 100 rectangles")]
        public void CreatePictureWithRectangles(int amountOfRectangles)
        {
            var visualizer = new CloudVisualizer { Settings = DrawSettings.OnlyRectangles };
            var rectangles = new Rectangle[amountOfRectangles];
            for (var i = 0; i < amountOfRectangles; i++)
                rectangles[i] = new Rectangle(0, 0, 10, 10);

            var picture = visualizer.CreatePictureWithRectangles(rectangles);

            IsPictureContainsAllLocationPoints(rectangles, picture)
                .Should().BeTrue();
        }

        public bool IsPictureContainsAllLocationPoints(Rectangle[] rectangles, Bitmap picture)
        {
            return rectangles.All(rectangle =>
                picture.GetPixel(rectangle.Location.X, rectangle.Location.Y).ToArgb() == Color.Black.ToArgb());
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