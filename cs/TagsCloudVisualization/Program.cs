using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Program
    {
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
        
        private static readonly Size[] _layout2 = Enumerable.Range(0, 100).Select(i => new Size(200, 30)).ToArray();
        
        static void Main(string[] args)
        {
            drawLayout(_layout1, "layout1");
            drawLayout(_layout2, "layout2");
        }

        static void drawLayout(Size[] rects, string layoutName)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            foreach (var el in rects)
            {
                layouter.PutNextRectangle(el);
            }

            var resultImagePath = Path.Combine(Directory.GetCurrentDirectory(), $"{layoutName}_result.png");
            CloudLayoutVisualizer.SaveAsPngImage(layouter.Rects, resultImagePath);
            System.Console.WriteLine($"Layout {layoutName} saved to {resultImagePath}");
        }
    }
}