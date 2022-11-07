using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Tests
    {
        private CircularCloudLayouter layout;

        [SetUp]
        public void Setup()
        {
            layout = new CircularCloudLayouter(new Point(0, 0));
            layout.FillFreeRectangles = false;
        }

        [Test]
        public void PlacesOneRectangle_InCenter()
        {
            layout.PutNextRectangle(new Size(10, 5))
                .Should().Be(new Rectangle(-5, -2, 10, 5));
        }

        [Test]
        public void PlacesOneRectangle_WithNotZeroPointCenter()
        {
            layout = new CircularCloudLayouter(new Point(-2, -2));

            layout.PutNextRectangle(new Size(10, 5))
                .Should().Be(new Rectangle(-7, -4, 10, 5));
        }

        [Test]
        public void PlacesSmallSecondRectangle_AlignedWithUpperLine()
        {
            layout.PutNextRectangle(new Size(5, 2));
            layout.PutNextRectangle(new Size(3, 1))
                .Should().Be(new Rectangle(3, -1, 3, 1));
        }

        [Test]
        public void PlacesSmallThirdRectangle_AlignedWithRightLine()
        {
            layout.PutNextRectangle(new Size(5, 2));
            layout.PutNextRectangle(new Size(3, 1));
            layout.PutNextRectangle(new Size(6, 3))
                .Should().Be(new Rectangle(0, 1, 6, 3));
        }

        [Test]
        public void PlacesSmallFourthRectangle_AlignedWithBottomLine()
        {
            layout.PutNextRectangle(new Size(5, 2));
            layout.PutNextRectangle(new Size(3, 1));
            layout.PutNextRectangle(new Size(6, 3));
            layout.PutNextRectangle(new Size(8, 4))
                .Should().Be(new Rectangle(-10, 0, 8, 4));
        }

        [Test]
        public void PlacesSmallFifthRectangle_AlignedWithLeftLine()
        {
            layout.PutNextRectangle(new Size(5, 2));
            layout.PutNextRectangle(new Size(3, 1));
            layout.PutNextRectangle(new Size(6, 3));
            layout.PutNextRectangle(new Size(8, 4));
            layout.PutNextRectangle(new Size(7, 3))
                .Should().Be(new Rectangle(-10, -4, 7, 3));
        }

        [Test]
        public void PlacesManySmallRectangles_AlignedRespectively()
        {
            layout.PutNextRectangle(new Size(5, 2));
            layout.PutNextRectangle(new Size(3, 1));
            layout.PutNextRectangle(new Size(6, 3));
            layout.PutNextRectangle(new Size(8, 4));
            layout.PutNextRectangle(new Size(7, 3));

            layout.PutNextRectangle(new Size(1, 6))
                .Should().Be(new Rectangle(6, -4, 1, 6));

            layout.PutNextRectangle(new Size(4, 3))
                .Should().Be(new Rectangle(3, 4, 4, 3));

            layout.PutNextRectangle(new Size(4, 10))
                .Should().Be(new Rectangle(-14, -3, 4, 10));

            layout.PutNextRectangle(new Size(7, 3))
                .Should().Be(new Rectangle(-14, -7, 7, 3));
        }

        [Test]
        public void PlacesManyBigRectangles_AlignedRespectively()
        {
            layout.PutNextRectangle(new Size(2, 2));

            layout.PutNextRectangle(new Size(1, 3))
                .Should().Be(new Rectangle(1, -1, 1, 3));

            layout.PutNextRectangle(new Size(4, 1))
                .Should().Be(new Rectangle(-2, 2, 4, 1));

            layout.PutNextRectangle(new Size(1, 5))
                .Should().Be(new Rectangle(-3, -2, 1, 5));

            layout.PutNextRectangle(new Size(6, 1))
                .Should().Be(new Rectangle(-3, -3, 6, 1));
        }
    }

    [TestFixture]
    public class TestsWithFreeRectangles
    {
        private CircularCloudLayouter layout;

        [SetUp]
        public void Setup()
        {
            layout = new CircularCloudLayouter(new Point(0, 0));
            layout.FillFreeRectangles = true;
        }

        [Test]
        public void FillsFreeRectangle_Created_BySmallRectangle()
        {
            layout.PutNextRectangle(new Size(2, 2));
            layout.PutNextRectangle(new Size(1, 1));
            layout.PutNextRectangle(new Size(3, 1));
            layout.PutNextRectangle(new Size(1, 1))
                .Should().Be(new Rectangle(1, 0, 1, 1));
        }

        [Test]
        public void DoesNotHave_FreeRectangle_AfterFillingIt()
        {
            layout.PutNextRectangle(new Size(2, 2));
            layout.PutNextRectangle(new Size(1, 1));
            layout.PutNextRectangle(new Size(1, 1));
            layout.PutNextRectangle(new Size(1, 1))
                .Should().Be(new Rectangle(1, 1, 1, 1));
        }

        [Test]
        public void UltimateTest()
        {
            layout.PutNextRectangle(new Size(6, 2));
            layout.PutNextRectangle(new Size(2, 4));
            layout.PutNextRectangle(new Size(3, 2));
            layout.PutNextRectangle(new Size(6, 1));
            layout.PutNextRectangle(new Size(3, 2));
            layout.PutNextRectangle(new Size(2, 2));
            layout.PutNextRectangle(new Size(8, 2));

            layout.PutNextRectangle(new Size(2, 4));
            layout.PutNextRectangle(new Size(2, 1));
            layout.PutNextRectangle(new Size(3, 2));
            layout.PutNextRectangle(new Size(2, 3));
            layout.PutNextRectangle(new Size(14, 1));
            layout.PutNextRectangle(new Size(1, 7));
            layout.PutNextRectangle(new Size(1, 5));
            layout.PutNextRectangle(new Size(16, 1));
            layout.PutNextRectangle(new Size(1, 8))
                .Should().Be(new Rectangle(7, -3, 1, 8));
        }

    }
}