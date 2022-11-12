using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(new PointF(400, 250));
        }

        [Test]
        public void CircularCloudLayouter_ShouldThrowArgumentException_WhenNegativeX_Y()
        {
            Action act = () =>
            {
                var circularCloudLayouter = new CircularCloudLayouter(new Point(-1, -1));
            };
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleInCenter_WhenOneRectangle()
        {
            var center = cloudLayouter.Center;
            var rectangleSizeF = new SizeF(200, 30);
            var point = new PointF(center.X - rectangleSizeF.Width / 2, center.Y - rectangleSizeF.Height / 2);
            var expectedRect = new RectangleF(point, rectangleSizeF);

            var rect = cloudLayouter.PutNextRectangle(rectangleSizeF);

            rect.Should().BeEquivalentTo(expectedRect);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PutNextRectangle_ShouldReturnIntersectedRectangles(bool input)
        {
            if (input)
                cloudLayouter.IsOffsetToCenter = true;
            cloudLayouter.PutNextRectangle(new SizeF(300, 100));
            cloudLayouter.PutNextRectangle(new SizeF(100, 31));
            cloudLayouter.PutNextRectangle(new SizeF(50, 52));
            cloudLayouter.PutNextRectangle(new SizeF(100, 31));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 21));

            cloudLayouter.Rectangles.AreIntersected().Should().BeFalse();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PutNextRectangle_ShouldArrangeRectanglesInShapeCircle(bool input)
        {
            if (input)
                cloudLayouter.IsOffsetToCenter = true;
            var center = cloudLayouter.Center;
            cloudLayouter.PutNextRectangle(new SizeF(300, 100));
            cloudLayouter.PutNextRectangle(new SizeF(100, 31));
            cloudLayouter.PutNextRectangle(new SizeF(50, 52));
            cloudLayouter.PutNextRectangle(new SizeF(100, 31));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 21));

            var distanceToExtremePoints = new List<float>();
            distanceToExtremePoints.Add(center.X - cloudLayouter.Rectangles.Min(x => x.Left));
            distanceToExtremePoints.Add(cloudLayouter.Rectangles.Max(x => x.Right) - center.X);
            distanceToExtremePoints.Add(center.Y - cloudLayouter.Rectangles.Min(x => x.Top));
            distanceToExtremePoints.Add(cloudLayouter.Rectangles.Max(x => x.Bottom) - center.Y);

            var avr = distanceToExtremePoints.Average();
            var distMoreAvr = distanceToExtremePoints.Where(x => x > 1.2 * avr || x < 0.8 * avr);
            distMoreAvr.Count()
                .Should().Be(0, "расстояния до крайних точек не должны отличаться от среднего больше, чем на 20%");
            //если поменять строки на закоменченные, можно проверить, что задача 3 работает
            //var distMoreAvr = distanceToExtremePoints.Where(x => x > 1.1 * avr || x < 0.9 * avr);
            //distMoreAvr.Count()
            //    .Should().Be(0, "расстояния до крайних точек не должны отличаться от среднего больше, чем на 10%");
        }

        [TestCase(true)]
        [TestCase(false)]
        //[Ignore("Ignore a test")]
        public void CreateNewImageCloudLayouter(bool input)
        {
            if (input)
                cloudLayouter.IsOffsetToCenter = true;
            cloudLayouter.PutNextRectangle(new SizeF(300, 100));
            cloudLayouter.PutNextRectangle(new SizeF(100, 31));
            cloudLayouter.PutNextRectangle(new SizeF(50, 52));
            cloudLayouter.PutNextRectangle(new SizeF(100, 31));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 21));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 100));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(100, 31));
            cloudLayouter.PutNextRectangle(new SizeF(100, 30));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));
            cloudLayouter.PutNextRectangle(new SizeF(50, 20));

            cloudLayouter.SaveBitmap(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;

            if (Equals(testResult, ResultState.Failure) ||
                Equals(testResult == ResultState.Error))
            {
                cloudLayouter.SaveBitmap(TestContext.CurrentContext.Test.Name);
                Console.WriteLine("Tag cloud visualization saved to file " + Environment.CurrentDirectory 
                                                                           + "\\" + TestContext.CurrentContext.Test.Name+ ".bmp");
            }
        }

    }
}
