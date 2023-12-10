using System;
using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using NUnit.Framework.Interfaces;
using System.Security.Claims;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagCloudVisualisationTest
    {
        private CircularCloudLayouter layouter;

        [TestCase(-1, -1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(0, 0)]
        public void CircularCloudLayouterConstructor_ThrowExceptionOnIncorrectCentralPoins(int x, int y)
        {
            Action a = () => new CircularCloudLayouter(new Point(x, y));
            a.Should().Throw<ArgumentException>();
        }

        [TestCase(1)]
        [TestCase(123)]
        public void LayoutRectangles_ReturnCorrectNumberOfRectangles(int rectCount)
        {
            layouter = new CircularCloudLayouter(new Point(500, 500));

            var sizes = new List<Size>();

            for (int i = 0; i < rectCount; i++)
            {
                sizes.Add(new Size(1, 1));
            }

            layouter.LayoutRectancles(sizes);

            layouter.Cloud.Count().Should().Be(rectCount);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangle()
        {
            layouter = new CircularCloudLayouter(new Point(50, 50));
            layouter.PutNextRectangle(new Size(1, 2)).Should().BeOfType(typeof(Rectangle));
        }

        [TestCase(1, 1)]
        [TestCase(20, 1)]
        [TestCase(256, 255)]
        [TestCase(1, 20)]
        public void PutNextRectangle_ShouldReturnRectangleOfCorrectSize(int width, int height)
        {
            layouter = new CircularCloudLayouter(new Point(500, 500));
            var rect = layouter.PutNextRectangle(new Size(width, height));
            rect.Width.Should().Be(width);
            rect.Height.Should().Be(height);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void PutNextRectangle_ShouldThrowExceptionOnIncorrectSize(int width, int height)
        {
            layouter = new CircularCloudLayouter(new Point(50, 50));
            Action a = () => layouter.PutNextRectangle(new Size(width, height));
            a.Should().Throw<ArgumentException>();
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(200)]
        public void LayoutRectangles_RectanglesShouldNotIntersect(int rectCount)
        {
            layouter = new CircularCloudLayouter(new Point(500, 500));
            var sizes = new List<Size>();
            var rnd = new Random();

            for (int i = 0; i < rectCount; i++)
            {
                sizes.Add(new Size(rnd.Next(1, 50), rnd.Next(1, 50)));
            }

            layouter.LayoutRectancles(sizes);

            foreach (var rectangleA in layouter.Cloud)
            {
                foreach (var rectangleB in layouter.Cloud)
                {
                    if (rectangleA == rectangleB) continue;
                    var isIntersect = rectangleA.IntersectsWith(rectangleB);

                    isIntersect.Should().BeFalse();
                }
            }
        }

        [TearDown]
        public void SaveImageOnTestFails()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                var img = layouter.ToImage();
                string filename = TestContext.CurrentContext.Test.Name + "_Failed_" + DateTime.Now.ToString("H - mm - ss") + ".png";
                img.Save(filename);
            }
        }
    }
}
