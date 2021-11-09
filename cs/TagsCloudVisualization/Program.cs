using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "ImageExamples");
            var drawer = new Visualization();
            var i = 1;
            foreach (var parameter in Parameters())
            {
                var layout = new CircularCloudLayouter(new Point(0, 0));
                var count = parameter.Count;
                var factory = parameter.Factory;
                var color = parameter.Color;
                var rectangles = Enumerable.Range(1, count).Select(rec => layout.PutNextRectangle(factory.Invoke()));
                var image = drawer.Draw(rectangles.ToArray(), color);
                var filename = $"{i++}.bmp";
                var path = Path.Combine(directory,  filename);
                image.Save(path, ImageFormat.Bmp);
            }
        }


        private static IEnumerable<Parameters> Parameters()
        {
            var rnd = new Random();
            
            yield return new Parameters(
                100, () => new Size(rnd.Next(50, 80), rnd.Next(50, 80)), Color.Azure);
            yield return new Parameters(
                500, () => new Size(rnd.Next(40, 80), rnd.Next(10, 20)), Color.LawnGreen);
            yield return new Parameters(
                1000, () => new Size(rnd.Next(10, 20), rnd.Next(10, 30)), Color.Gold);
        }
    }

    public class Parameters
    {
        public Func<Size> Factory { get; }

        public int Count { get; }
        public Color Color { get; }

        public Parameters(int count, Func<Size> factory, Color color)
        {
            Factory = factory;
            Count = count;
            Color = color;
        }
    }
}