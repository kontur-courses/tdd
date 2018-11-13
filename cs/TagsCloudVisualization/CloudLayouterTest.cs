using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public abstract class CloudLayouterTest
    {
        protected  readonly Random Rnd = new Random();
        protected CircularCloudLayouter CircularCloudLayouter;
        protected readonly Point CenterPoint = new Point(100, 100);
        protected List<Rectangle> Rectangles = new List<Rectangle>();
        protected IEnumerable<Point> PointsGenerator;

        [SetUp]
        public void SetUp()
        {
            PointsGenerator = new ArchimedesSpiralPointGenerator(CenterPoint, 0.1, 0.0);
            CircularCloudLayouter = new CircularCloudLayouter(CenterPoint, PointsGenerator);
        }

        [TearDown]
        public void TearDown()
        {
            SaveIfFailTest();
        }

        private void SaveIfFailTest()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var testMethodName = TestContext.CurrentContext.Test.MethodName;
                var cloudFilename = $"{testMethodName}.bmp";
                var cloudDirectory = TestContext.CurrentContext.WorkDirectory;
                DrawHandler.DrawRectangles(Rectangles, CenterPoint, cloudFilename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {cloudDirectory}\\{cloudFilename}");
            }
        }

        protected Size GetRandomSize()
        {
            var h = Rnd.Next(30, 100);
            var w = Rnd.Next(50, 200);
            return new Size(w, h);
        }

        protected Point GetRectangleCenter(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        protected List<Rectangle> GenerateRectangles(int countRectangles)
        {
            var result = new List<Rectangle>();
            for (var i = 0; i < countRectangles; i++)
            {
                var size = GetRandomSize();
                var rectangle = CircularCloudLayouter.PutNextRectangle(size);
                result.Add(rectangle);
            }

            return result;
        }

        protected  bool HaveIntersect(List<Rectangle> cloudRectangles)
        {
            foreach (var rectangle1 in cloudRectangles)
            foreach (var rectangle2 in cloudRectangles)
                if (rectangle2 != rectangle1 && rectangle1.IntersectsWith(rectangle2))
                    return true;
            return false;

        }
    }

    class CloudLayouterConstructor : CloudLayouterTest
    {
        [TestCase(1, -2, TestName = "when central point coords x is positive, y is negative")]
        [TestCase(-1, 2, TestName = "when central point coords x is negative, y is positive")]
        [TestCase(-1, -2, TestName = "when central point coords is negative")]
        public void Constructor_ShouldThrowArgumentException(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(
                new Point(x, y), PointsGenerator);

            act.ShouldThrow<ArgumentException>();
        }


        [TestCase(0, 0, TestName = "when central point coords is zero")]
        [TestCase(1, 0, TestName = "when central point coords x is positive, y is zero")]
        [TestCase(0, 2, TestName = "when central point coords x is zero, y is positive")]
        [TestCase(3, 4, TestName = "when central point coords is positive")]
        public void Constructor_DontShouldThrowArgumentException(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(new Point(x, y), PointsGenerator);

            act.ShouldNotThrow<ArgumentException>();
        }

        [Test]
        public void Constructor_ShouldCorrectSetUpCentralPoint()
        {
            var size = GetRandomSize();

            var firstRectangle = CircularCloudLayouter.PutNextRectangle(size);
            var rectangleCenter = GetRectangleCenter(firstRectangle);

            rectangleCenter.ShouldBeEquivalentTo(CenterPoint);
        }
    }

    public class CloudLayouterPutNextRectangle : CloudLayouterTest
    {

        [TestCase(-3, -4, TestName = "when size is negative")]
        [TestCase(-3, 4, TestName = "when size width is negative, height is positive")]
        [TestCase(3, -4, TestName = "when size width is positive, height is negative")]
        public void PutNextRectangle_ShouldThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            Action act = () => CircularCloudLayouter.PutNextRectangle(size);

            act.ShouldThrow<ArgumentException>();
        }


        [TestCase(3, 4, TestName = "when size is positive")]
        [TestCase(0, 0, TestName = "when size is zero")]
        [TestCase(3, 0, TestName = "when size width is positive, height is zero")]
        [TestCase(0, 4, TestName = "when size width is zero, height is positive")]
        public void PutNextRectangle_DontShouldThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            Action act = () => CircularCloudLayouter.PutNextRectangle(size);

            act.ShouldNotThrow<ArgumentException>();
        }


        [Test]
        public void PutRectangle_ShouldReturnRectangleWithTransferredSize()
        {
            var size = GetRandomSize();

            var rectangle = CircularCloudLayouter.PutNextRectangle(size);

            rectangle.Size.ShouldBeEquivalentTo(size);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(50)]
        public void Cloud_ShouldNoHaveIntersectingRectangles(int countRectangles)
        {
            Rectangles = GenerateRectangles(countRectangles);

            HaveIntersect(Rectangles).Should().BeFalse();
        }

        [Test]
        public void Cloud_ShouldBeLikeCircle()
        {
            Rectangles = GenerateRectangles(150);
            var rectanglesArea = Rectangles
                .Select(rect => rect.Width * rect.Height)
                .Sum();
            var circleRadius = 1.5 * Math.Sqrt(rectanglesArea / Math.PI);

            CountPointsOutOfCircle(circleRadius, Rectangles, CenterPoint)
                .Should()
                .HaveCount(0);
        }

        private IEnumerable<Point> CountPointsOutOfCircle(double circleRadius, List<Rectangle> rectangles, Point centerPoint)
        {
            return rectangles.Select(ToBoundaryPoints).SelectMany(point => point).Distinct()
                .Where(x => Math.Pow(centerPoint.X - x.X, 2) + Math.Pow(centerPoint.Y - x.Y, 2) > circleRadius * circleRadius);
        }

        private Point[] ToBoundaryPoints(Rectangle rectangle)
        {
            var x = rectangle.X;
            var y = rectangle.Y;
            var h = rectangle.Height;
            var w = rectangle.Width;
            return new[]
            {
                new Point(x,y),
                new Point(x, y + h),
                new Point(x + w,y),
                new Point(x + w,y + h)
            };
        }
    }
    
    public class CloudLayouterPerformance : CloudLayouterTest
    {
        [Test, Timeout(1000)]
        public void AddALotOfIdenticalRectangles_WithArchimedesSpiralGenerator_TimeOut()
        {
            var size = new Size(50, 50);

            for (int i = 0; i < 100; i++)
            {
                CircularCloudLayouter.PutNextRectangle(size);
            }
        }

        [Test, Timeout(1000)]
        public void AddALotOfIdenticalRectangles_WithCirclePointGenerator_TimeOut()
        {
            var size = new Size(50, 50);
            CircularCloudLayouter = new CircularCloudLayouter(CenterPoint, new CirclePointGenerator(CenterPoint));

            for (int i = 0; i < 100; i++)
            {
                CircularCloudLayouter.PutNextRectangle(size);
            }
          
        }
    }
}
