using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudDrawer : IDisposable
    {
        private readonly SpiralCloudLayouter layouter;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private readonly Brush brush;
        private readonly Pen pen;
        private readonly StringFormat stringFormat;

        public CircularCloudDrawer(Size imageSize, SpiralCloudLayouter layouter)
        {
            bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            this.layouter = layouter;
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Teal);
            brush = Brushes.Peru;
            pen = new Pen(Brushes.Black);
            stringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
            };
        }

        public void DrawWord(string word, Font font)
        {
            var wordSize = graphics.MeasureString(word, font) + new SizeF(1, 1);
            var wordRect = layouter.PutNextRectangle(wordSize.ToSize());
            graphics.DrawString(word, font, brush, wordRect, stringFormat);
            DrawRectangle(wordRect);
        }

        public void DrawRectangle(Rectangle rectangle)
        {
            graphics.DrawRectangle(pen, rectangle);
        }

        public void Save(string filename)
        {
            bitmap.Save(filename);
        }

        public void Dispose()
        {
            bitmap.Dispose();
            graphics.Dispose();
            brush.Dispose();
            stringFormat.Dispose();
        }
    }
}