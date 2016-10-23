using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Picture
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private readonly SolidBrush myBrush;
        private readonly float maxFontSize;
        private readonly StringFormat drawFormat;
        private readonly CircularCloudLayouter cloud;


        public Picture(int width, int height)
        {
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
            graphics.DrawRectangle(new Pen(Color.Aqua), rectangle);
        }

        public void SaveToFile(string filename)
        {
            bitmap.Save(filename);
        }

    }
}