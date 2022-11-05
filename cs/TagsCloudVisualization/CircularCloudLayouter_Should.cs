using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter _cloud;

        [SetUp]
        public void SetUp()
        {
            var rnd = new Random();
            _cloud = new CircularCloudLayouter(new Point(rnd.Next(-500, 500), rnd.Next(-500, 500)));
        }

        [TestCase(1, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 0)]
        public void CloudCtorShouldTakeAnyArgs(int X, int Y)
        {
            _cloud = new CircularCloudLayouter(new Point(X, Y));
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void PutNextRectangle_ThrowExceptionOnIncorrectArgs(int width, int height)
        {
            var action = () => _cloud.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentException>().WithMessage("only positive size");
        }

        [Test]
        public void PutNextRectangle_FirstRectangle_InCenterOfCloud()
        {
            var resultRect = _cloud.PutNextRectangle(new Size(10, 10));

            resultRect.Should().Be(new Rectangle(_cloud.Center.X - 5, _cloud.Center.Y - 5, 10, 10));
        }

        [Test]
        public void PutNextRectangle_AddRectanglesToList()
        {
            _cloud.PutNextRectangle(new Size(10, 10));
            _cloud.PutNextRectangle(new Size(10, 10));

            _cloud.Rectangles.Count.Should().Be(2);
        }

        [Test]
        public void PutNextRectangle_SecondRectangleCloseToFirst()
        {
            _cloud.PutNextRectangle(new Size(10, 10));
            _cloud.PutNextRectangle(new Size(10, 10));

            var first = _cloud.Rectangles.First();
            var second = _cloud.Rectangles.Last();

            var dx = Math.Abs(first.GetCenter().X - second.GetCenter().X);
            var dy = Math.Abs(first.GetCenter().Y - second.GetCenter().Y);

            (dx + dy).Should().Be(10);
        }

        [Test]
        public void PutNextRectangle_RectanglesDontIntersectEachOther()
        {
            FillCloudRandomly(50, 50, 250);
            var rectangles = _cloud.Rectangles.ToArray();
            for (int i = 0; i < rectangles.Length; i++)
                for (int j = i + 1; j < rectangles.Length; j++)
                    if (rectangles[j].GetCenter() != rectangles[i].GetCenter())
                        rectangles[i].IntersectsWith(rectangles[j]).Should().Be(false);
        }

        [Test]
        public void PutNextRectangle_ResultCloudLikeCircle()
        {
            FillCloudRandomly(50, 50, 250);

            int heighest = int.MaxValue, lowest = int.MinValue;
            int left = int.MaxValue, right = int.MinValue;
            var totalSquare = 0.0;

            foreach (var rect in _cloud.Rectangles)
            {
                if (rect.Top < heighest)
                    heighest = rect.Top;
                if (rect.Bottom > lowest)
                    lowest = rect.Bottom;
                if (rect.Left < left)
                    left = rect.Left;
                if (rect.Right > right)
                    right = rect.Right;
                totalSquare += rect.Width * rect.Height;
            }

            double dx = Math.Abs(right - left);
            double dy = Math.Abs(heighest - lowest);
            var square = Math.PI * Math.Pow(Math.Max(dx, dy), 2) / 4;


            (Math.Max(dx, dy) / Math.Min(dx, dy)).Should().BeGreaterThan(0.9);
            (totalSquare / square).Should().BeGreaterThan(0.75);
        }

        private void FillCloudRandomly(int rectCount, int minRectSize, int maxRectSize)
        {
            var rnd = new Random();
            for (int i = 0; i < rectCount; i++)
            {
                var width = rnd.Next(minRectSize, maxRectSize);
                var height = rnd.Next(minRectSize, maxRectSize);
                _cloud.PutNextRectangle(new Size(width, height));
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var filename = TestContext.CurrentContext.Test.MethodName;
                var path = Environment.CurrentDirectory;
                Drawer.CreateImage(1500, 1500, _cloud.Rectangles, filename);
                Console.WriteLine($"Tag cloud visualization saved to file {path + "\\" + filename}");
            }
        }
    }
}
