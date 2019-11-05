using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class LayouterVisualizer
    {
        public static void SaveLayoutBitmap(string fileName, CircularCloudLayouter layouter)
        {
            int maxX = layouter.Layout.Select(r => r.Right).Max();
            int minX = layouter.Layout.Select(r => r.Left).Min();
            int maxY = layouter.Layout.Select(r => r.Bottom).Max();
            int minY = layouter.Layout.Select(r => r.Top).Min();

            int bmpWidth = maxX - minX;
            int bmpHeight = maxY - minY;

            var bmp = new Bitmap(bmpWidth, bmpHeight);
            var graphics = Graphics.FromImage(bmp);
            var whiteRectangle =
                new Rectangle(minX - 500 + bmpWidth / 2, minY - 500 + bmpHeight / 2, bmpWidth, bmpHeight);

            graphics.FillRegion(new SolidBrush(Color.White), new Region(whiteRectangle));

            Pen pen = new Pen(new SolidBrush(Color.Blue));
            foreach (var rect in layouter.Layout)
            {
                rect.Offset(new Point(-layouter.CenterPosition.X, -layouter.CenterPosition.Y));
                rect.Offset(new Point(bmpWidth / 2, bmpHeight / 2));
                graphics.DrawRectangle(pen, rect);
            }

            bmp.Save(fileName);
        }
    }
}