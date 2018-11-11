using System;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Program
    {
        private static Random rand = new Random(1);
        
        private static readonly Size[] _layout1 =
        {
            new Size(100, 30), 
            new Size(150, 50), 
            new Size(80, 50), 
            new Size(200, 70), 
            new Size(144, 32), 
            new Size(100, 60), 
            new Size(123, 30), 
            new Size(200, 40), 
            new Size(248, 100), 
        };
        
        private static readonly Size[] _layout2 = Enumerable.Range(0, 200)
            .Select(i => new Size(rand.Next(100, 400) * rand.Next(8, 10), rand.Next(30, 100) * rand.Next(8, 10)))
            .ToArray();
        
        private static readonly Size[] _layout3 = Enumerable.Range(0, 500)
            .Select(i => new Size(rand.Next(100, 400) * rand.Next(1, 10), rand.Next(30, 100) * rand.Next(8, 10)))
            .ToArray();
        
        static void Main(string[] args)
        {
            var centerPoint = new Point(0, 0);
            var circularLayouter = new CircularCloudLayouter(centerPoint);
            var basePointsLayouter = new BasePointsCloudLayouter(centerPoint);
            
            DrawLayoutResult(circularLayouter, _layout1, "circularLayouter_layout_1");
            DrawLayoutResult(circularLayouter, _layout2, "circularLayouter_layout_2");
            DrawLayoutResult(circularLayouter, _layout3, "circularLayouter_layout_3");
            DrawLayoutResult(basePointsLayouter, _layout1, "basePointsLayouter_layout_1");
            DrawLayoutResult(basePointsLayouter, _layout2, "basePointsLayouter_layout_2");
            DrawLayoutResult(basePointsLayouter, _layout3, "basePointsLayouter_layout_3");
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

        static void DrawLayoutEachStep(ICloudLayouter layouter, Size[] rects, string layoutName)
        {
            for (int i = 0; i < rects.Length; i++)
            {
                layouter.PutNextRectangle(rects[i]);
                var resultImagePath = Path.Combine(Directory.GetCurrentDirectory(), $"{layoutName}_step{i + 1}.png");
                CloudLayoutVisualizer.SaveAsPngImage(layouter.GetLayout(), resultImagePath);
            }
            
            System.Console.WriteLine($"Layout steps {layoutName} saved to {Directory.GetCurrentDirectory()}");
        }
    }
}