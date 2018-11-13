using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geom;


namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(100, 100, 1024, 1024);
        }

        [Test]
        public void NotConatinIntersectedRectangles()
        {
            for (var i = 0; i < 100; i++)
            {   
                layouter.PutNextRectangle(new Size().GenerateRandom(3, 30, 3, 20));
            }

            foreach (var r1 in layouter.Rectangles)
                foreach (var r2 in layouter.Rectangles)
                    if (r1 != r2)
                        r1.IntersectsWith(r2).Should().BeFalse();
        }

        [Test]
        public void HaveTheFormOfCirle()
        {
            layouter.PutNextRectangle(new Size(10, 5));
            layouter.PutNextRectangle(new Size(10, 10));
            layouter.PutNextRectangle(new Size(10, 5));
            layouter.PutNextRectangle(new Size(10, 10));
            var expectedRadius = 21;

            var actualMaxRadius = double.MinValue;

            foreach (var rect in layouter.Rectangles)
            {
                var currentMaxRadius = rect
                    .Vertices()
                    .Select(p =>
                        Math.Sqrt(Math.Pow(layouter.Center.X - p.X, 2) +
                                  Math.Pow(layouter.Center.Y - p.Y, 2)))
                    .Max();

                actualMaxRadius = Math.Max(actualMaxRadius, currentMaxRadius);
            }

            actualMaxRadius.Should().BeLessOrEqualTo(expectedRadius);
        }

        [Test]
        public void NotHaveInvalidCloudSize()
        {
            Action action = () => new CircularCloudLayouter(new Point(0, 0), new Size(-100, 12));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NotPutBigSizeRectangles()
        {
            Action action = () => layouter.PutNextRectangle(new Size(1028, 10));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PlaceRectanglesTightly()
        {
            double rectanglesArea = 0;
            for (var i = 0; i < 100; i++)
            {
                var size = new Size().GenerateRandom(3, 70, 3, 50);
                layouter.PutNextRectangle(size);
                rectanglesArea += size.Width * size.Height;
            }

            rectanglesArea.Should().BeLessOrEqualTo(0.7 * layouter.Area);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                var currentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                var imagePath = Path.Combine(currentDirectory, $"{TestContext.CurrentContext.Test.Name}-fail.png");

                new ImageWriter(imagePath).WriteLayout(layouter);
                Console.WriteLine($"Tag cloud visualization saved to file {imagePath}");
            }
        }

    }
}