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

        public Visualizer(CircularCloudLayouter cloudLayouter) : this(cloudLayouter.Size.ToSize(), Color.Bisque)
        {
            this.cloudLayouter = cloudLayouter;
        }

        public Visualizer(Size size, Color backgroundColor)
        {
            cloudLayouter =
                new CircularCloudLayouter(new Spiral(0.01f, 2, new PointF(size.Width / 2f, size.Height / 2f)));
            bitmap = new Bitmap(size.Width, size.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(new SolidBrush(backgroundColor), 0, 0, size.Width, size.Height);
        }

        public void Draw(IEnumerable<(string, Font)> words, string filename)
        {
            foreach (var (word, font) in words)
                DrawString(word, font);
            bitmap.Save(filename);
        }

        private void DrawString(string word, Font font)
        {
            var wordSize = graphics.MeasureString(word, font).ToSize() + new Size(1, 1);
            var wordRectangle = cloudLayouter.PutNextRectangle(wordSize);
            graphics.DrawString(word, font, Brush, wordRectangle, StringFormat.GenericDefault);
        }

        public void DrawRectangles(string file)
        {
            var cloudWithOffsetLocation = cloudLayouter.GetCloud()
                .Select(r =>
                    new RectangleF(
                        new PointF(r.X + cloudLayouter.Size.Width / 2, r.Y + cloudLayouter.Size.Height / 2),
                        r.Size)).ToArray();
            graphics.DrawRectangles(Pens.Aqua, cloudWithOffsetLocation);
            bitmap.Save(file);
        }

        public void Draw(Template template, string filename)
        {
            foreach (var wordParameter in template.GetWords())
            {
                var offset = new PointF(template.Size.Width / 2f, template.Size.Height / 2f);
                var rectangleF = wordParameter.WordRectangleF;
                rectangleF.Offset(offset);
                graphics.DrawString(wordParameter.Word, wordParameter.Font, Brush, rectangleF);
            }

            bitmap.Save(filename);
        }
    }
}