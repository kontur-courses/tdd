using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layout1 = new CircularCloudLayouter(new Point(0, 0));
            var layout2 = new CircularCloudLayouter(new Point(2, 3));
            var layout3 = new CircularCloudLayouter(new Point(-1, 5));
            DrawLayout(layout1, 100, new int[] {100, 200}, new int[] { 100, 200 }, "result_0'0_100.png");
            DrawLayout(layout2, 1000, new int[] { 100, 200 }, new int[] { 50, 100 }, "result_2'3_1000.png");
            DrawLayout(layout3, 500, new int[] { 50, 100 }, new int[] { 100, 200 }, "result_-1'5_500.png");
        }

        public static void DrawLayout(CircularCloudLayouter layout, int times, int[] widthRange, int[] heightRange, string filename)
        {
            var visualizer = new CircularCloudVisualizer();
            var random = new Random();
            for (var i = 0; i < times; i++)
                layout.PutNextRectangle(new Size(random.Next(widthRange[0], widthRange[1]), 
                    random.Next(heightRange[0], heightRange[1])));
            var image = visualizer.DrawRectangles(layout);
            image.Save(filename);
        }
        

    }
}
