using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private static readonly Brush Brush = Brushes.Black;
        private CircularCloudLayouter cloudLayouter;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public Visualizer(CircularCloudLayouter cloudLayouter) : this(cloudLayouter.SizeF.ToSize())
        {
            this.cloudLayouter = cloudLayouter;
        }

        public Visualizer(Size size)
        {
            cloudLayouter =
                new CircularCloudLayouter(new PointF(size.Width / 2f, size.Height / 2f), new Spiral(1, 1));
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

        public void DrawRectangles(string file)
        {
            var cloudWithOffsetLocation = cloudLayouter.GetCloud()
                .Select(r =>
                    new RectangleF(
                        new PointF(r.X + cloudLayouter.SizeF.Width / 2, r.Y + cloudLayouter.SizeF.Height / 2),
                        r.Size)).ToArray();
            graphics.DrawRectangles(Pens.Aqua, cloudWithOffsetLocation);
            bitmap.Save(file);
        }
    }
}