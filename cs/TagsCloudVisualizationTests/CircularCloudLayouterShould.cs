using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
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
            for (int i = 0; i < 100; i++)
            {   
                layouter.PutNextRectangle(new Size().SetRandom(3, 30, 3, 20));
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
    }
}