using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(400, 400));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            var path = String.Format(@"C:\Users\{0}\Desktop\{1}", Environment.UserName,
                TestContext.CurrentContext.Test.Name);
            var visualiser = new CircularCloudVisualiser(Color.Blue, Color.Black, Color.Yellow);
            visualiser.SaveBitmap(path, 800, 800, layouter.Result);

            Console.WriteLine("Tag cloud visualization saved to file {0}.bmp", path);
        }

        [Test]
        public void CircularCloudLayouter_CreatesEmpty()
        {
            layouter.Result.Count.Should().Be(0);
        }

        [TestCase(0, 0, Description = "Zeros")]
        [TestCase(10, 0, Description = "Non-zero")]
        [TestCase(-5, -100, Description = "Negative")]
        public void CircularCloudLayouter_CreatesCenterCorrectly(int x, int y)
        {
            new CircularCloudLayouter(x, y).Center.Should().Be(new Point(x, y));
        }

        [TestCase(0, 0, 10, 6, Description = "Zero center")]
        [TestCase(-5, 4, 10, 6, Description = "Non zero center")]
        [TestCase(2, 2, 7, 3, Description = "Odd width and height")]
        public void PutFirstRectangle_InCenter(int x, int y, int width, int height)
        {
            var layouter = new CircularCloudLayouter(new Point(x, y));
            layouter.PutNextRectangle(new Size(width, height)).Location
                .IsSameOrEqualTo(new Point(x - width / 2, y - height / 2));
        }

        [TestCase(new [] {5, 5, 5, 5} , new[] { -5, -5, 5, 5 }, ExpectedResult = false)]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { 2, 2, 5, 5 }, ExpectedResult = true, Description = "Internal rectangle")]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { -2, -2, 5, 5 }, ExpectedResult = true)]
        [TestCase(new[] { -2, -2, 5, 5 }, new[] { 0, 0, 10, 10 }, ExpectedResult = true)]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { 0, 20, 10, 10 }, ExpectedResult = false, Description = "Y-Axis is parallel")]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { -10, 20, 10, 10 }, ExpectedResult = false)]
        public bool CheckCollision_BetweenTwoRectangles(int[] rect, int[] other)
        {
            return layouter.IsCollision(
                new Rectangle(rect[0], rect[1], rect[2], rect[3]),
                new Rectangle(other[0], other[1], other[2], other[3]));
        }

        [TestCase(40, 40, 15, 15, Description = "Equal rectangles")]
        [TestCase(10, 30, 10, 30)]
        [TestCase(30, 50, 10, 20)]
        public void CloudIsDenseEnough(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            layouter = new CircularCloudLayouter(0, 0);
            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(rnd.Next(minWidth, maxWidth), rnd.Next(minHeight, maxHeight)));
            }

            var radius = 0.0;
            var occupiedArea = 0.0;
            foreach (var rectangle in layouter.Result)
            {
                occupiedArea += rectangle.Width * rectangle.Height;

                for (var i = rectangle.X; i <= rectangle.X + rectangle.Width; i += rectangle.Width)
                {
                    for (var j = rectangle.Y; j <= rectangle.Y + rectangle.Height; j += rectangle.Height)
                    {
                        var range = Math.Sqrt(i * i + j * j);
                        if (range > radius)
                            radius = range;
                    }
                }
            }

            (occupiedArea / (radius * radius * Math.PI)).Should().BeGreaterThan(0.5);
        }
    }
}
