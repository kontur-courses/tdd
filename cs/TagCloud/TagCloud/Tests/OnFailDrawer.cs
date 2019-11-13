using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagCloud.Tests
{
    internal class OnFailDrawer
    {
        private int imgWidth;
        private int imgHeight;
        protected CircularCloudLayouter cloudLayouter;

        public OnFailDrawer(int imgWidth = 1200, int imgHeight = 1200)
        {
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
        }

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(TestingDegenerateSize.CenterPoint);
        }

        [TearDown]
        protected void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            var fname = $"{TestContext.CurrentContext.Test.FullName}.png";
            DrawOriginOrientedRectangles(
                new Size(imgWidth, imgHeight),
                cloudLayouter
                    .GetAllRectangles()
                    .Select(rect => new Rectangle(imgWidth / 2 + rect.X, imgHeight / 2 + rect.Y, rect.Width, rect.Height)),
                fname);
            TestContext.WriteLine($"Tag cloud visualisation saved to file: '{fname}'");
        }
        
        internal static List<Color> colors = new List<Color>
        {
            Color.Yellow,
            Color.Blue,
            Color.Green,
            Color.Red,
            Color.Azure,
            Color.Chocolate,
            Color.Fuchsia,
            Color.BlueViolet,
            Color.DarkOrange,
            Color.Cyan,
            Color.Bisque,
            Color.Thistle,
            Color.DeepPink
        };
        
        public static void DrawOriginOrientedRectangles(Size drawerSize, IEnumerable<Rectangle> rectangles, string fname)
        {
            var rand = new Random();
            using (var drawer = new Drawer(drawerSize))
            {
                foreach (var rectangle in rectangles)
                {
                    var randColorBrush = new SolidBrush(colors[rand.Next(0, colors.Count)]);
                    drawer.DrawRectangle(rectangle, randColorBrush);
                }

                drawer.SaveImg(fname);
            }
        }
    }
}