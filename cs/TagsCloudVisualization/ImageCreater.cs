using System;
using System.Collections.Generic;
//using System.Drawing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TagsCloudVisualization
{
    public class ImageCreater
    {
        private readonly int Width;
        private readonly int Height;

        public ImageCreater(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Generate(IEnumerable<Rectangle> items, string directory)
        {
            var image = new Bitmap(Width, Height);
            var g = Graphics.FromImage(image);
            var pen = new Pen(Brushes.AliceBlue, 2);
            var count = 0;
            foreach (var item in items)
            {
                count++;
                g.DrawRectangle(pen, item);
            }

            image.Save(directory, ImageFormat.Jpeg);
            
        }
    }
}
