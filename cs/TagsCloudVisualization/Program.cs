using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Program
    {
        private static Random rand = new Random(1);
        
        private static readonly Size[] _layout1 = Enumerable.Range(0, 200)
            .Select(i => new Size(rand.Next(300, 1800), rand.Next(150, 400)))
            .ToArray();
        
        private static readonly Size[] _layout2 = Enumerable.Range(0, 500)
            .Select(i => new Size(rand.Next(300, 1800), rand.Next(150, 400)))
            .ToArray();
        
        static void Main(string[] args)
        {
            var centerPoint = new Point(0, 0);
            var circularLayouter = new CircularCloudLayouter(centerPoint);
            var basePointsLayouter = new BasePointsCloudLayouter(centerPoint);
            
            DrawLayoutResult(circularLayouter, _layout1, "circularLayouter_layout_1");
            DrawLayoutResult(circularLayouter, _layout2, "circularLayouter_layout_2");
            DrawLayoutResult(basePointsLayouter, _layout1, "basePointsLayouter_layout_1");
            DrawLayoutResult(basePointsLayouter, _layout2, "basePointsLayouter_layout_2");
        }

        static void DrawLayoutResult(ICloudLayouter layouter, Size[] rects, string layoutName)
        {
            foreach (var el in rects)
            {
                layouter.PutNextRectangle(el);
            }
            
            var resultImagePath = Path.Combine(Directory.GetCurrentDirectory(), $"{layoutName}.png");
            CloudLayoutVisualizer.SaveAsPngImage(layouter.GetLayout(), resultImagePath);
            System.Console.WriteLine($"Layout {layoutName} saved to {resultImagePath}");
        }
    }
}