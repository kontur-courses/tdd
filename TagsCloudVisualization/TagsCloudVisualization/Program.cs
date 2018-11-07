using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            var layouter = new CircularCloudLayouter(new Point(400, 400));
            for (var i = 0; i < 40; i++)
            {
                layouter.PutNextRectangle(new Size(rnd.Next(20, 50), rnd.Next(10, 20)));
            }
            
            var visualizer = new CircularCloudVisualizer(Color.RoyalBlue, Color.DarkBlue, Color.LightBlue);
            visualizer.SaveBitmap("r", 800, 800, layouter.Result);
        }
    }
}
