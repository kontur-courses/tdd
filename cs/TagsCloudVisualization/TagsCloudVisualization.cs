using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions.Specialized;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly IEnumerator<Point> spiralGenerator;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            spiralGenerator = new SpiralGenerator(center).GetEnumerator();
            spiralGenerator.MoveNext();
        }

        public IEnumerable<Rectangle> Rectangles => rectangles.AsEnumerable();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException("size has non positive parts");
            }

            var nextPosition = spiralGenerator.Current;
            var rectangle = new Rectangle(nextPosition, rectangleSize);
            while (rectangles.Exists(rect => rectangle.IntersectsWith(rect)))
            {
                spiralGenerator.MoveNext();
                nextPosition = spiralGenerator.Current;
                rectangle = new Rectangle(nextPosition, rectangleSize);
            }

            rectangles.Add(rectangle);
            return rectangle;
        }

        public void PutNextRectangles(IEnumerable<Size> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                PutNextRectangle(rectangle);
            }
        }
    }

    internal class SpiralGenerator : IEnumerable<Point>
    {
        private readonly Point center;

        public SpiralGenerator(Point center)
        {
            this.center = center;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            Point currentPoint;
            yield return center;
            yield return new Point(center.X + 1, center.Y);
            yield return currentPoint = new Point(center.X + 1, center.Y - 1);
            var iteration = 1;
            while (true)
            {
                var delta = iteration % 2 == 1 ? 1 : -1;
                for (var i = 0; i < iteration; i++)
                {
                    currentPoint = new Point(currentPoint.X - delta, currentPoint.Y);
                    yield return currentPoint;
                }

                for (var i = 0; i < iteration; i++)
                {
                    currentPoint = new Point(currentPoint.X, currentPoint.Y + delta);
                    yield return currentPoint;
                }

                iteration++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

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
        private CircularCloudLayouter layouter;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(3, 4);
            layouter = new CircularCloudLayouter(new Point(3, 4));
        }

        [TearDown]
        public void TearDown()
        {
            var image = new Bitmap(500, 500);
            var graphics = Graphics.FromImage(image);
            foreach (var rectangle in layouter.Rectangles)
            {
                graphics.DrawRectangle(Pens.Blue, rectangle);
            }

            image.Save(@"C:\Users\grygr\Desktop\New folder (2)\t.jpg");
        }

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

        [Test]
        public void PutFirstRectangleToCenter()
        {
            var size = new Size(3, 4);
            layouter.PutNextRectangle(size)
                    .Should()
                    .BeEquivalentTo(new Rectangle(center, size));
        }

        [Test]
        public void AddRectangleToRectangles()
        {
            for (var i = 1; i < 6; i++)
            {
                layouter.PutNextRectangle(new Size(i, 3 * i));
            }

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
                    .And.HaveCount(10);
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

        [TestCase(100, 1000, TestName = "N = 100, M = 1000 ms")]
        [TestCase(100, 1000, TestName = "N = 1000, M = 1000 ms")]
        [TestCase(100, 1000, TestName = "N = 10000, M = 1000 ms")]
        [TestCase(100, 1000, TestName = "N = 100000, M = 1000 ms")]

        public void AddNRectangles_FasterThanMms(int n, int m)
        {
            void Addition()
            {
                var random = new Random(42);

                for (int i = 0; i < n; i++)
                {
                    layouter.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 30)));
                }
            }

            new ExecutionTime(Addition).Should()
                                       .BeLessThan(new TimeSpan(0, 0, 0, 0, m));
            layouter.Rectangles
                    .Should()
                    .HaveCount(n);
        }
    }
}