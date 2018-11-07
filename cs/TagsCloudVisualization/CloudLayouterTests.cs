using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CloudLayouterTests
    {
        private readonly Random rnd = new Random();
        private CircularCloudLayouter circularCloudLayouter;
        private readonly Point centerPoint = new Point(100, 100);
        private List<Rectangle> rectangles = new List<Rectangle>();

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {

                var testMethodName = TestContext.CurrentContext.Test.MethodName;
                var cloudFilename = $"{testMethodName}.bmp";
                var cloudDirectory = TestContext.CurrentContext.WorkDirectory;
                DrawHandler.DrawRectangles(rectangles, centerPoint, cloudFilename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {cloudDirectory}\\{cloudFilename}");
            }
        }


        [TestCase(1, -2, TestName = "when central point coords x is positive, y is negative")]
        [TestCase(-1, 2, TestName = "when central point coords x is negative, y is positive")]
        [TestCase(-1, -2, TestName = "when central point coords is negative")]
        public void ConstructorThrowArgumentException(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(new Point(x, y));

            act.ShouldThrow<ArgumentException>();
        }


        [TestCase(0, 0, TestName = "when central point coords is zero")]
        [TestCase(1, 0, TestName = "when central point coords x is positive, y is zero")]
        [TestCase(0, 2, TestName = "when central point coords x is zero, y is positive")]
        [TestCase(3, 4, TestName = "when central point coords is positive")]
        public void ConstructorDontThrowArgumentException(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(new Point(x, y));

            act.ShouldNotThrow<ArgumentException>();
        }


        [Test]
        public void CorrectSetUpCentralPoint()
        {
            var size = GetRandomSize();
            circularCloudLayouter = new CircularCloudLayouter(centerPoint);
                        
            var firstRectangle = circularCloudLayouter.PutNextRectangle(size);
            var rectangleCenter = GetRectangleCenter(firstRectangle);

            rectangleCenter.ShouldBeEquivalentTo(centerPoint);
        }


        [TestCase(-3, -4, TestName = "when size is negative")]
        [TestCase(-3, 4, TestName = "when size width is negative, height is positive")]
        [TestCase(3, -4, TestName = "when size width is positive, height is negative")]
        public void PutNextRectangeThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);
            circularCloudLayouter = new CircularCloudLayouter(centerPoint);

            Action act = () => circularCloudLayouter.PutNextRectangle(size);

            act.ShouldThrow<ArgumentException>();
        }


        [TestCase(3, 4, TestName = "when size is positive")]
        [TestCase(0, 0, TestName = "when size is zero")]
        [TestCase(3, 0, TestName = "when size width is positive, height is zero")]
        [TestCase(0, 4, TestName = "when size width is zero, height is positive")]
        public void PutNextRectangeDontThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);
            circularCloudLayouter = new CircularCloudLayouter(centerPoint);

            Action act = () => circularCloudLayouter.PutNextRectangle(size);

            act.ShouldNotThrow<ArgumentException>();
        }


        [Test]
        public void PutRectangle_ShouldNotChangeSize()
        {
            var size = GetRandomSize();
            circularCloudLayouter = new CircularCloudLayouter(centerPoint);

            var rectangle = circularCloudLayouter.PutNextRectangle(size);

            rectangle.Size.ShouldBeEquivalentTo(size);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(50)]
        public void NoIntersectingRectanglesCloud(int countRectangles)
        {
            circularCloudLayouter = new CircularCloudLayouter(centerPoint);
            rectangles = new List<Rectangle>();
            for (var i = 0; i < countRectangles; i++)
            {
                var size = GetRandomSize();
                var rectangle = circularCloudLayouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }

            var intersect = HaveIntersect(rectangles);

            intersect.Should().BeFalse();
        }


        private Size GetRandomSize()
        {
            var h = rnd.Next(30, 100);
            var w = rnd.Next(50, 200);
            return new Size(w, h);
        }

        private Point GetRectangleCenter(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        private bool HaveIntersect(List<Rectangle> cloudRectangles)
        {
            foreach (var rectangle1 in cloudRectangles)
                foreach (var rectangle2 in cloudRectangles)
                    if (rectangle2 != rectangle1 && rectangle1.IntersectsWith(rectangle2))
                        return true;
            return false;
                        
        }
    }
}