using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouts;
using TagsCloudVisualization.CloudVisualizers;

namespace TagsCloudVisualizationTest
{
    [TestFixture]
    public class CircularCloudLayout_should
    {
        private Point center;
        private CircularCloudLayout cloud;

        [SetUp]
        public void SetUpCloud()
        {
            center = new Point();
            cloud = new CircularCloudLayout(center);
        }

        private static IEnumerable IncorrectSizes
        {
            get
            {
                yield return new Size(-2, 3);
                yield return new Size(-3, -4);
                yield return new Size(4, -5);
                yield return new Size(0, 0);
                yield return new Size(14, 0);
                yield return new Size(0, 14);
            }
        }

        [Test]
        [TestCaseSource(nameof(IncorrectSizes))]
        public void PutNextRectangle_ShouldFail_OnIncorrectSizes(Size size)
        {
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(size));
        }
        
        [Test]
        public void PutNextRectangle_FirstRectangle_ShouldBeInCenter()
        {
            var size = new Size(100, 100);
            var expected = new Rectangle(center, size);

            var result = cloud.PutNextRectangle(size);

            result.Should().Be(expected);
        }

        [Test]
        public void PutNextRectangle_SecondRectangle_ShouldNotIntersect_WithFirst()
        {
            var size = new Size(100, 100);

            var firstRectangle = cloud.PutNextRectangle(size);
            var secondRectangle = cloud.PutNextRectangle(size);

            firstRectangle.Should().NotBe(secondRectangle);
        }

        [Test]
        public void PutNextRectangle_RectanglesShouldBe_InsideCircle()
        {
            var size = new Size(100, 100);
            var rectangles = new List<Rectangle>();
            
            for (var i = 0; i < 100; i++)
            {
                rectangles.Add(cloud.PutNextRectangle(size));
            }
            var maxRadius = CalculateRadius(rectangles.Last()) + 100;
            
            rectangles
                .Should()
                .Match(rects => rects.All(rect => CalculateRadius(rect) < maxRadius));
        }

        [Test]
        public void PutNextRectangle_RectanglesSquares_ShouldBeLesser_ThanCircleSquare()
        {
            var size = new Size(100, 100);
            var rectangles = new List<Rectangle>();
            
            for (int i = 0; i < 10; i++)
            {
                rectangles.Add(cloud.PutNextRectangle(size));
            }
            var radius = CalculateRadius(rectangles.Last());
            var circleSquare = Math.PI * radius * radius;
            var squareSum = rectangles.Aggregate(0.0, ((i, rectangle) => i + rectangle.Height * rectangle.Width));
            
            squareSum.Should().BeLessThan(circleSquare * 0.5);
        }

        private double CalculateRadius(Rectangle rect)
        {
            var xDistance = Math.Abs(rect.X) + rect.Width - center.X;
            var yDistance = Math.Abs(rect.Y) + rect.Height - center.Y;
            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }

        [TearDown]
        public void OnTearDown()
        {
            if (TestContext.CurrentContext.Result.FailCount > 0)
            {
                var visualizer = new CloudVisualizer(Pens.Red, cloud);
                var image = visualizer.GenerateImage(new Size(3000, 3000));
                var path = TestContext.CurrentContext.TestDirectory +
                           $"//{TestContext.CurrentContext.Test.Name}_failed.png";
                image.Save(path, ImageFormat.Png);
                TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }
    }
}