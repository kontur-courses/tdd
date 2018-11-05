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
        private Size GetRandomSize()
        {
            var h = rnd.Next(30, 100);
            var w = rnd.Next(50, 200);
            return new Size(w, h);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {

                var testMethodName = TestContext.CurrentContext.Test.MethodName;
                var cloudFilename = $"{testMethodName}.bmp";
                var cloudDirectory = TestContext.CurrentContext.WorkDirectory;
                DrawHandler.DrawRectangles(circularCloudLayouter, cloudFilename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {cloudDirectory}\\{cloudFilename}");
            }
        }

        [TestCase(-1, -2, TestName = "when central point coords is negative")]
        public void ConstructorThrowArgumentException(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(new Point(x, y));
            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void CorrectSetUpCentralPoint()
        {
            var point = new Point(2, 4);
            circularCloudLayouter = new CircularCloudLayouter(point);
            circularCloudLayouter.Center.ShouldBeEquivalentTo(point);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(150)]
        public void CheckCorrect_WhenPutAnyRectangles(int countRectangles)
        {
            var point = new Point(100, 100);
            circularCloudLayouter = new CircularCloudLayouter(point);
            var rectagles = new List<Rectangle>();
            var sizes = new List<Size>();

            for (int i = 0; i < countRectangles; i++)
            {
                var size = GetRandomSize();
                sizes.Add(size);
                var rect = circularCloudLayouter.PutNextRectangle(size);
                rectagles.Add(rect);
            }

            CheckCorrectLayouter(circularCloudLayouter, sizes, rectagles);
        }

        private void CheckCorrectLayouter(CircularCloudLayouter layouter, List<Size> sizes, List<Rectangle> rectangles)
        {
            layouter.Rectangles.Should().HaveCount(sizes.Count);

            foreach (var rectangle in layouter.Rectangles)
                sizes.Should().Contain(rectangle.Size);
            
            var x = rectangles.Zip(layouter.Rectangles, (r1, r2) => new List<Rectangle>{r1, r2});
            foreach (var list in x)
            {
                list[0].ShouldBeEquivalentTo(list[1]);
            }

            foreach (var rectangle1 in layouter.Rectangles)
            foreach (var rectangle2 in layouter.Rectangles)
                if (rectangle2 != rectangle1)
                    rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
        }
    }
}