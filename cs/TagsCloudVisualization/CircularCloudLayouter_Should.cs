using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Specialized;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using static TagsCloudVisualization.EntryPoint;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudConstructor_Should
    {
        [TestCase(0, 0, TestName = "is zero")]
        [TestCase(100, 100, TestName = "is in I quarter")]
        [TestCase(100, -100, TestName = "is in II quarter")]
        [TestCase(-100, 100, TestName = "is in III quarter")]
        [TestCase(-100, -100, TestName = "is in IV quarter")]
        public void NotThrow_WhenCenter(int x, int y)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action creation = () => new CircularCloudLayouter(new Point(x, y));
            creation.Should()
                    .NotThrow();
        }
    }

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [SetUp]
        public void SetUp()
        {
            center = new Point(3, 4);
            layouter = new CircularCloudLayouter(new Point(3, 4));
        }

        [TearDown]
        public void TearDown()
        {
            if (!TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
                return;
            var path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) +
                       $@"\{TestContext.CurrentContext.Test.Name}.png";
            DrawRectangles(layouter.Rectangles, path);
        }

        private CircularCloudLayouter layouter;
        private Point center;

        [TestCase(0, 1, TestName = "has zero as width")]
        [TestCase(1, 0, TestName = "has zero as height")]
        [TestCase(-1, 5, TestName = "has negative width")]
        [TestCase(1, -5, TestName = "has negative height")]
        public void ThrowArgumentException_WhenSize(int w, int h)
        {
            Action addition = () => layouter.PutNextRectangle(new Size(w, h));
            addition.Should()
                    .Throw<ArgumentException>()
                    .WithMessage("size has non positive parts");
        }

        [TestCase(100, 1000, TestName = "N = 100, M = 1000 ms")]
        [TestCase(100, 1000, TestName = "N = 1000, M = 1000 ms")]
        [TestCase(100, 1000, TestName = "N = 10000, M = 1000 ms")]
        [TestCase(100, 1000, TestName = "N = 100000, M = 3500 ms")]
        public void AddNRectangles_FasterThanMms(int n, int m)
        {
            void Addition()
            {
                var random = new Random(42);

                for (var i = 0; i < n; i++)
                    layouter.PutNextRectangle(new Size(random.Next(10, 50), random.Next(10, 30)));
            }

            new ExecutionTime(Addition).Should()
                                       .BeLessThan(new TimeSpan(0, 0, 0, 0, m));
            layouter.Rectangles.Should()
                    .HaveCount(n);
        }

        [Test]
        public void AddRectangleToRectangles()
        {
            for (var i = 1; i < 6; i++) layouter.PutNextRectangle(new Size(i, 3 * i));

            layouter.Rectangles.Should()
                    .NotContainNulls()
                    .And.HaveCount(5);
        }

        [Test]
        public void AddSeveralRectanglesToRectangles()
        {
            layouter.PutNextRectangles(Enumerable.Range(10, 10)
                                                 .Select((n, i) => new Size(n, i + 1)));
            layouter.Rectangles.Should()
                    .NotContainNulls()
                    .And.HaveCount(11);
        }

        [Test]
        public void PutFirstRectangleToCenter()
        {
            var size = new Size(3, 4);
            layouter.PutNextRectangle(size)
                    .Should()
                    .BeEquivalentTo(new Rectangle(center, size));
        }

        [Test]
        public void PutSecondRectangle_SoThatItDoesNotIntersectsWithFirst()
        {
            var first = layouter.PutNextRectangle(new Size(3, 4));
            var second = layouter.PutNextRectangle(new Size(5, 6));
            second.IntersectsWith(first)
                  .Should()
                  .BeFalse();
        }
    }
}
