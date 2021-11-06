using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualization.ColorGenerators;

#pragma warning disable CA1416

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static readonly Size GeneratedImageSize = new(1000, 1000);
        private static readonly SizeF CloudScale = new(0.7f, 0.7f);

        private static void Main(string[] args)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedClouds");
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            foreach (var settings in GetSettings())
            {
                using var bitmap = new Bitmap(GeneratedImageSize.Width, GeneratedImageSize.Height);
                settings.Draw(bitmap, CloudScale);
                var filename = GenerateFileName();
                var path = Path.Combine(directory, filename + ".bmp");
                bitmap.Save(path, ImageFormat.Bmp);
            }
        }

        private static IEnumerable<DrawerSettings> GetSettings()
        {
            var rnd = new Random();
            yield return new DrawerSettings(GenerateRectangles(
                    50,
                    new CircularCloudLayouter(new Point()),
                    () => new Size(rnd.Next(30, 50), rnd.Next(20, 30))),
                new TagsCloudDrawer(new RandomColorGenerator(rnd)));
            yield return new DrawerSettings(GenerateRectangles(
                    100,
                    new CircularCloudLayouter(new Point()),
                    () => new Size(rnd.Next(40, 50), rnd.Next(20, 30))),
                new TagsCloudDrawer(new RainbowColorGenerator(rnd)));
            yield return new DrawerSettings(GenerateRectangles(
                    1000,
                    new CircularCloudLayouter(new Point()),
                    () => new Size(rnd.Next(40, 50), rnd.Next(20, 30))),
                new TagsCloudDrawer(new RainbowColorGenerator(rnd)));
            yield return new DrawerSettings(GenerateRectangles(
                    1000,
                    new CircularCloudLayouter(new Point()),
                    () => new Size(rnd.Next(10, 50), rnd.Next(10, 50))),
                new TagsCloudDrawer(new GrayscaleColorGenerator(rnd)));
        }

        private static Rectangle[] GenerateRectangles(int count, CircularCloudLayouter layouter, Func<Size> sizeFactory)
        {
            if (layouter == null) throw new ArgumentNullException(nameof(layouter));
            if (sizeFactory == null) throw new ArgumentNullException(nameof(sizeFactory));
            if (count <= 0) throw new ArgumentException($"{nameof(count)} should be positive");
            return Enumerable.Range(0, count)
                .Select(x => layouter.PutNextRectangle(sizeFactory()))
                .ToArray();
        }

        private static string GenerateFileName()
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}
#pragma warning restore CA1416