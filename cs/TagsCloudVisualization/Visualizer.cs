using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    public static class Visualizer
    {
        private static Brush brush = Brushes.Black;

        public static void Draw(IEnumerable<(string, Font)> words, Size size, string filename)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            var g = Graphics.FromImage(bitmap);
            var cloudLayouter = new CircularCloudLayouter(new Point(size.Width / 2, size.Height / 2))
                .WithPointGenerator(new Spiral());
            foreach (var (word, font) in words)
            {
                var wordSize = g.MeasureString(word, font).ToSize() + new Size(1, 1);
                var wordRectangle = cloudLayouter.PutNextRectangle(wordSize);
                g.DrawString(word, font, brush, wordRectangle, StringFormat.GenericDefault);
            }

            bitmap.Save(filename);
        }
    }
}