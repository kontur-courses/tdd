using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private static readonly Brush Brush = Brushes.Black;
        private readonly CircularCloudLayouter cloudLayouter;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public Visualizer(Size size)
        {
            cloudLayouter =
                new CircularCloudLayouter(new PointF(size.Width / 2f, size.Height / 2f), new Spiral(0.6f, 0.3f));
            bitmap = new Bitmap(size.Width, size.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void Draw(IEnumerable<(string, Font)> words, string filename)
        {
            foreach (var (word, font) in words)
                DrawString(word, font);
            bitmap.Save(filename);
        }

        private void DrawString(string word, Font font)
        {
            var wordSize = graphics.MeasureString(word, font).ToSize();
            var wordRectangle = cloudLayouter.PutNextRectangle(wordSize);
            graphics.DrawString(word, font, Brush, wordRectangle, StringFormat.GenericDefault);
        }
    }
}