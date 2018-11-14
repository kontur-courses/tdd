using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public static class CircleCloudDrawer
    {
        
        public static Bitmap Draw(IEnumerable<(string word,int fontSize)> wordWeightPairs,Size bitmapSize,
            ICloudLayouter layouter, string fontName = "NewTimesRoman", Brush brush = null)
        {
            brush = brush ?? Brushes.Black;
            var map = new Bitmap(bitmapSize.Width,bitmapSize.Height);
            using (var g = Graphics.FromImage(map))
            {
                g.FillRegion(Brushes.White, g.Clip);
                foreach (var t in wordWeightPairs)
                {
                    var font = new Font(fontName, t.fontSize);
                    var rectangleSize = g.MeasureString(t.word, font).ToSize();
                    var rect = layouter.PutNextRectangle(rectangleSize);
                    g.DrawString(t.word, font, brush, rect);
                }
            }
            return map;
        }
    }
}