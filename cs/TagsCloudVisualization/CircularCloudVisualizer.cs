using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer : IDisposable
    {
        private readonly Bitmap bitmap;
        private readonly CircularCloudLayouter layouter;
        private readonly Graphics graphics;

        public Pen RectangleBorderPen { get; set; } = new Pen(Brushes.Black);
        public Brush RectangleFillBrush { get; set; } = Brushes.SlateBlue;
        public Brush TextBrush { get; set; } = Brushes.Gold;

        public CircularCloudVisualizer(CircularCloudLayouter layouter, Size imageSize)
        {
            this.layouter = layouter;
            bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DrawPositionedRectangles()
        {
            foreach (var rectangle in layouter.Rectangles)
            {
                DrawRectangle(rectangle);
            }
        }

        public void DrawText(string text, Font font)
        {
            var textSize = graphics.MeasureString(text, font) + new SizeF(1, 1);
            var rectangle = DrawRectangle(textSize.ToSize());
            graphics.DrawString(text, font, TextBrush, rectangle);
        }

        public Rectangle DrawRectangle(Size size)
        {
            var rectangle = layouter.PutNextRectangle(size);
            DrawRectangle(rectangle);
            return rectangle;
        }

        public void Save(string filename)
        {
            bitmap.Save(filename);
        }

        public void Dispose()
        {
            graphics.Dispose();
            bitmap.Dispose();
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            graphics.FillRectangle(RectangleFillBrush, rectangle);
            graphics.DrawRectangle(RectangleBorderPen, rectangle);
        }
    }
}
