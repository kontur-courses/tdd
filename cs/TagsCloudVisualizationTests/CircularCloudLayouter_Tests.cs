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
            Math.Abs(firstRectCenterY - secondRectCenterY).Should().Be((firstRect.Height + secondRect.Height) / 2);
        }

        [Test]
        public void Rectangle_ShouldFindClosestPosition()
        {
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));

            var rectangle = layouter.PutNextRectangle(new Size(50, 50));
            var rectangleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            var distanceToCenter = Math.Sqrt(Math.Pow(rectangleCenter.X - center.X, 2) + Math.Pow(rectangleCenter.Y - center.Y, 2));
            distanceToCenter.Should().Be(50, "because last closest position is near the central rectangle");
        }

        [Test]
        public void Image_ShouldBeLikeACircle()
        {
            var bigRectSize = new Size(50, 50);
            var smallRectSize = new Size(20, 20);
            layouter.PutNextRectangle(bigRectSize);
            layouter.PutNextRectangle(bigRectSize);
            layouter.PutNextRectangle(bigRectSize);
            layouter.PutNextRectangle(bigRectSize);
            layouter.PutNextRectangle(bigRectSize);

            var rectangles = new List<Rectangle>
            {
                layouter.PutNextRectangle(smallRectSize),
                layouter.PutNextRectangle(smallRectSize),
                layouter.PutNextRectangle(smallRectSize),
                layouter.PutNextRectangle(smallRectSize)
            };
            var rectangleToCheck = new Rectangle(
                center.X - bigRectSize.Width / 2 - 1,
                center.Y - bigRectSize.Height / 2 - 1,
                bigRectSize.Width + 2,
                bigRectSize.Height + 2);
            rectangles.All(
                rect => rect.IntersectsWith(rectangleToCheck)
            ).Should().BeTrue("because there is enough plase on the image to set rectangles inside");
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
                DrawRectangles(rectangles, center, new Size(500, 500), fileName);
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