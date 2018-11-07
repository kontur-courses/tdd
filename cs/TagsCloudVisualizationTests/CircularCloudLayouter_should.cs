using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;


namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        public const int MaxWidth = 30;
        public const int MaxHeight = 20;

        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(100, 100));
        }


        [Test]
        public void not_contain_intersected_rectangles()
        {
            for (int i = 0; i < 100; i++)
            {   
                layouter.PutNextRectangle(new Size().SetRandom(30, 20));
            }

            foreach (var r1 in layouter.Rectangles)
                foreach (var r2 in layouter.Rectangles)
                    if (r1 != r2)
                        r1.IntersectsWith(r2).Should().BeFalse();
        }

        [Test]
        public void have_the_form_of_cirle()
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
                    .Vertexes()
                    .Select(p =>
                        Math.Sqrt(Math.Pow(layouter.Spiral.Center.X - p.X, 2) +
                                  Math.Pow(layouter.Spiral.Center.Y - p.Y, 2)))
                    .Max();

                actualMaxRadius = Math.Max(actualMaxRadius, currentMaxRadius);
            }

            actualMaxRadius.Should().BeLessOrEqualTo(expectedRadius);
        }
    }
}