using System.Drawing;
using System.Security.Cryptography.X509Certificates;
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
            layouter = new CircularCloudLayouter(center, spr);
        }

        [Test]
        public void CloudInitialization_ThrowsNullReferenceException_OnNullSpiral()
        {
            Assert.Throws<NullReferenceException>(() => new CircularCloudLayouter(center, null));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(100)]
        public void PutNextRectangle_ReturnPutRectangles_CorrectCount(int expectedCount)
        {
            for (int i = 0; i < expectedCount; i++)
                addedRectangles.Add(layouter.PutNextRectangle(new Size(400, 400)));

            addedRectangles.Count().Should().Be(expectedCount);
        }

        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        public void PutNextRectangle_ThrowsArgumentException_IncorrectArguments(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
        }
    }
}

