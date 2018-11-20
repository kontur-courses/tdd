using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.LayoutGeneration;
using TagsCloudVisualization.LayoutVizualization;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "cloud.bmp");
            var cloudAreaSize = new Size(1400, 800);
            var cloudAreaFrame = new Rectangle(new Point(0, 0), cloudAreaSize);
            var cloudCenter = new Point((int) (cloudAreaFrame.X + cloudAreaSize.Width / 2),
                (int) (cloudAreaFrame.Y + cloudAreaSize.Height / 2));
            
            var layouter = new CircularCloudLayouter(cloudCenter);
            layouter.GenerateTestLayout();
            
            var visualizer = new Vizualizer();
            var img = visualizer.GetLayoutImage(layouter, cloudAreaSize);
            visualizer.SaveImage(path,img);
        }
    }

}
