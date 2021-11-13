using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point());
        }
        
        [TearDown]
        public void ShowFailedCloud()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var visualizer = new TagsCloudVisualiser(layouter);
            var image = visualizer.DrawCloud(new Size(800, 800));
            var fileName = TestContext.CurrentContext.Test.Name + "Fail.png";
            image.Save(fileName, ImageFormat.Png);
            Console.WriteLine("Failed test saved at " + Directory.GetCurrentDirectory() + "\\" + fileName);
        }
        
        [Test]
        public void AddRectangle()
        {
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.Rectangles.Should().HaveCount(1);
        }

        [Test]
        public void FirstRectangleInCenter()
        {
            var rect = layouter.PutNextRectangle(new Size(2, 2));
            var expected = new RectangleF(-1, -1, 2, 2);
            rect.Should().Be(expected);
        }
        
        [Test]
        public void AddTwoRectangles()
        {
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.Rectangles.Should().HaveCount(2);
        }
        
        [Test]
        public void SecondRectangleContactsFirst()
        {
            var rect1 = layouter.PutNextRectangle(new Size(2, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            rect1.Contacts(rect2).Should().BeTrue();
        }

        [Test]
        public void SecondRectangleOnCloseSide()
        {
            layouter.PutNextRectangle(new Size(4, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            rect2.X.Should().Be(-1);
        }

        [Test]
        public void SecondAndThirdRectangleDifferent()
        {
            layouter.PutNextRectangle(new Size(2, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            var rect3 = layouter.PutNextRectangle(new Size(2, 2));
            rect2.Should().NotBe(rect3);
        }
        
        [Test]
        public void SecondAndThirdRectangleOnDifferentSides()
        {
            layouter.PutNextRectangle(new Size(4, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            var rect3 = layouter.PutNextRectangle(new Size(2, 2));
            rect2.Y.Should().BeNegative();
            rect3.Y.Should().BePositive();
        }

        [Test]
        public void CanPlaceInCorner()
        {
            for(var i = 0; i < 6; i++) layouter.PutNextRectangle(new Size(2, 2));
            var middleRectangle = layouter.Rectangles[0];
            var cornerRectangle = layouter.Rectangles[5];
            middleRectangle.Should().NotBe(cornerRectangle);
        }

        [Test]
        public void RectanglesNotIntersecting()
        {
            layouter.PutNextRectangle(new Size(10, 10));
            layouter.PutNextRectangle(new Size(20, 10));
            layouter.PutNextRectangle(new Size(10, 20));
            foreach (var rect1 in layouter.Rectangles)
            {
                foreach (var rect2 in layouter.Rectangles)
                {
                    if (rect1 == rect2) continue;
                    if (rect1.IntersectsWith(rect2))
                        Assert.Fail();
                }
            }
        }

        [Test]
        public void RectanglePlacedInside()
        {
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(10, 2));
            layouter.PutNextRectangle(new Size(2, 6));
            layouter.PutNextRectangle(new Size(2, 2));
            var expected = new RectangleF(1, -3, 2, 2);
            layouter.Rectangles[^1].Should().Be(expected);
        }
    }
}