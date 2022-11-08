using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private static readonly Size CanvasSize = new(2000, 2000);
        private static readonly Size MinRectangle = new(20, 15);
        private static readonly Size MaxRectangle = new(200, 100);

        private RectangleGenerator generator;
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            generator = new RectangleGenerator(MinRectangle, MaxRectangle);
            layouter = new CircularCloudLayouter(CanvasSize);
            rectangles = new List<Rectangle>();
        }

        [Test, Category("DrawImageWhenFail")]
        public void PutNextRectangle_RectanglesShouldBeCompact_For100Rectangles()
        {
            rectangles.Add(layouter.PutNextRectangle(generator.GetRandomRectangle()));
            using (new AssertionScope())
            {
                for (var i = 0; i < 100; i++)
                {
                    var rectangle = layouter.PutNextRectangle(generator.GetRandomRectangle());

                    var closerRectangle = new Rectangle(rectangle.Location, rectangle.Size);
                    closerRectangle.Y += Math.Sign(layouter.Center.Y - rectangle.Center().Y);
                    closerRectangle.X += Math.Sign(layouter.Center.X - rectangle.Center().X);

                    var result = closerRectangle.IntersectsWith(rectangles);
                    result.Should().BeTrue($"Iteration: {i}, rectangles should intersect: {closerRectangle.Location} can be placed");
                    rectangles.Add(rectangle);
                }
            }
        }

        [Test, Category("DrawImageWhenFail")]
        public void PutNextRectangle_ShouldNotIntersect_For100Rectangles()
        {
            using (new AssertionScope())
            {
                for (var i = 0; i < 100; i++)
                {
                    var rectangle = layouter.PutNextRectangle(generator.GetRandomRectangle());

                    var result = rectangle.IntersectsWith(rectangles);
                    result.Should().BeFalse($"Iteration: {i}, rectangles should not intersect. {rectangle.Location} wrong");

                    rectangles.Add(rectangle);
                }
            }
        }

        [Test]
        public void PutNextRectangle_ShouldThrowException_WhenRectangleSizeBiggerThanCanvas()
        {
            Action action = () =>
            {
                var layouter = new CircularCloudLayouter(new Size(2, 2));
                layouter.PutNextRectangle(new Size(10000, 1000));
            };
            action.Should().Throw<Exception>();
        }


        [TestCase(0, 0)]
        [TestCase(-7, 1)]
        public void PutNextRectangle_ShouldThrowArgumentException_WhenRectangleSizeNotPositive(int width, int height)
        {
            Action action = () =>
            {
                var layouter = new CircularCloudLayouter(new Size(100, 100));
                layouter.PutNextRectangle(new Size(width, height));
            };
            action.Should().Throw<ArgumentException>();
        }

        [TearDown]
        public void TearDown()
        {
            if (!TestContext.CurrentContext.Test.Properties["Category"].Contains("DrawImageWhenFail") ||
                TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;

            var path = "../../../errors/" + TestContext.CurrentContext.Test.MethodName + ".png";
            Painter.DrawToFile(new Size(1500, 1500), rectangles, path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}
