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
            Console.WriteLine("Asdas");
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            for (var i = 0; i <1000; i++)
                circularCloudLayouter.PutNextRectangle(new Size(1, 1));

            ConsoleVisualization(circularCloudLayouter);
        }

        private static void ConsoleVisualization(CircularCloudLayouter circularCloudLayouter)
        {
            var coords = circularCloudLayouter.GetRectangles()
                .SelectMany(rect =>
                {
                    var points = new List<Point>();
                    for (var i = rect.Left; i < rect.Right; i++)
                    for (var j = rect.Top; j < rect.Bottom; j++)
                        points.Add(new Point(i, j));
                    return points;
                }).ToList();
            for (var i = 0; i < 40; i++)
            {
                for (var j = 0; j < 40; j++) 
                    Console.Write(coords.Contains(new Point(j + 30, i + 30)) ? "|||" : "...");
                Console.Write("\n");
            }

            Console.ReadKey();
        }
    }
}
