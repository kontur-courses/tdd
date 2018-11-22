using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloudVisualization;


namespace TagsCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private List<Rectangle> puttedRectangles;
        private CircularCloudLayouter circularCloudLayouter;
        private Point center = new Point(500, 500);
        private readonly int Width = 1920;
        private readonly int Height = 1080;
        private TagsCloudRenderer renderer = new TagsCloudRenderer();

        [SetUp]
        public void SetUp()
        {
            puttedRectangles = new List<Rectangle>();
            circularCloudLayouter = new CircularCloudLayouter(center);
        }

        [TestCase(-1, 1, TestName = "X less than 0")]
        [TestCase(1, -1, TestName = "Y less than 0")]
        public void ctor_ThrowsArgumentExceptionWhen(int centerX, int centerY)
        {
            Action act = () => new CircularCloudLayouter(new Point(centerX, centerY));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ReturnsTwoNonIntersectRectangles()
        {
            var rect1 = circularCloudLayouter.PutNextRectangle(new Size(50, 10));
            var rect2 = circularCloudLayouter.PutNextRectangle(new Size(70, 140));
            puttedRectangles.Add(rect1);
            puttedRectangles.Add(rect2);
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [TestCase(4)]
        [TestCase(16)]
        [TestCase(65)]
        [TestCase(100)]
        public void PutNextRectangle_ReturnsManyNonIntersectRectangles(int rectanglesCount)
        {
            for (var i = 0; i < rectanglesCount; i++)
            {
                puttedRectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(50, 10)));
            }

            for (var i = 0; i < puttedRectangles.Count; i++)
            {
                for (var j = 0; j < puttedRectangles.Count; j++)
                {
                    if (i != j)
                    {
                        puttedRectangles[i].IntersectsWith(puttedRectangles[j]).Should().BeFalse();
                    }
                }
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsDifferentRectangles()
        {
            var rect1 = circularCloudLayouter.PutNextRectangle(new Size(50, 10));
            var rect2 = circularCloudLayouter.PutNextRectangle(new Size(70, 140));
            puttedRectangles.Add(rect1);
            puttedRectangles.Add(rect2);
            rect1.Should().NotBe(rect2);
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        public void PutNextRectangle_ReturnsManyRectanglesInscribedInACircle(int rectanglesCount)
        {
            var totalSquare = 0;
            for (var i = 0; i < rectanglesCount; i++)
            {
                var x = 50;
                var y = 10;
                puttedRectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(x, y)));
                totalSquare += x * y;
            }

            var r = (int) Math.Sqrt(totalSquare / Math.PI);
            foreach (var rect in puttedRectangles)
            {
                var distance = Math.Sqrt(Math.Pow(rect.X - center.X, 2) + Math.Pow(rect.Y - center.Y, 2));
                distance.Should().BeLessThan(r * 1.2);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var name = TestContext.CurrentContext.Test.FullName + ".png";
                var path = AppDomain.CurrentDomain.BaseDirectory + name;
                Console.WriteLine($"Tag cloud visualization saved to {path}");
                renderer.RenderIntoFile(path, circularCloudLayouter.TagsCloud);
            }
        }
    }
}