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
            ICircularCloudLayouter circ = new CircularCloudLayouter(new Point());
            var sizes = new List<Size>();
            var random = new Random();

            for (var x = 0; x < 100; x++)
            {
                sizes.Add(new Size(random.Next(50, 200), random.Next(10, 150)));
            }

            var rectangles = sizes
                .Select(circ.PutNextRectangle)
                .ToList();

            IVisualizator viz = new Visualizator(new Size(1600, 1600), rectangles);
            viz
                .Generate()
                .Save("result.bmp");
        }
    }
}