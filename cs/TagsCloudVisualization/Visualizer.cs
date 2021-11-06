using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.IO;

namespace TagsCloudVisualization
{
    public static class Visualizer
    {
        private static Brush brush = Brushes.Black;
        private static Font defaultFont = new Font("Arial", 10f);
        public static void Draw(IEnumerable<Rectangle> cloud, Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            var g = Graphics.FromImage(bitmap);
            foreach (var tag in cloud)
            {
                g.DrawString("word", defaultFont, brush, tag.Location);
            }
            bitmap.Save("1.png");
        }

        private static void DrawTag(Rectangle tagRectangle, string word, Graphics g)
        {
           // g.DrawRectangle(Pens.Transparent, tagRectangle);
            g.DrawString(word, defaultFont, brush, tagRectangle.Location);
        }
    }
}