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
                layouter.PutNextRectangle(new Size(rnd.Next(60 - i, 100 - i), rnd.Next(20 - i / 2, 40 - i / 2)));
            }
            
            var visualiser = new CircularCloudVisualiser(Color.RoyalBlue, Color.DarkBlue, Color.LightBlue);
            visualiser.SaveBitmap("test", 800, 800, layouter.Result);
        }
    }
}
