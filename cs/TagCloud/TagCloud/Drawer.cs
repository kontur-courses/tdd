using System;
using System.Drawing;

namespace TagCloud
{
    public class Drawer
    {
        private readonly CircularCloudLayouter _layouter;
        
        private readonly Bitmap bitmap;
        public readonly StringFormat stringFormat;
        public readonly Brush brush;
        public readonly Pen pen;
        
        public Drawer(Size imgSize)
        {
            stringFormat = new StringFormat {LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center};
            brush = Brushes.Bisque;
            pen = new Pen( Color.Bisque,3.0f );
            _layouter = new CircularCloudLayouter(new Point(imgSize.Width / 2, imgSize.Height / 2));
            
            bitmap = new Bitmap(imgSize.Width, imgSize.Height);
            Graphics.FromImage(bitmap).Clear(Color.Black);
        }

        public void DrawWord(string word, Font font)
        {
            var graphics = Graphics.FromImage(bitmap);
            var size = graphics.MeasureString(word, font);
            var rect = _layouter.PutNextRectangle(size.ToSize());
            graphics.DrawString(word, font, brush, rect, stringFormat);
        }

        public void DrawRectangle(Rectangle rectangle, Brush brush)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(brush, rectangle);
        }

        public void SaveImg(string fname)
        {
            bitmap.Save(fname);
        }
    }
}