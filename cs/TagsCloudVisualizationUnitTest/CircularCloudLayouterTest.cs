using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationUnitTest
{
    public class TagsCloudVisualizationTest
    {
        [TestCase(-10, -10, TestName = "Negative size")]
        [TestCase(0, 0, TestName = "Size is Empty")]
        [TestCase(10, 0, TestName = "Weight = zero")]
        public void PutNextRectangleTest_ShouldBeThrowWhen(int h, int w)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));

            Action act = () => circularCloudLayouter.PutNextRectangle(new Size(h, w));

            act.Should().Throw<Exception>();
        }

        [TestCase(10, 10, TestName = "Usual param")]
        public void PutNextRectangleTest_ShouldNotThrowWhen(int h, int w)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));

            Action act = () => circularCloudLayouter.PutNextRectangle(new Size(h, w));

            act.Should().NotThrow();
        }

        [TestCase(33, 33, 10, 10, ExpectedResult = false, TestName = "ShouldBeFalse")]
        [TestCase(5, 5, 10, 10, ExpectedResult = true, TestName = "ShouldBeTrue")]
        public bool IsIntersectsRectanglesTest(int x, int y, int h, int w)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));

            circularCloudLayouter.PutNextRectangle(new Size(10, 10));

            return circularCloudLayouter.IsIntersects(new Rectangle(x, y, w, h));

        [Test]
        public void PutNextRectangle_RectanglesShouldBeHave_UniqueCoordinates()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));

            for (int i = 0; i < 1000; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            }

            circularCloudLayouter.Rectangles.Should().OnlyHaveUniqueItems();
        }
    }
}