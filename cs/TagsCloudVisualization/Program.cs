using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            var circularCloudLayouter = new CircularCloudLayouter(new Point(175, 175));

            for (var i = 0; i < 100; i++)
                circularCloudLayouter.PutNextRectangle(new Size((10 + i * i) % 30, (10 + i * i) % 20));
            Bitmap bitmap = new Bitmap(300, 300);
            var graphics = Graphics.FromImage(bitmap);
            var rectangles = circularCloudLayouter.GetRectangles();
            foreach (var rectangle in rectangles)
            {
                graphics.DrawRectangle(new Pen(Color.RoyalBlue), rectangle);
            }

            bitmap.Save(Environment.CurrentDirectory + "\\Cloud2.png");


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
