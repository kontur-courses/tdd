using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private Point center = new Point(400, 400);
        private CircularCloudLayouterTestWrapper layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouterTestWrapper(center);
        }

        [Test]
        public void FirstRectangle_ShouldBeInCenter()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(100, 70));
            firstRectangle
                .Should()
                .BeEquivalentTo(new Rectangle(
                    center.X - firstRectangle.Width / 2, 
                    center.Y - firstRectangle.Height / 2, 
                    firstRectangle.Width,
                    firstRectangle.Height));
        }

        [Test]
        public void SecondRectangle_ShouldBeNearCenter()
        {
            var firstRect = layouter.PutNextRectangle(new Size(100, 70));
            var secondRect = layouter.PutNextRectangle(new Size(70, 50));
            var firstRectCenterY = firstRect.Y + firstRect.Height / 2;
            var secondRectCenterY = secondRect.Y + secondRect.Height / 2;
            Math.Abs(firstRectCenterY - secondRectCenterY)
                .Should().Be((firstRect.Height + secondRect.Height) / 2, "because secondhorizontal rectangle should be above or below first");
        }

        [Test]
        public void AnotherRectangle_ShouldFindClosestPosition()
        {
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));

            var rectangle = layouter.PutNextRectangle(new Size(50, 50));
            var distanceToCenter = 
                CircularCloudLayouter.CalculateDistance(center, CircularCloudLayouter.CalculateCenterPosition(rectangle));
            distanceToCenter.Should().Be(50, "because last closest position is near the central rectangle");
        }

        [Test]
        public void Image_ShouldBeLikeACircle()
        {
            var squareSize = new Size(30, 30);
            for (var i = 0;
                i < 5 * (5 * 3) + 2 * (2 * 13 + 11 + 9 + 5); // ровно столько квадратиков создают красивый круг
                i++)
                layouter.PutNextRectangle(squareSize);
            layouter.Rectangles
                .Select(rectangle => 
                    CircularCloudLayouter.CalculateDistance(center, CircularCloudLayouter.CalculateCenterPosition(rectangle)))
                .Should().BeInAscendingOrder();
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome == ResultState.Failure ||
                context.Result.Outcome == ResultState.Error)
            {
                var fileName = $"{context.WorkDirectory}\\{context.Test.Name}.png";
                var rectangles = layouter.Rectangles;
                TestContext.WriteLine($"Tag cloud visualization saved to file {fileName}");
                DrawRectangles(rectangles, center, new Size(800, 800), fileName);
            }
        }

        private void DrawRectangles(List<Rectangle> rectangles, Point center, Size imageSize, string fileName)
        {
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black);
            graphics.DrawRectangles(new Pen(Color.Aqua, 2), rectangles.ToArray());
            graphics.FillEllipse(Brushes.Green, new Rectangle(center - new Size(2, 2), new Size(4, 4)));
            image.Save(fileName);
        }
    }
}