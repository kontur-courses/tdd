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
    public class CircularCloudMakerShould
    {
        private CircularCloudMaker maker;

        [SetUp]
        public void SetUp()
        {
            maker = new CircularCloudMaker(new Point());
        }
        
        [TearDown]
        public void ShowFailedCloud()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var visualizer = new TagsCloudVisualiser(maker);
            var image = visualizer.DrawCloud(new Size(800, 800));
            var fileName = TestContext.CurrentContext.Test.Name + "Fail.png";
            image.Save(fileName, ImageFormat.Png);
            Console.WriteLine("Failed test saved at " + Directory.GetCurrentDirectory() + "\\" + fileName);
        }
        
        [Test]
        public void AddRectangle()
        {
            maker.PutNextRectangle(new Size(2, 2));
            maker.Rectangles.Should().HaveCount(1);
        }

        [Test]
        public void FirstRectangleInCenter()
        {
            var rect = maker.PutNextRectangle(new Size(2, 2));
            var expected = new RectangleF(-1, -1, 2, 2);
            rect.Should().Be(expected);
        }
        
        [Test]
        public void AddTwoRectangles()
        {
            maker.PutNextRectangle(new Size(2, 2));
            maker.PutNextRectangle(new Size(2, 2));
            maker.Rectangles.Should().HaveCount(2);
        }
        
        [Test]
        public void SecondRectangleContactsFirst()
        {
            var rect1 = maker.PutNextRectangle(new Size(2, 2));
            var rect2 = maker.PutNextRectangle(new Size(2, 2));
            rect1.Contacts(rect2).Should().BeTrue();
        }

        [Test]
        public void SecondRectangleOnCloseSide()
        {
            maker.PutNextRectangle(new Size(4, 2));
            var rect2 = maker.PutNextRectangle(new Size(2, 2));
            rect2.X.Should().Be(-1);
        }

        [Test]
        public void SecondAndThirdRectangleDifferent()
        {
            maker.PutNextRectangle(new Size(2, 2));
            var rect2 = maker.PutNextRectangle(new Size(2, 2));
            var rect3 = maker.PutNextRectangle(new Size(2, 2));
            rect2.Should().NotBe(rect3);
        }
        
        [Test]
        public void SecondAndThirdRectangleOnDifferentSides()
        {
            maker.PutNextRectangle(new Size(4, 2));
            var rect2 = maker.PutNextRectangle(new Size(2, 2));
            var rect3 = maker.PutNextRectangle(new Size(2, 2));
            (rect2.Y *  rect3.Y).Should().BeNegative();
        }

        [Test]
        public void CanPlaceInCorner()
        {
            for(var i = 0; i < 6; i++) maker.PutNextRectangle(new Size(2, 2));
            var middleRectangle = maker.Rectangles[0];
            var cornerRectangle = maker.Rectangles[5];
            middleRectangle.Should().NotBe(cornerRectangle);
        }

        [Test]
        public void RectanglesNotIntersecting()
        {
            maker.PutNextRectangle(new Size(10, 10));
            maker.PutNextRectangle(new Size(20, 10));
            maker.PutNextRectangle(new Size(10, 20));
            foreach (var rect1 in maker.Rectangles)
            {
                foreach (var rect2 in maker.Rectangles)
                {
                    if (rect1 == rect2) continue;
                    if (rect1.IntersectsWith(rect2))
                        Assert.Fail();
                }
            }
        }

        [Test]
        public void RectanglesPlacedInsideHole()
        {
            /*layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(10, 2));
            layouter.PutNextRectangle(new Size(6, 2));
            layouter.PutNextRectangle(new Size(10, 2));
            layouter.PutNextRectangle(new Size(6, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            var expectedPoses = new []{new PointF(1, -3)};
            layouter.Rectangles[^1].Should().Be(expected);*/
        }
    }
}