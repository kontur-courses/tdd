using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class CircularCloudLayouter_Specification
    {
        private CircularCloudLayouter layouter;
        private readonly Size size = new Size(20, 20);

        [SetUp]
        public void SetUp()
        {
            var point = new Point(0, 0);
            layouter = new CircularCloudLayouter(point);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
                return;
            var imagePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                TestContext.CurrentContext.Test.Name + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-fff}" + ".bmp");
            new CloudVisualizer(layouter).CreateImage(imagePath);
            TestContext.Out.WriteLine("Tag cloud visualization saved to file " + imagePath);
        }

        [Test]
        public void Constructor_CreatesLayouterWithCenter()
        {
            new CircularCloudLayouter(new Point(0, 0)).Center.Should().Be(new Point(0, 0));
        }

        [TestCase(0, 1, TestName = "has zero width")]
        [TestCase(1, 0, TestName = "has zero height")]
        [TestCase(-1, 1, TestName = "has negative width")]
        [TestCase(1, -1, TestName = "has negative height")]
        public void PutNextRectangle_ThrowsExceptionWhenSize(int width, int height)
        {
            Action putRectangle = () => layouter.PutNextRectangle(new Size(width, height));

            putRectangle.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ReturnsCorrectRectangle()
        {
            layouter.PutNextRectangle(size)
                .Should().Be(new Rectangle(layouter.Center, size));
        }

        [Test]
        public void PutNextRectangle_PlacesRectangleIntoCollection()
        {
            layouter.PutNextRectangle(size);
            layouter.Rectangles.Should().Contain(new Rectangle(layouter.Center, size));
        }

        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect()
        {
            PopulateWithRandomRectangles(layouter);

            new CloudVisualizer(layouter).CreateImage(@"D:\image.bmp");

            var rectanglesChecked = 1;
            foreach (var rectangle in layouter.Rectangles)
            {
                foreach (var otherRectangle in layouter.Rectangles.Skip(rectanglesChecked++))
                {
                    RectanglesIntersect(rectangle, otherRectangle).Should().BeFalse();
                }
            }
        }

        [Test]
        public void PutNextRectangle_RectanglesHaveDenseDistribution()
        {
            PopulateWithRandomRectangles(layouter);
            var rectanglesArea = layouter.Rectangles.Sum(r => r.Width * r.Height);
            var radius = Math.Max(layouter.Width, layouter.Height) / 2;
            var circleArea = Math.PI * Math.Pow(radius, 2);
            var density = rectanglesArea / circleArea;

            density.Should().BeGreaterOrEqualTo(0.6);
        }

        [Test]
        public void PutNextRectangle_CloudHasCircularShape()
        {
            PopulateWithRandomRectangles(layouter);
            var radius = Math.Max(layouter.Width, layouter.Height) / 2;
            var circleArea = Math.PI * Math.Pow(radius, 2);
            var rectangleArea = layouter.Width * layouter.Height;

            circleArea.Should().BeLessThan(rectangleArea);
        }

        private static void PopulateWithRandomRectangles(ICloudLayouter layouter)
        {
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(
                    random.Next(50, 201),
                    random.Next(20, 61)));
            }
        }

        private static bool RectanglesIntersect(Rectangle r1, Rectangle r2)
        {
            return r1.X < r2.X + r2.Width &&
                   r2.X < r1.X + r1.Width &&
                   r1.Y < r2.Y + r2.Height &&
                   r2.Y < r1.Y + r1.Height;
        }
    }
}
