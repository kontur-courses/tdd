using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloud;

        [SetUp]
        public void SetUp()
        {
            var center = new Point(750, 750);
            cloud = new CircularCloudLayouter(center, SpiralFunction.GetPointFinderFunction(center));
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void PutNextRectangle_ThrowExceptionOnIncorrectArgs(int width, int height)
        {
            var action = () => cloud.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentException>().WithMessage("only positive size");
        }

        [Test]
        public void PutNextRectangle_FirstRectangle_InCenterOfCloud()
        {
            var resultRect = cloud.PutNextRectangle(new Size(10, 10));

            resultRect.Should().Be(new Rectangle(cloud.Center.X - 5, cloud.Center.Y - 5, 10, 10));
        }

        [Test]
        public void PutNextRectangle_AddRectanglesToList()
        {
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(10, 10));

            cloud.GetRectangles().Count.Should().Be(2);
        }

        [Test]
        public void PutNextRectangle_SecondRectangleCloseToFirst()
        {
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(10, 10));

            var first = cloud.GetRectangles().First();
            var second = cloud.GetRectangles().Last();

            first.Left.Should().Be(second.Right);
        }

        [Test]
        public void PutNextRectangle_RectanglesDontIntersectEachOther()
        {
            FillCloudRandomly(50, 50, 250);
            var rectangles = cloud.GetRectangles().ToArray();
            for (int i = 0; i < rectangles.Length; i++)
                for (int j = i + 1; j < rectangles.Length; j++)
                    rectangles[i].IntersectsWith(rectangles[j]).Should().Be(false);
        }

        [Test]
        public void PutNextRectangle_ResultCloudLikeCircle()
        {
            FillCloudRandomly(50, 50, 250);

            int highest = int.MaxValue, lowest = int.MinValue;
            int left = int.MaxValue, right = int.MinValue;
            var totalSquare = 0;

            foreach (var rect in cloud.GetRectangles())
            {
                if (rect.Top < highest)
                    highest = rect.Top;
                if (rect.Bottom > lowest)
                    lowest = rect.Bottom;
                if (rect.Left < left)
                    left = rect.Left;
                if (rect.Right > right)
                    right = rect.Right;
                totalSquare += rect.Width * rect.Height;
            }

            double dx = Math.Abs(right - left);
            double dy = Math.Abs(highest - lowest);
            var square = Math.PI * Math.Pow(Math.Max(dx, dy), 2) / 4;

            (Math.Min(dx, dy) / Math.Max(dx, dy)).Should().BeGreaterThan(0.9);
            (totalSquare / square).Should().BeGreaterThan(0.75);
        }

        private void FillCloudRandomly(int rectCount, int minRectSize, int maxRectSize)
        {
            var rnd = new Random();
            for (int i = 0; i < rectCount; i++)
            {
                var width = rnd.Next(minRectSize, maxRectSize);
                var height = rnd.Next(minRectSize, maxRectSize);
                cloud.PutNextRectangle(new Size(width, height));
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var filename = TestContext.CurrentContext.Test.MethodName;
                var path = Environment.CurrentDirectory;
                Drawer.CreateImage(1500, 1500, cloud.GetRectangles(), filename!);
                Console.WriteLine($"Tag cloud visualization saved to file {path + "\\" + filename}");
            }
        }
    }
}
