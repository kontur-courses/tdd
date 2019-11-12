using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization

{
    public class Programm
    {
        public static void GetCloud(CircularCloudLayouter layouter, int width, int height, int number)
        {
            var cloud = new CloudVisualization(width, height);
            var result = cloud.DrawRectangles(layouter.Rectangles);
            result.Save(string.Format(@"../../layouter{0}.png", number));
        }

        public static CircularCloudLayouter GetLayouter(int count, Point center)
        {
            var layouter = new CircularCloudLayouter(center);
            Random random = new Random();
            for (int i = 0; i < count; i++)
                layouter.PutNextRectangle(new Size(random.Next(3, 50), random.Next(3, 50)));
            return layouter;
        }
        public static void Main()
        {

            GetCloud(GetLayouter(400, new Point(500, 500)), 1000, 1000, 1);
            GetCloud(GetLayouter(200, new Point(250, 250)), 500, 500, 2);
            Console.WriteLine("Введиьте путь к файлу");
            var path = Console.ReadLine();
            var sizes = File.ReadAllLines(path)
                .Select(l => l.Split(' ')
                    .Select(s => int.Parse(s))
                    .ToArray())
                .Select(ar => Tuple.Create(ar[0], ar[1])).ToList();
            var layouter = new CircularCloudLayouter(new Point(sizes.Count, sizes.Count));
            foreach (var size in sizes)
                layouter.PutNextRectangle(new Size(size.Item1, size.Item2));
            var width = (layouter.Rectangles.Max(rec => rec.X) -layouter.Rectangles.Min(rec=>rec.X))*2;
            var height = (layouter.Rectangles.Max(rec => rec.Y) - layouter.Rectangles.Min(rec => rec.X))*2;
            GetCloud(layouter, width, height, 3);
        }
    }
}
