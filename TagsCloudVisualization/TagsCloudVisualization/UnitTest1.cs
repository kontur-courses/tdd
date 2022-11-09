using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TestsWithFreeRectangles
    {
        private CircularCloudLayouter layout;

        [SetUp]
        public void Setup()
        {
            layout = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void FillsFreeRectangle_Created_BySmallRectangle()
        {
            layout.PutNextRectangle(new Size(2, 2));
            layout.PutNextRectangle(new Size(1, 1));
            layout.PutNextRectangle(new Size(3, 1));
            layout.PutNextRectangle(new Size(1, 1))
                .Should().Be(new Rectangle(1, 0, 1, 1));
        }

        [Test]
        public void DoesNotHave_FreeRectangle_AfterFillingIt()
        {
            layout.PutNextRectangle(new Size(2, 2));
            layout.PutNextRectangle(new Size(1, 1));
            layout.PutNextRectangle(new Size(1, 1));
            layout.PutNextRectangle(new Size(1, 1))
                .Should().Be(new Rectangle(1, 1, 1, 1));
        }

        [Test]
        public void UltimateTest()
        {
            layout.PutNextRectangle(new Size(6, 2));
            layout.PutNextRectangle(new Size(2, 4));
            layout.PutNextRectangle(new Size(3, 2));
            layout.PutNextRectangle(new Size(6, 1));
            layout.PutNextRectangle(new Size(3, 2));
            layout.PutNextRectangle(new Size(2, 2));
            layout.PutNextRectangle(new Size(8, 2));

            layout.PutNextRectangle(new Size(2, 4));
            layout.PutNextRectangle(new Size(2, 1));
            layout.PutNextRectangle(new Size(3, 2));
            layout.PutNextRectangle(new Size(2, 3));
            layout.PutNextRectangle(new Size(14, 1));
            layout.PutNextRectangle(new Size(1, 7));
            layout.PutNextRectangle(new Size(1, 5));
            layout.PutNextRectangle(new Size(16, 1));
            layout.PutNextRectangle(new Size(1, 8))
                .Should().Be(new Rectangle(7, -3, 1, 8));
        }

    }

    public class DrawingTests
    {
        private TagCloudDrawer drawer;
        private CircularCloudLayouter layout;

        [SetUp]
        public void SetUp()
        {
            drawer = new TagCloudDrawer(new Size(2000, 2000), 10);
            layout = new CircularCloudLayouter(new Point(1000, 1000));
        }

        [Test]
        public void DrawDefaultCloud()
        {
            List<Rectangle> rects = new List<Rectangle>(new[]
            {
                layout.PutNextRectangle(new Size(6, 2)),
                layout.PutNextRectangle(new Size(2, 4)),
                layout.PutNextRectangle(new Size(3, 2)),
                layout.PutNextRectangle(new Size(6, 1)),
                layout.PutNextRectangle(new Size(3, 2)),
                layout.PutNextRectangle(new Size(2, 2)),
                layout.PutNextRectangle(new Size(8, 2)),

                layout.PutNextRectangle(new Size(2, 4)),
                layout.PutNextRectangle(new Size(2, 1)),
                layout.PutNextRectangle(new Size(3, 2)),
                layout.PutNextRectangle(new Size(2, 3)),
                layout.PutNextRectangle(new Size(14, 1)),
                layout.PutNextRectangle(new Size(1, 7)),
                layout.PutNextRectangle(new Size(1, 5)),
                layout.PutNextRectangle(new Size(16, 1)),
                layout.PutNextRectangle(new Size(1, 8))
            });

            foreach (var r in rects)
            {
                drawer.DrawRectangle(r);
            }
            drawer.SaveImage();
        }

        [Test]
        public void DrawRandomCloud()
        {
            Random r = new Random();
            for(int i=0;i<100;i++)
            {
                drawer.DrawRectangle(layout.PutNextRectangle(new Size(r.Next(10, 20), r.Next(1, 10))));
            }
            drawer.SaveImage();
        }


    }
}