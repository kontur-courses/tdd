using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    internal class TagCloudDrawer
    {
        private Bitmap bitmap;
        private Graphics graphics;

        public string SavePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(),
            "SavedImages", "img.jpg");
        public Pen pen;
        private int scale = 1;

        public TagCloudDrawer(Size bitmapSize, int scale)
        {
            bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(Color.Red, 2);
            this.scale = scale;
        }

        public void DrawRectangle(Rectangle rectangle)
        {
            rectangle.X *= scale;
            rectangle.X -= bitmap.Width / 2 * (scale - 1);
            rectangle.Y *= scale;
            rectangle.Y -= bitmap.Height / 2 * (scale - 1);
            rectangle.Width *= scale;
            rectangle.Height *= scale;
            graphics.FillRectangle(Brushes.Blue, rectangle);
            graphics.DrawRectangle(pen, rectangle);
        }

        public void SaveImage()
        {
            bitmap.Save(SavePath, ImageFormat.Jpeg);
        }
        
    }
}
