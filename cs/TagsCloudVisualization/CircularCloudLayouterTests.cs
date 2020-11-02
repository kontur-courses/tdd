using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private Point center = new Point(400, 400);
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void FirstRectangle_ShouldBeInCenter()
        {
            layouter.PutNextRectangle(new Size(100, 70))
                .Should().BeEquivalentTo(new Rectangle(350, 365, 100, 70));
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
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));

            var rectangles = new List<Rectangle>
            {
                layouter.PutNextRectangle(new Size(20, 20)),
                layouter.PutNextRectangle(new Size(20, 20)),
                layouter.PutNextRectangle(new Size(20, 20)),
                layouter.PutNextRectangle(new Size(20, 20))
            };
            rectangles.All(
                rect => rect.X >= center.X - 45
                        && rect.X + rect.Width <= center.X + 45
                        && rect.Y >= center.Y - 45
                        && rect.Y + rect.Height <= center.Y + 45
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
                SaveWrongImage(rectangles, center, fileName);
            }
        }

        private void SaveWrongImage(List<Rectangle> rectangles, Point center, string fileName)
        {
            var image = new Bitmap(500, 500);
            var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black);
            var rand = new Random();
            graphics.DrawRectangles(new Pen(Color.Aqua, 2), rectangles.ToArray());
            graphics.FillEllipse(Brushes.Green, new Rectangle(center - new Size(2, 2), new Size(4, 4)));
            image.Save(fileName);
        }
    }
}