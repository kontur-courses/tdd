using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualization.ColorGenerators;

#pragma warning disable CA1416

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static readonly Size GeneratedImageSize = new Size(1000, 1000);
        private static readonly SizeF CloudScale = new SizeF(0.7f, 0.7f);
        private static readonly Color BackgroundColor = Color.Gray;
        private static void Main(string[] args)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedClouds");
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            foreach (var drawer in GenerateDrawers())
            {
                var bitmap = drawer.Draw(GeneratedImageSize, CloudScale);
                var filename = GenerateFileName();
                var path = Path.Combine(directory, filename + ".bmp");
                bitmap.Save(path, ImageFormat.Bmp);
            }
        }

        private static IEnumerable<TagsCloudDrawer> GenerateDrawers()
        {
            var rnd = new Random();
            yield return new TagsCloudDrawer(50,
                new CircularCloudLayouter(new Point()),
                () => new Size(rnd.Next(30, 50), rnd.Next(20, 30)),
                new RandomColorGenerator(rnd));
            yield return new TagsCloudDrawer(100,
                new CircularCloudLayouter(new Point()),
                () => new Size(rnd.Next(40, 50), rnd.Next(20, 30)),
                new RainbowColorGenerator(rnd));
            yield return new TagsCloudDrawer(1000,
                new CircularCloudLayouter(new Point()),
                () => new Size(rnd.Next(40, 50), rnd.Next(20, 30)),
                new RainbowColorGenerator(rnd));
            yield return new TagsCloudDrawer(1000,
                new CircularCloudLayouter(new Point()),
                () => new Size(rnd.Next(10, 50), rnd.Next(10, 50)),
                new GrayscaleColorGenerator(rnd));
        }

        private static string GenerateFileName()
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}
#pragma warning restore CA1416