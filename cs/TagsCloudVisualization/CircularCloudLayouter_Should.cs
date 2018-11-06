using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private int centerWidth = 1000;
        private int centerHeight = 1000;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(centerWidth, centerHeight));
        }
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = TestContext.CurrentContext.TestDirectory;
                var fileName = $"{TestContext.CurrentContext.Test.Name} failed on {DateTime.Now}.png";
                var outputFileName = Path.Combine(path, fileName);
                using (var bitmap = new Bitmap(layouter.Size.Width, layouter.Size.Height))
                {
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        foreach (var rectangle in layouter.GetRectangles())
                        {
                            graphics.DrawRectangle(new Pen(Color.Red), rectangle);
                        }
                        bitmap.Save(outputFileName);
                    }
                }

                TestContext.WriteLine($"Tag cloud visualization saved to file {outputFileName}");
            }
        }
        [Test]
        public void HaveSizeTwiceAsMuchCenterPoint_OnInitialization()
        {
            layouter.Size.Should().Be(new Size(2 * centerWidth, 2 * centerHeight));
        }

        [Test]
        public void HaveEmptyRectanglesCollection_OnInitialization()
        {
            layouter.GetRectangles().Should().BeEmpty();
        }

        [Test]
        public void ThrowArgumentException_OnPuttingNextRectangleOfEmptySize()
        {
            Action action = () => layouter.PutNextRectangle(new Size());
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ThrowArgumentException_OnPuttingNextRectangleOfNegativeSize()
        {
            Action action = () => layouter.PutNextRectangle(new Size(-5, -5));
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CreateFirstRectangle_InCenter()
        {
            var rectangle = layouter.PutNextRectangle(new Size(100, 50));
            rectangle.Location.Should().Be(layouter.Center);
        }

        [Test]
        public void AddSeveralRectangles()
        {
            for (int i = 0; i < 1000; i++)
            {
                layouter.PutNextRectangle(new Size(100, 50));
            }
            layouter.GetRectangles().Count.Should().Be(1000);
        }

        [Test]
        public void AddRectanglesWithoutIntersections()
        {
            for (int i = 0; i < 1000; i++)
            {
                layouter.PutNextRectangle(new Size(100, 50));
            }

            foreach (var rectangle in layouter.GetRectangles())
            {
                layouter.GetRectangles()
                    .All(e => e.IntersectsWith(rectangle)).Should().BeFalse();
            }
        }

        [Test]
        public void RenderRoundCloud()
        {
            for (int i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(100, 50));
            }

            int leftBound = centerWidth, rightBound = centerWidth, topBound = centerHeight, bottomBound = centerHeight;

            foreach (var rectangle in layouter.GetRectangles())
            {
                if (rectangle.Location.X > rightBound)
                    rightBound = rectangle.Location.X;

                if (rectangle.Location.X < leftBound)
                    leftBound = rectangle.Location.X;

                if (rectangle.Location.Y > topBound)
                    topBound = rectangle.Location.Y;

                if (rectangle.Location.Y < bottomBound)
                    bottomBound = rectangle.Location.Y;
            }

            var relativeRightBound = Math.Abs(rightBound - centerWidth);
            var relativeLeftBound = Math.Abs(leftBound - centerWidth);

            var relativeTopBound = Math.Abs(topBound - centerHeight);
            var relativeBottomBound = Math.Abs(bottomBound - centerHeight);

            Assert.That(relativeRightBound, Is.EqualTo(relativeLeftBound).Within(50));
            Assert.That(relativeTopBound, Is.EqualTo(relativeBottomBound).Within(50));
        }
    }
}
