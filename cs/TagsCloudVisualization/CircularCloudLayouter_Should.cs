using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void CreatesClassInstance_WithoutException()
        {
            var createCloudLayouter = () => new CircularCloudLayouter(new Point(0, 0));
            createCloudLayouter.Should().NotThrow();
        }

        [TestCase(-1, 0, TestName = "Negative width")]
        [TestCase(0, -1, TestName = "Negative height")]
        [TestCase(-5, -5, TestName = "Negative width and height")]
        public void CreatesRectangle_ThrowsArgumentException_WhenNegativeParameters(int rectWidth, int rectHeight)
        {   
            var cloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var rectSize = new Size(rectWidth, rectHeight);
            var rectangleCreation = () => cloudLayouter.PutNextRectangle(rectSize);
            rectangleCreation.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 0, 100, 100)]
        [TestCase(0, 0, 1, 1)]
        [TestCase(0, 0, 5, 4)]
        [TestCase(5, 4, 2, 1)]
        [TestCase(5, 4, 3, 6)]
        public void CreatesFirstRectangle_InTheCenter(int centerX, int centerY, int rectWidth, int rectHeight)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangle = cloudLayouter.PutNextRectangle(new Size(rectWidth, rectHeight));
            var halfWidth = (int)Math.Floor(rectWidth / 2.0);
            var halfHeight = (int)Math.Floor(rectHeight / 2.0);

            var expectedRectLeft = cloudLayouter.CenterPoint.X - halfWidth;
            var expectedRectRight = cloudLayouter.CenterPoint.X + halfWidth + (rectWidth % 2);
            var expectedRectTop = cloudLayouter.CenterPoint.Y - halfHeight;
            var expectedRectBottom = cloudLayouter.CenterPoint.Y + halfHeight + (rectHeight % 2);

            rectangle.Left.Should().Be(expectedRectLeft);
            rectangle.Right.Should().Be(expectedRectRight);
            rectangle.Top.Should().Be(expectedRectTop);
            rectangle.Bottom.Should().Be(expectedRectBottom);
        }
    }
}