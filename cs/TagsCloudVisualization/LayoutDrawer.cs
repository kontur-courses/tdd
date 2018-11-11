using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public static class LayoutDrawer
    {
        public static Bitmap DrawCloud(this IEnumerable<Tuple<string,int>> wordWeightPairs,
            Size bitmapSize, Func<Size,Rectangle> rectanglePutter, 
            string fontName = "NewTimesRoman",Brush brush = null)
        {
            brush = brush ?? Brushes.Black;
            var map = new Bitmap(bitmapSize.Width,bitmapSize.Height);
            using (var g = Graphics.FromImage(map))
            {
                g.FillRegion(Brushes.White, g.Clip);
                foreach (var tuple in wordWeightPairs)
                {
                    var font = new Font(fontName, tuple.Item2);
                    var rectangleSize = g.MeasureString(tuple.Item1, font).ToSize();
                    var rect = rectanglePutter(rectangleSize);
                    g.DrawString(tuple.Item1, font, brush, rect);
                }
            }
                
            return map;
        }
    }
}