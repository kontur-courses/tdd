using System;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;

namespace TagCloudVisualisation
{
    class Program
    {
        public static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(600, 300));

            var sizes = new List<Size>();
            var rnd = new Random();

            for (int i = 0; i < 200; i++)
            {
                sizes.Add(new Size(rnd.Next(1, 50), rnd.Next(1, 50)));
            }

            var c = layouter.CreateCloud(sizes);

            var image = layouter.CreateImage();

            image.Save("cloud.png");
            Console.WriteLine("Image saved to file cloud.png");
        }
    }
}

