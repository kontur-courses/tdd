using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagCloud
{
    public class TagCloudVisualization
    {
        private static Random random = new Random();

        public static void SaveAsBitmap(CircularCloudLayouter tagCloud, string file)
        {
            var bitmap = new Bitmap( tagCloud.GetWidth()+1, tagCloud.GetHeight()+1);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var reactangle in tagCloud.Reactangles)
                {
                    graphics.DrawRectangle(new Pen(GetRandomBrush()), reactangle);
                }
            }
            bitmap.Save(file);
        }

        protected static Brush GetRandomBrush()
        {
            return new SolidBrush(GetRandomColor());
        }

        protected static Color GetRandomColor()
        {
            var knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            var randomColorName = knownColors[random.Next(knownColors.Length)];
            return Color.FromKnownColor(randomColorName);
        }
    }
}