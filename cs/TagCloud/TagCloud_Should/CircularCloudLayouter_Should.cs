using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagCloud
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [TearDown]
        public void SaveTestsResult()
        {
            var name = TestContext.CurrentContext.Test.Name;
            if (Equals(TestContext.CurrentContext.Result.Outcome, ResultState.Failure))
                new TagCloudVisualization(layouter.Rectangles.Count, layouter, name).MakeTagCloudForUnitTest();
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void CircularCloudLayouter_NonPositiveScreenSize_ThrowsException(int width, int height)
        {
            Action action = () =>
                layouter = new CircularCloudLayouter(new Point(0, 0), new Size(width, height));
            action.ShouldThrow<ArgumentException>();
        }

        [TestCase(-100, -100, 100, 100)]
        [TestCase(-50, 0, 75, 75)]
        [TestCase(0, -50, 75, 75)]
        public void CircularCloudLayouter_OutTheScreenSquare_ThrowsException(int x, int y, int width, int height)
        {
            Action action = () =>
                layouter = new CircularCloudLayouter(new Point(x, y), new Size(width, height));
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void CircularCloudLayouter_RectanglesSet_ShouldNotBeNull()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(600, 600));
            layouter.Rectangles.Should().NotBeNull();
        }

        [Test]
        public void CircularCloudLayouter_RectanglesSet_ShouldBeZeroLengthAtStart()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(600, 600));
            layouter.Rectangles.Count.Should().Be(0);
        }

        [Test]
        public void PutNextRectangle_FirstRect_ShouldBeInCenterWithSomeBias()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(600, 600));
            layouter.PutNextRectangle(new Size(10, 3)).Location.ShouldBeEquivalentTo(new Point(7, 3));
        }

        [Test]
        public void PutNextRectangle_ShouldAddOneRectangle()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(600, 600));
            layouter.PutNextRectangle(new Size(10, 3));
            layouter.Rectangles.Count.Should().Be(1);
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void PutNextRectangle_NonPositiveRectSize_ThrowsException(int width, int height)
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(600, 600));
            Action action = () =>
                layouter
                    .PutNextRectangle(new Size(width, height));
            action.ShouldThrow<ArgumentException>();
        }

        [TestCase(101, 50, 100, 100)]
        [TestCase(100, 101, 200, 100)]
        [TestCase(101, 51, 100, 50)]
        public void PutNextRectangle_SizeBiggerThanScreen_ThrowsException(int width, int height, int screenWidth,
            int screenHeight)
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(screenWidth, screenHeight));
            Action action = () =>
                layouter
                    .PutNextRectangle(new Size(width, height));
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_PositionOutOfScreen_ReturnsFalse()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(100, 100));
            for (var i = 0; i < 50; i++)
                layouter.PutNextRectangle(new Size(10, 10));
            layouter.PutNextRectangle(new Size(10, 10)).Size.ShouldBeEquivalentTo(Size.Empty);
        }

        [Test]
        public void PutNextRectangle_AddingRect_ShouldSetNewCurrentCoords()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), new Size(600, 600));
            layouter.PutNextRectangle(new Size(5, 6));
            layouter.Spiral.GetNewPointLazy().First().ShouldBeEquivalentTo(new Point(3, 6));
        }
    }
}