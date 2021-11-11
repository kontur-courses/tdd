using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization;
using TagsCloudVisualization.Layouters;

namespace TagCloudUsageSample
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            for (var j = 0; j < 3; j++)
            {
                var layouter = new CircularCloudLayouter(new Point(25, 25));
                
                RectanglePainter
                    .GetBitmapWithRectangles(GetRectangles(layouter, 100))
                    .Save($"..\\..\\CloudTagSample{j}.jpg", ImageFormat.Jpeg);
            }
        }
    
        private static IEnumerable<Rectangle> GetRectangles(CircularCloudLayouter layouter, int count)
        {
            var rnd = new Random();
            
            for (var i = 0; i < count; i++)
            {
                yield return layouter.PutNextRectangle(new Size(count + 25, rnd.Next(10, count + 25 - i * 1)));
            }
        }
    }
}