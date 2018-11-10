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
            var wigth = sizeTagCloud.Width+100;
            var height = sizeTagCloud.Height + 100;
            var resultBitmap = new Bitmap(wigth, height);
            var graphics = Graphics.FromImage(resultBitmap);
            graphics.TranslateTransform(wigth/2, height/2);
            graphics.FillEllipse(Brushes.DarkOrange, -1, -1, 3,3);
            DrawRectangles(rectangles, graphics);
            return resultBitmap;
        }

        private static void DrawRectangles(Rectangle[] rectangles, Graphics g)
        {
            foreach (var rect in rectangles)
            {
                g.FillRectangle(new SolidBrush(ColorPallete.GetColor()), rect);
            }
               
        }
    }
}
    