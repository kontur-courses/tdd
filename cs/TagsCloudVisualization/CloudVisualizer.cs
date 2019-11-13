using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.tools;

namespace TagsCloudVisualization
{
    internal class CloudVisualizer
    {
        private readonly int imageWidth;
        private readonly int imageHeight;

        private static readonly List<Color> Colors = new List<Color>()
        {
            Color.BurlyWood,
            Color.Chartreuse,
            Color.DarkSlateGray,
            Color.DarkOrange,
            Color.Blue,
            Color.Violet,
            Color.Crimson,
            Color.AliceBlue,
            Color.Bisque,
            Color.BlueViolet,
            Color.DarkKhaki,
            Color.Aqua,
            Color.Sienna
        };

        public CloudVisualizer(int imageWidth, int imageHeight)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        public Bitmap Draw(Cloud cloud)
        {
            var rectangles = cloud.Rectangles;

            if (rectangles.Count == 0)
                throw new ArgumentException("There are no rectangles");

            var bitmap = new Bitmap(imageWidth, imageHeight);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Black);
                graphics.TranslateTransform(imageWidth / 2, imageHeight / 2);

                var scale = ComputeScale(rectangles);
                graphics.ScaleTransform(scale, scale);

                for (var i = 0; i < rectangles.Count; i++)
                {
                    var color = Colors[i % Colors.Count];
                    var br = new SolidBrush(color);
                    graphics.FillRectangle(br, rectangles[i]);
                }
            }

            return bitmap;
        }

        private float ComputeScale(IReadOnlyCollection<Rectangle> rectangles)
        {
            var widthScale = imageWidth / rectangles.Average(x => x.Width) / (rectangles.Count / 3);
            var heightScale = imageHeight / rectangles.Average(x => x.Height) / (rectangles.Count / 3);

            return Math.Min((float) widthScale, (float) heightScale);
        }
    }
}