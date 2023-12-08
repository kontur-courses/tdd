using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests.UnitTests
{
    class CircularCloudLayouter_Should
    {
        [TestCaseSource(typeof(TestDataCircularCloudLayouter),
            nameof(TestDataCircularCloudLayouter.ZeroOrLessHeightOrWidth_Size))]
        public void Throw_WhenPutNewRectangle_WidthOrHeightLessEqualsZero(Size size)
        {
            var action = new Action(() => new CircularCloudLayouter(new Point()).PutNextRectangle(size));
            action.Should().Throw<ArgumentException>()
                .Which.Message.Should().Contain("zero or negative height or width");
        }

        [Test]
        public void RectanglesEmpty_AfterCreation()
        {
            var layouter = new CircularCloudLayouter(new Point());
            layouter.Rectangles.Should().BeEmpty();
        }

        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void Add_FirstRectangle_ToCenter(Point center)
        {
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(new Size(10, 2));
            layouter.Rectangles.Should().HaveCount(1)
                .And.BeEquivalentTo(new Rectangle(
                    new Point(center.X - 10 / 2, center.Y - 2 / 2), new Size(10, 2)));
        }

        [Test]
        public void AddSeveralRectangles_Correctly()
        {
            var amount = 25;
            var layouter = CreateValidator_WithSeveralRectangles(amount);
            layouter.Rectangles.Should().HaveCount(amount);
        }

        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void AddSeveralRectangles_DoNotIntersect(Point point)
        {
            var layouter = CreateValidator_WithSeveralRectangles(25);

            var rectangles = layouter.Rectangles;
            for (var i = 1; i < rectangles.Count; i++)
                rectangles.Skip(i).All(x => !rectangles[i - 1].IntersectsWith(x)).Should().Be(true);
        }

        [Test]
        public void DensityTest()
        {
            var layouter = new CircularCloudLayouter(new Point());
            for (var i = 0; i < 2000; i++)
                layouter.PutNextRectangle(new Size(35, 15));
            var rectanglesSquare = 0;
            var maxdX = 0;
            var maxdY = 0;
            foreach (var rectangle in layouter.Rectangles)
            {
                rectanglesSquare += rectangle.Width * rectangle.Height;
                maxdX = Math.Max(maxdX, Math.Abs(rectangle.X) + rectangle.Width / 2 - layouter.CenterPoint.X);
                maxdY = Math.Max(maxdY, Math.Abs(rectangle.Y) + rectangle.Height / 2 - layouter.CenterPoint.Y);
            }

            var radius = Math.Max(maxdX, maxdY);
            var circleSquare = Math.PI * radius * radius;

            (rectanglesSquare / circleSquare).Should().BeGreaterOrEqualTo(0.9);
        }

        private static CircularCloudLayouter CreateValidator_WithSeveralRectangles(int amount)
        {
            var layouter = new CircularCloudLayouter(new Point());
            for (var i = 1; i < amount + 1; i++)
            {
                layouter.PutNextRectangle(new Size(i * 20, i * 10));
            }

            return layouter;
        }
    }
}
