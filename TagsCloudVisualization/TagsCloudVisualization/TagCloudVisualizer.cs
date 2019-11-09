using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace TagsCloudVisualization
{
    public static class TagCloudVisualizer
    {
        public static Bitmap Visualize(CircularCloudLayouter layouter, Size imageSize)
        {
            var bmp = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bmp);
            var random = new Random();
            foreach (var rectangle in layouter.GetRectangles().ToArray())
                graphics.FillRectangle(new SolidBrush(GetRandomColor(random)), rectangle);
            return bmp;
        }

        private static Color GetRandomColor(Random rnd)
        {
            return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }
    }
}
                                                                                                                                 