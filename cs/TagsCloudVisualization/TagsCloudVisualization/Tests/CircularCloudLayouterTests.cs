using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [TestCase(1, 1, 0, 1, "image width should be positive",
            TestName = "FallOn_NotPositiveImageWidth")]
        [TestCase(1, 1, 1, -1, "image height should be positive",
            TestName = "FallOn_NotPositiveImageHeight")]
        [TestCase(-2, -3, 10, 10, "both center coordinates should be non-negative",
            TestName = "FallOn_NegativeCoordinates")]
        [TestCase(10, 10, 2, 4, "center coordinates are not inside the image",
            TestName = "FallOn_CenterCoordinatesOutsideImage")]
        public void ConstructorIncorrectInput(int centerX, int centerY, int imageWidth, int imageHight, string msg)
        {
            Action act = () => new CircularCloudLayouter(new Point(centerX, centerY), new Size(imageWidth, imageHight));

            act.Should().Throw<ArgumentException>()
                .WithMessage(msg);
        }

        [Test]
        public void PutNextRectangle(Size rectangleSize)
        {
            
        }
    }


}
