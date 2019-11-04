using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Tests
{
    internal class OnFailDrawer
    {
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

        internal static void DrawOriginOrientedRectangles(Size drawerSize, IEnumerable<Rectangle> rectangles, string fname)
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