using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void CircularCloudLayouter_ShouldThrowArgumentException_WhenNegativeX_Y()
        {
            Action act = () =>
            {
                var cloudLayouter = new CircularCloudLayouter(new Point(-1, -1));
            };
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleInCenter_WhenOneRectangle()
        {
            var center = new System.Drawing.Point(400, 250);
            var cloudLayouter = new CircularCloudLayouter(center);
            var rectangleSize = new Size(200, 30);
            var point = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            var expectedRect = new Rectangle(point, rectangleSize);

            var rect = cloudLayouter.PutNextRectangle(rectangleSize);

            rect.Should().BeEquivalentTo(expectedRect);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PutNextRectangle_ShouldReturnIntersectedRectangles(bool input)
        {
            var cloudLayouter = new CircularCloudLayouter(new System.Drawing.Point(400, 250),input);

            cloudLayouter.PutNextRectangle(new Size(300, 100));
            cloudLayouter.PutNextRectangle(new Size(100, 31));
            cloudLayouter.PutNextRectangle(new Size(50, 52));
            cloudLayouter.PutNextRectangle(new Size(100, 31));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 21));

            cloudLayouter.Rectangles.AreIntersected().Should().BeFalse();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PutNextRectangle_ShouldArrangeRectanglesInShapeCircle(bool input)
        {
            var center = new System.Drawing.Point(400, 250);
            var cloudLayouter = new CircularCloudLayouter(center, input);
            cloudLayouter.PutNextRectangle(new Size(300, 100));
            cloudLayouter.PutNextRectangle(new Size(100, 31));
            cloudLayouter.PutNextRectangle(new Size(50, 52));
            cloudLayouter.PutNextRectangle(new Size(100, 31));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 21));

            var distanceToExtremePoints = new List<int>();
            distanceToExtremePoints.Add(center.X - cloudLayouter.Rectangles.Min(x => x.Left));
            distanceToExtremePoints.Add(cloudLayouter.Rectangles.Max(x => x.Right) - center.X);
            distanceToExtremePoints.Add(center.Y - cloudLayouter.Rectangles.Min(x => x.Top));
            distanceToExtremePoints.Add(cloudLayouter.Rectangles.Max(x => x.Bottom) - center.Y);

            var avr = distanceToExtremePoints.Average();
            var distMoreAvr = distanceToExtremePoints.Where(x => x > 1.2 * avr || x < 0.8 * avr);
            distMoreAvr.Count()
                .Should().Be(0, "расстояния до крайних точек не должны отличаться от среднего больше, чем на 20%");
        }

        [TestCase(true)]
        [TestCase(false)]
        [Ignore("Ignore a test")]
        public void CreateNewImageCloudLayouter(bool input)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(400, 250),input);
            cloudLayouter.PutNextRectangle(new Size(300, 100));
            cloudLayouter.PutNextRectangle(new Size(100, 31));
            cloudLayouter.PutNextRectangle(new Size(50, 52));
            cloudLayouter.PutNextRectangle(new Size(100, 31));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 21));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 100));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(100, 31));
            cloudLayouter.PutNextRectangle(new Size(100, 30));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(50, 20));
            cloudLayouter.PutNextRectangle(new Size(50, 20));

            var bmp = new Bitmap(800, 500);
            using (Graphics gph = Graphics.FromImage(bmp))
            {
                var blackPen = new Pen(Color.Black, 3);
                foreach (var rect in cloudLayouter.Rectangles)
                {
                    gph.DrawRectangle(blackPen, rect);
                }
                if (input)
                    bmp.Save("CloudLayouterWithOffsetToCenter.bmp");
                else
                    bmp.Save("CloudLayouter.bmp");
            }
        }
    }
}
