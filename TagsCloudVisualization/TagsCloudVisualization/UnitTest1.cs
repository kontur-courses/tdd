using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class PlacementTests
    {
        private CircularCloudLayouter layout;

        [SetUp]
        public void SetUp()
        {
            layout = new CircularCloudLayouter(new Point(1500, 1500));
        }

        [Test]
        public void DefaultTest()
        {
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                layout.PutNextRectangle(new Size(r.Next(10, 50), r.Next(1, 10)));
            }

            layout.PutNextRectangle(new Size(10, 10)).Should().Be(new Rectangle(0, 0, 0, 0));
        }


        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                TagCloudDrawer drawer = new TagCloudDrawer(layout);
                drawer.Scale = 10;
                drawer.DrawImage();
                drawer.SaveImage();
            }
        }
    }

    public class DrawingTests
    {
        private TagCloudDrawer drawer;
        private CircularCloudLayouter layout;

        [SetUp]
        public void SetUp()
        {
            layout = new CircularCloudLayouter(new Point(1000, 1000));
            drawer = new TagCloudDrawer(layout);
            drawer.Scale = 10;
        }

        [Test]
        public void DrawDefaultCloud()
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

        }

        [Test]
        public void DrawRandomCloud()
        {
            Random r = new Random();
            for(int i=0;i<500;i++)
            {
                layout.PutNextRectangle(new Size(r.Next(10, 80), r.Next(1, 10)));
            }
        }

        [Test]
        public void DrawGrowingSizeRectanglesCloud()
        {
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                layout.PutNextRectangle(new Size(i, 2));
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                drawer.DrawImage();
                drawer.SaveImage();
            }
        }

    }

    public class SpiralLayoutDrawingTests
    {
        private TagCloudDrawer drawer;
        private SpiralCloudLayout layout;

        [SetUp]
        public void SetUp()
        {
            layout = new SpiralCloudLayout(new Point(400, 150));
            drawer = new TagCloudDrawer(layout);
            drawer.Scale = 10;
        }

        [Test]
        public void DrawSimpleCloud()
        {
            for (int i = 0; i < 500; i++)
            {
                layout.PutNextRectangle(new Size(4, 4));
            }
        }

        [Test]
        public void DrawGrowingSizeRectanglesCloud()
        {
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                layout.PutNextRectangle(new Size(i, 2));
            }
        }

        [Test]
        public void Draw2Rectangles()
        {
            layout.PutNextRectangle(new Size(1, 1));
            layout.PutNextRectangle(new Size(5, 5));
        }

        [Test]
        public void DrawRandomCloud()
        {
            Random r = new Random();
            for (int i = 0; i < 500; i++)
            {
                layout.PutNextRectangle(new Size(r.Next(10, 80), r.Next(1, 10)));
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                drawer.DrawImage();
                drawer.SaveImage();
            }
        }

    }
}