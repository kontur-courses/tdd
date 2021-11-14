using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal static class CloudLayouterPainter
    {
        private static readonly List<Brush> BrushList = new List<Brush>
        {
            Brushes.Blue, Brushes.Aquamarine, Brushes.BlueViolet,
            Brushes.CornflowerBlue, Brushes.DarkBlue, Brushes.DarkCyan,
            Brushes.Indigo, Brushes.SteelBlue, Brushes.SlateBlue,
            Brushes.Purple, Brushes.SkyBlue, Brushes.Navy, Brushes.DarkCyan,
            Brushes.DarkSlateGray, Brushes.DarkOrchid, Brushes.MediumAquamarine,
            Brushes.MidnightBlue
        };

        public static void Draw(CircularCloudLayouter layouter, string imagePathToSave)
        {
            var image = new Bitmap(1500, 1500);
            var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black);
            foreach (var rect in layouter.Rectangles)
                graphics.FillRectangle(GetRandomBrush(), rect);
            image.Save(imagePathToSave);
        }

        private static Brush GetRandomBrush()
        {
            var randomBrushNumber = new Random(Guid.NewGuid().GetHashCode()).Next(BrushList.Count);
            return BrushList[randomBrushNumber];
        }
    }
}
