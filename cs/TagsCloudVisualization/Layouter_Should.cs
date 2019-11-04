using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Layouter_Should
    {
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(500, 500));
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine(layouter.layout[0]);
            Bitmap bmp = new Bitmap(1000, 1000);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.FillRegion(new SolidBrush(Color.White), new Region(new Rectangle(0, 0, 1000, 1000)));
            Pen pen = new Pen(new SolidBrush(Color.Blue));
            foreach (var rect in layouter.layout)
            {
                graphics.DrawRectangle(pen, rect);
            }

            bmp.Save($"rects{bmpCounter++}.bmp");
        }

        private CircularCloudLayouter layouter;
        private int bmpCounter = 0;


        [Test]
        public void PlaceFirstRectInCentre()
        {
            Size size = new Size(10, 4);
            Rectangle rect = layouter.PutNextRectangle(size);
            rect.Left.Should().Be(495);
            rect.Right.Should().Be(505);
            rect.Top.Should().Be(498);
            rect.Bottom.Should().Be(502);
        }

        [Test]
        public void RectanglesDoNotIntersectEachOther()
        {
            Size size0 = new Size(10, 4);
            Rectangle rect0 = layouter.PutNextRectangle(size0);
            Size size1 = new Size(20, 8);
            Rectangle rect1 = layouter.PutNextRectangle(size1);
            rect0.IntersectsWith(rect1).Should().BeFalse();
        }

        [Test]
        public void RectanglesDoNotIntersectEachOther_OnHundredLongRects()
        {
            Size[] sizes = new Size[100];
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                sizes[i] = new Size(random.Next(30, 200), random.Next(12, 32));
            }

            for (int i = 0; i < 100; i++)
                layouter.PutNextRectangle(sizes[i]);

            for (int i = 0; i < 100; i++)
                layouter.layout.Any(r => r.IntersectsWith(layouter.layout[i]) && !(r == layouter.layout[i])).Should()
                    .BeFalse();
        }

        [Test]
        public void RectanglesDoNotIntersectEachOther_OnHundredShortRects()
        {
            Size[] sizes = new Size[100];
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                sizes[i] = new Size(random.Next(8, 40), random.Next(4, 20));
            }

            for (int i = 0; i < 100; i++)
                layouter.PutNextRectangle(sizes[i]);

            for (int i = 0; i < 100; i++)
                layouter.layout.Any(r => r.IntersectsWith(layouter.layout[i]) && !(r == layouter.layout[i])).Should()
                    .BeFalse();
        }
    }
}