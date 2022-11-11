using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private Point center;
        private Spiral spr;
        private List<Rectangle> addedRectangles;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            center = new Point(400, 400);
            spr = new Spiral(center);
            addedRectangles = new List<Rectangle>();
            layouter = new CircularCloudLayouter(spr);
        }

        [Test]
        public void CloudInitialization_ThrowsNullReferenceException_OnNullSpiral()
        {
            var action = () => new CircularCloudLayouter(null);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Spiral should not be null");
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(100)]
        public void PutNextRectangle_ReturnPutRectangles_CorrectCount(int expectedCount)
        {
            var size = new Size(100, 100);

            for (int i = 0; i < expectedCount; i++)
            {
                var rectangle = layouter.PutNextRectangle(size);
                addedRectangles.Add(rectangle);
            }

            addedRectangles.Count().Should().Be(expectedCount);
        }

        [TestCase(1, -1, TestName = "Negative height, correct width")]
        [TestCase(-1, 1, TestName = "Negative width, correct height")]
        [TestCase(1, 0, TestName = "Zero height, correct width")]
        [TestCase(0, 1, TestName = "Zero width, correct height")]
        public void PutNextRectangle_ThrowsArgumentException_IncorrectArguments(int width, int height)
        {
            var action = () => layouter.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentException>()
                .WithMessage("Sides of the rectangle should not be non-positive");
        }
    }
}