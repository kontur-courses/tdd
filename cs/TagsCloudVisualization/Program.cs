using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
            for (var i = 0; i < 1000; i++)
                circularCloudLayouter.PutNextRectangle(new Size(20, 3));
            Console.WriteLine("Cloud Filled");
            DrawAndSaveCloudImage(circularCloudLayouter, "Cloud2");
            Console.WriteLine("Image Generated");
            Console.ReadKey();
        }

        private static void DrawAndSaveCloudImage(CircularCloudLayouter circularCloudLayouter, string name)
        {
            var imageSize = circularCloudLayouter.Center.X * 2;
            Bitmap bitmap = new Bitmap(imageSize, imageSize);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in circularCloudLayouter.Rectangles)
            {
                var rnd = new Random(rectangle.Left * rectangle.Right - rectangle.Bottom);
                graphics.FillRectangle(new SolidBrush(
                    Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))), rectangle);
            }

            bitmap.Save($"{Environment.CurrentDirectory}\\{name}.png", ImageFormat.Png);
        }

        private static void ConsoleVisualization(CircularCloudLayouter circularCloudLayouter)
        {
            var coords = circularCloudLayouter.Rectangles
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
