using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class TagCloudImageCreator
    {
        private class CustomCloudGeneration
        {
            public readonly Point Center;
            public readonly Size ImageSize;
            public readonly int MinWidth;
            public readonly int MaxWidth;
            public readonly int MinHeight;
            public readonly int MaxHeight;

            public CustomCloudGeneration(Point center, Size imageSize,
                int minWidth, int maxWidth, int minHeight, int maxHeight)
            {
                Center = center;
                ImageSize = imageSize;

                MinWidth = minWidth;
                MaxWidth = maxWidth;
                MinHeight = minHeight;
                MaxHeight = maxHeight;
            }

            public Bitmap GenerateCloudBitmap()
            {
                var rnd = new Random();
                var bitmap = new Bitmap(ImageSize.Width, ImageSize.Height);
                var graphics = Graphics.FromImage(bitmap);
                var layouter = new CircularCloudLayouter(Center);
                var amountOfRectangles = rnd.Next(50, 100);

                while (amountOfRectangles-- > 0)
                {
                    var brush = new SolidBrush(
                        Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                    graphics.FillRectangle(brush, layouter.PutNextRectangle(
                        new Size(rnd.Next(MinWidth, MaxWidth + 1),
                                rnd.Next(MinHeight, MaxHeight + 1))));
                }

                return bitmap;
            }
        }

        public static void Main()
        {
            var generationsInfo = new List<CustomCloudGeneration>()
            {
                new CustomCloudGeneration(
                    new Point(800,600),
                    new Size(800, 600),
                    3, 150,
                    3, 150
                    ),
                new CustomCloudGeneration(
                    new Point(400, 300),
                    new Size(800, 600),
                    50, 50,
                    50, 50
                    ),
                new CustomCloudGeneration(
                    new Point(400, 300),
                    new Size(1600, 1200),
                    3, 200,
                    3, 200)
            };
            for(var i = 0; i < generationsInfo.Count; i++)
            {
                var bitmap = generationsInfo[i].GenerateCloudBitmap();
                bitmap.Save($"result{i}.bmp");
            }
        }
    }
}
