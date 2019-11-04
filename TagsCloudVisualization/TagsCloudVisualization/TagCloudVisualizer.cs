using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagCloudVisualizer
    {
        public static Bitmap Visualize(CircularCloudLayouter layouter, Size imageSize)
        {
            var rnd = new Random();
            var bmp = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bmp);

            foreach (var rectangle in layouter.GetRectangles().ToArray())
            {
                var color = Color.FromArgb(
                    rnd.Next(256),
                    rnd.Next(256),
                    rnd.Next(256)
                    );
                graphics.FillRectangle(new SolidBrush(color), rectangle);
            }

            return bmp;
        }
    }
}