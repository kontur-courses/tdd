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
        public void DrawRectangles(CircularCloudLayouter circularCloudLayouter, string outputFile)
        {
            var bmp = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bmp);
            graphics.FillRectangle(Brushes.Green, new Rectangle(
                circularCloudLayouter.Center.X,
                circularCloudLayouter.Center.Y, 3, 3));
            var pen = new Pen(Color.Red, 1);
            circularCloudLayouter.Rectangles.ForEach(rect => graphics.DrawRectangle(pen, rect));
            bmp.Save("test2.bmp", ImageFormat.Bmp);
        }

    }
}
