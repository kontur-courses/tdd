using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class TagCloudImage
    { 
        public static Bitmap GenerateTagCloudBitmap(Rectangle[] rectangles, Size sizeTagCloud)
        {
            var wigth = sizeTagCloud.Width * 2;
            var height = sizeTagCloud.Height*2;
            var resultBitmap = new Bitmap(wigth, height);
            var graphics = Graphics.FromImage(resultBitmap);
            graphics.TranslateTransform(wigth/2, height/2);           
            DrawRectangles(rectangles, graphics);
            graphics.FillEllipse(Brushes.Red, -1, -1, 3, 3);
            graphics.DrawEllipse(Pens.Red, new Rectangle(-sizeTagCloud.Width/2, -sizeTagCloud.Height/2,sizeTagCloud.Width, sizeTagCloud.Height));
            return resultBitmap;
        }

        private static void DrawRectangles(Rectangle[] rectangles, Graphics g)
        {
            foreach (var rect in rectangles)
            {
                g.FillRectangle(new SolidBrush(ColorPallete.GetColor()), rect);
                g.DrawRectangle(new Pen(Color.Black, 4), rect);
            }
               
        }
    }
}
    