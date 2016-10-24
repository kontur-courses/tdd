using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Picture
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private readonly SolidBrush myBrush;
        private readonly float maxFontSize;
        private readonly StringFormat drawFormat;
        private readonly CircularCloudLayouter cloud;
        private readonly int shiftX;
        private readonly int shiftY;
        private readonly int width;
        private readonly int height;

        public Picture(CircularCloudLayouter cloud)
        {
            this.cloud = cloud;
            shiftX = -cloud.Min(rect => rect.Left);
            shiftY = -cloud.Min(rect => rect.Top);
            width = cloud.Max(rect => rect.Right) + shiftX;
            height = cloud.Max(rect => rect.Top) + shiftY;
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Beige);
            myBrush = new SolidBrush(Color.Brown);
            maxFontSize = (float)(height + width) / 50;
            drawFormat = new StringFormat();
        }

        public Picture(int width, int height)
        {
            this.width = width;
            this.height = height;
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Beige);
            myBrush = new SolidBrush(Color.Brown);
            maxFontSize = (float)(height + width) / 50;
            drawFormat = new StringFormat();
            cloud = new CircularCloudLayouter(new Point(width / 2, height / 2));
        }

        public void DrawPhrase(string phrase, double coefficientFontSize)
        {
            var emSize = maxFontSize * coefficientFontSize;
            var font = new Font(FontFamily.GenericSerif, (float)emSize, FontStyle.Regular);
            var sizeF = graphics.MeasureString(phrase, font);
            var size = new Size(
                (int)Math.Ceiling(sizeF.Width) + 1,
                (int)Math.Ceiling(sizeF.Height) + 1);
            var rectangle = cloud.PutNextRectangle(size);
            graphics.DrawString(
                phrase,
                font,
                myBrush,
                rectangle,
                drawFormat
            );
        }

        public void DrawRectanglesFromCloud()
        {
            foreach (var rectangle in cloud)
            {
                var rectangleForDrawing = new Rectangle(
                    new Point(rectangle.X + shiftX, 
                    rectangle.Y + shiftY),
                    rectangle.Size);
                graphics.DrawRectangle(new Pen(Color.Crimson), rectangleForDrawing);
            }
        }

        public void SaveToFile(string filename)
        {
            bitmap.Save(filename);
        }

    }
}