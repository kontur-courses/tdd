using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    public static class Visualizer
    {
        private static Brush brush = Brushes.Black;
        private static Font defaultFont = new Font("Arial", 15);
        public static void Draw(IEnumerable<(string, Font)> words, Size size, string filename)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            var g = Graphics.FromImage(bitmap);
            var cloudLayouter = new CircularCloudLayouter(new Point(size.Width/2, size.Height/2));
            foreach (var word in words)
            {
                var wordSize = g.MeasureString(word.Item1, word.Item2).ToSize() + new Size(1,1);
                var wordRectangle = cloudLayouter.PutNextRectangle(wordSize);
                g.DrawString(word.Item1, word.Item2, brush, wordRectangle, StringFormat.GenericDefault);
            }
            bitmap.Save(filename);
        }

    }
}