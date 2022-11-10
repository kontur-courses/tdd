using System;
using System.Drawing;

namespace TagCloud
{
    public class TagCloudVisualization
    {
        private static Random random = new Random();

        public static void SaveAsBitmap(TagCloud tagCloud, string file)
        {
            var bitmap = new Bitmap(1000, 1000);//tagCloud.GetWidth(), tagCloud.GetHeight());
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var rectangle in tagCloud.Rectangles)
                {
                    graphics.DrawRectangle(new Pen(GetRandomBrush()), rectangle);
                }
            }
            bitmap.Save(file);
        }

        private static Brush GetRandomBrush()
        {
            return new SolidBrush(GetRandomColor());
        }

        private static Color GetRandomColor()
        {
            var knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            var randomColorName = knownColors[random.Next(knownColors.Length)];
            return Color.FromKnownColor(randomColorName);
        }
    }
}