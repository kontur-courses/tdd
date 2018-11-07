using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class DrawHandler
    {
        public static void DrawRectangles(List<Rectangle> rectangles, Point center, string outputFile)
        {
            var bmp = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bmp);
            graphics.FillRectangle(Brushes.Green, new Rectangle(
                center.X,
                center.Y, 3, 3));
            var pen = new Pen(Color.Red, 1);
            rectangles.ForEach(rect => graphics.DrawRectangle(pen, rect));
            bmp.Save(outputFile, ImageFormat.Bmp);
        }

    }
}
