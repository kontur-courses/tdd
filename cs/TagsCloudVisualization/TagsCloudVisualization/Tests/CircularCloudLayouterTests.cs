using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [TestCase(-2, -3,"both center coordinates should be non-negative",
            TestName = "FallOn_NegativeCoordinates")]
        public void ConstructorIncorrectInput(int centerX, int centerY, string msg)
        {
            Action act = () => new CircularCloudLayouter(new Point(centerX, centerY));

            act.Should().Throw<ArgumentException>()
                .WithMessage(msg);
        }

        [TestCase(10, 5)]
        public void PutNextRectangle(int width, int height)
        {
            var layout = new CircularCloudLayouter(new Point(0, 0));
            var rectangle = layout.PutNextRectangle(new Size(width, height));

            rectangle.Location.X.Should().Be(0);
            rectangle.Location.Y.Should().Be(0);
            rectangle.Width.Should().Be(width);
            rectangle.Height.Should().Be(height);
        }

        [Test]
        public void InsertNewPointsCreatedByRectangle()
        {
            
        }
    }


}
