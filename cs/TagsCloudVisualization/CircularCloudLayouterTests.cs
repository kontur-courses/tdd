using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter cloudLayouter;

        private static List<List<int>> rectanglesSampleForPlaceCheck = new List<List<int>>
        {
            new List<int> { 30, 10, 470, 500 },
            new List<int> { 60, 30, 440, 470 },
            new List<int> { 40, 30, 500, 470 },
            new List<int> { 30, 40, 500, 500 },
            new List<int> { 40, 80, 460, 510 },
            new List<int> { 20, 30, 440, 510 }
        };

        private static List<List<int>> rectanglesSampleForFilledRectCheck = new List<List<int>>
        {
            new List<int> { 30, 10, 500, 500, 0, 0 },
            new List<int> { 60, 30, 470, 500, 30, 10 },
            new List<int> { 40, 30, 440, 470, 60, 40 },
            new List<int> { 30, 40, 440, 470, 100, 40 },
            new List<int> { 40, 80, 440, 470, 100, 40 },
            new List<int> { 20, 30, 440, 470, 100, 40 }
        };

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(new Point(500, 500));
        }
        
        [TearDown]
        public void E()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed)
            {
                var image = new Bitmap(800, 800);
                var brush = Graphics.FromImage(image);
                brush.DrawEllipse(new Pen(Color.Red, 3), 500, 500, 3, 3);
                foreach (var rectangle in cloudLayouter.Rectangles)
                {
                    brush.FillRectangle(new SolidBrush(Color.Green), rectangle);
                    brush.DrawRectangle(new Pen(Color.Black), rectangle);
                }
                image.Save(
                    $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.Name}_result.png");
                TestContext.Write(
                    $"Tag cloud visualization saved to file {TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.Name}_result.png");
            }
        }

        [Test]
        public void PullNextRectangle_ShouldPlace()
        {
            foreach (var source in rectanglesSampleForPlaceCheck)
            {
                cloudLayouter.PullNextRectangle(new Size(source[0], source[1]))
                    .Location.Should().BeEquivalentTo(new Point(source[2], source[3]));
            }
        }

        [Test]
        public void PullNextRectangle_ShouldPlaceFirstRectangleLeftFromCenter()
        {
            var center = new Point(0, 0);
            var size = new Size(30, 40);
            var cloudLayouter = new CircularCloudLayouter(center);
            var rect = new Rectangle(new Point(-size.Width, 0), size);
            cloudLayouter.PullNextRectangle(size).Should().BeEquivalentTo(rect);
        }
        
        [Test]
        public void UpdateFilledRect_ShouldBe()
        {
            foreach (var source in rectanglesSampleForFilledRectCheck)
            {
                cloudLayouter.PullNextRectangle(new Size(source[0], source[1]));
                cloudLayouter.filledArea.Should()
                    .BeEquivalentTo(new Rectangle(source[2], source[3], source[4], source[5]));
            }
        }

        [Test]
        public void PullNextRectangle_ShouldReturn_NotIntersectedRects()
        {
            for (var i = 5; i < 15; i++)
            {
                cloudLayouter.PullNextRectangle(new Size(i * 5, 100 - i * 5));
            }

            for (var i = 0; i < cloudLayouter.Rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < cloudLayouter.Rectangles.Count; j++)
                {
                    Assert.False(cloudLayouter.Rectangles[i].IntersectsWith(cloudLayouter.Rectangles[j]));
                }
            }
        }

        [Test]
        public void PullNextRectangle_ShouldPlace_SecondRectangleAboveFirst()
        {
            var firstRect = cloudLayouter.PullNextRectangle(new Size(30, 10));
            var secondRect = cloudLayouter.PullNextRectangle(new Size(50, 30));
            firstRect.Location.Should().BeEquivalentTo(new Point(470, 500));
            secondRect.Location.Should().BeEquivalentTo(new Point(450, 470));
        }

        [TestCase(-10, 10, TestName = "Negative width")]
        [TestCase(10, -10, TestName = "Negative height")]
        [TestCase(0, 10, TestName = "Zero width")]
        [TestCase(10, 0, TestName = "Zero height")]
        public void PullNextRectangle_ShouldThrowArgumentException_WhenSizeIncorrect(int width, int height)
        {
            Action act = () => cloudLayouter.PullNextRectangle(new Size(-10, 10));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Test_ShouldFail_ForPictureSave()
        {
            cloudLayouter.PullNextRectangle(new Size(20, 40));
            cloudLayouter.PullNextRectangle(new Size(30, 40));
            cloudLayouter.PullNextRectangle(new Size(20, 30));
            cloudLayouter.Rectangles.Should().BeEmpty();
        }
    }
}