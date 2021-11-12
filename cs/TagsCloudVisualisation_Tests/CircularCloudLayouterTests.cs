using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private class CachedCircularLayouter : CircularCloudLayouter
        {
            public IReadOnlyList<Rectangle> Rectangles => rectangles;

            private readonly List<Rectangle> rectangles = new();
            public CachedCircularLayouter(Point center) : base(center)
            { }

            public new Rectangle PutNextRectangle(Size size)
            {
                var rectangle = base.PutNextRectangle(size);
                rectangles.Add(rectangle);
                return rectangle;
            }
        }

        private CachedCircularLayouter layouter;

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status == TestStatus.Failed && layouter != null)
            {
                if (!Directory.Exists("testresults"))
                    Directory.CreateDirectory("testresults");
                var rectangles = layouter.Rectangles;
                Visualizer.GetBitmapFromRectangles(rectangles.ToArray(), true, 100).Save($"testresults/{context.Test.Name}.png", ImageFormat.Png);
                TestContext.Out.WriteLine($"Tag cloud visualization saved to file <testresults/{context.Test.Name}.png>");
            }
        }

        [Test]
        public void ShouldHaveSetCenter_OnCreation()
        {
            var expectedCenter = new Point(10, 11);
            layouter = new CachedCircularLayouter(expectedCenter);
            layouter.Center.Should().Be(expectedCenter);
        }

        [Test]
        public void ShouldReturnRectangleWithSetSize()
        {
            layouter = new CachedCircularLayouter(Point.Empty);
            var expectedSize = new Size(10, 11);

            var resulRectangle = layouter.PutNextRectangle(expectedSize);

            resulRectangle.Size.Should().Be(expectedSize);
        }

        [TestCase(0, 2)]
        [TestCase(0, 0)]
        [TestCase(2, 0)]
        public void ShouldThrow_WhenTryingToPutDegenerateRectangle(int width, int height)
        {
            layouter = new CachedCircularLayouter(Point.Empty);
            Action action = () => layouter.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestCase(0, 0)]
        [TestCase(10, 11)]
        public void FirstRectangle_ShouldHaveSameCenter(int x, int y)
        {
            var expectedCenter = new Point(x, y);
            layouter = new CachedCircularLayouter(expectedCenter);

            var resultRectangle = layouter.PutNextRectangle(new Size(10, 11));
            var resultCenter = resultRectangle.GetCenter();

            resultCenter.Should().Be(expectedCenter);
        }

        [Test]
        public void SecondRectangle_ShouldBePlacedNextToFirst()
        {
            layouter = new CachedCircularLayouter(Point.Empty);

            var firstRectangle = layouter.PutNextRectangle(new Size(10, 11));
            var secondRectangle = layouter.PutNextRectangle(new Size(11, 10));

            secondRectangle.Should()
                .Match<Rectangle>(second => IsNextToEachOther(firstRectangle, second));
        }

        private static bool IsNextToEachOther(Rectangle first, Rectangle second)
        {
            return first.Bottom == second.Top
                   || first.Top == second.Bottom
                   || first.Left == second.Right
                   || first.Right == second.Left;
        }

        [Test]
        public void SecondRectangle_ShouldBePlacedAsCloseToCenterAsPossible()
        {
            var center = Point.Empty;
            layouter = new CachedCircularLayouter(center);
            var firstSize = new Size(10, 11);
            var secondSize = new Size(10, 11);

            var maximumExpectedDistance = GetClosestDistance(firstSize, secondSize);

            layouter.PutNextRectangle(firstSize);
            var secondRectangle = layouter.PutNextRectangle(secondSize);
            var actualDistance = center.DistanceTo(secondRectangle.GetCenter());

            actualDistance.Should().BeLessOrEqualTo(maximumExpectedDistance + 1);
        }

        [Test]
        public void ThirdRectangle_ShouldBePlacedAsCloseToCenterAsPossible()
        {
            var center = Point.Empty;
            layouter = new CachedCircularLayouter(center);
            var firstSize = new Size(100, 11);
            var secondSize = new Size(10, 11);
            var thirdSize = new Size(20, 13);

            var maximumExpectedDistance = GetClosestDistance(firstSize, thirdSize);


            layouter.PutNextRectangle(firstSize);
            layouter.PutNextRectangle(secondSize);
            var thirdRectangle = layouter.PutNextRectangle(thirdSize);
            var actualDistance = center.DistanceTo(thirdRectangle.GetCenter());

            actualDistance.Should().BeLessOrEqualTo(maximumExpectedDistance + 1);
        }

        private static double GetClosestDistance(Size origin, Size toBePlaced)
        {
            bool isHorizontal;
            int minimalSide;
            if (origin.Height > origin.Width)
            {
                isHorizontal = true;
                minimalSide = origin.Width;
            }
            else
            {
                isHorizontal = false;
                minimalSide = origin.Height;
            }
            return Math.Ceiling(minimalSide / 2d + (isHorizontal ? toBePlaced.Width : toBePlaced.Height) / 2d);
        }

        [TestCase(3)]
        [TestCase(300)]
        public void EveryRectangle_ShouldNotIntersectWithEachOther(int count)
        {
            layouter = new CachedCircularLayouter(Point.Empty);
            var size = new Size(10, 11);

            for (var i = 0; i < count; i++)
            {
                layouter.PutNextRectangle(size);
            }

            var resultRectangles = layouter.Rectangles.Select((rectangle, i) => (rectangle, i));

            resultRectangles.Should()
                .OnlyContain(rectangle => resultRectangles
                    .Where(x => x.i != rectangle.i)
                    .All(x => !x.rectangle.IntersectsWith(rectangle.rectangle)),
                "every rectangle should not intersect with others");
        }

        [TestCase(100, 50)]
        [TestCase(1000, 50)]
        public void CloudForm_ShouldBeCloseToCircle(int count, int maxSize)
        {
            var center = Point.Empty;
            layouter = new CachedCircularLayouter(center);
            var rnd = new Random();

            var radius = 0d;
            var area = 0;
            for (var i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next(maxSize) + 1, rnd.Next(maxSize) + 1);
                area += size.GetArea();
                var rectangle = layouter.PutNextRectangle(size);
                var maxDistance = rectangle.GetCenter().DistanceTo(center);
                if (maxDistance > radius)
                    radius = maxDistance;
            }

            var mainCircleArea = Math.PI * radius * radius;
            var resultingDensity = area / mainCircleArea;

            resultingDensity.Should().BeGreaterThan(0.7);
        }

        [TestCase(20)]
        public void PuttingRectanglesShouldBeFast(int seconds)
        {
            layouter = new CachedCircularLayouter(Point.Empty);
            var size = new Size(10, 11);

            Action action = () =>
              {
                  for (var i = 0; i < 1300; i++)
                  {
                      layouter.PutNextRectangle(size);
                  }
              };

            action.ExecutionTime().Should().BeLessOrEqualTo(new TimeSpan(0, 0, 0, seconds));
        }
    }
}
