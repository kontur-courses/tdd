using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Classes;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "cloud.bmp");
            var cloudAreaSize = new Size(1400, 800);
            var cloudAreaFrame = new RectangleF(new Point(0, 0), cloudAreaSize);
            var cloudCenter = new Point((int) (cloudAreaFrame.X + cloudAreaSize.Width / 2),
                (int) (cloudAreaFrame.Y + cloudAreaSize.Height / 2));
            
            var layouter = new CircularCloudLayouter(cloudCenter);
            var rectangles = layouter.GenerateTestLayout();
            
            var visualizer = new Vizualizer(cloudAreaSize);
            visualizer.DrawRectangles(rectangles);
            visualizer.SaveImage(path);
        }
    }

}
