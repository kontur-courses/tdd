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
            /*var cloudAreaSize = new Size(2500, 2500);
            var cloudAreaFrame = new Rectangle(new Point(0, 0), cloudAreaSize);
            var cloudCenter = new Point((int) (cloudAreaFrame.X + cloudAreaSize.Width / 2),
                (int) (cloudAreaFrame.Y + cloudAreaSize.Height / 2));
            */

            var testingLayouter = new TestingCircularLayoutGenerator(new Point(500, 45));
            var cloud = testingLayouter.GenerateTestLayout();
            
            var visualizer = new Vizualizer();
            var img = visualizer.GetLayoutImage(cloud);
            visualizer.SaveImage(path,img);
        }
    }

}
