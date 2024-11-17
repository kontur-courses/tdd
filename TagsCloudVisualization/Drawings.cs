using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Drawings
    {
        public static void DrawPicture(IRectangleGenerator rectangleGenerator, int quantity,
            IPointGenerator pointGenerator, Point startPoint, string filename)
        {
            var rectangleSizes = rectangleGenerator.
                GenerateRectangles(quantity).ToArray();
            var layout = new CircularCloudLayouter(startPoint, pointGenerator);
            var image = new Bitmap(layout.Size.Width, layout.Size.Height);
            foreach (var size in rectangleSizes)
            {
                DrawRectangle(image, layout.PutNextRectangle(size));
            }
            image.Save(filename);
        }

        public static void DrawRectangle(Bitmap image, Rectangle rectangle)
        {
            var brush = new SolidBrush(GetRandomColor());
            var formGraphics = Graphics.FromImage(image);
            formGraphics.FillRectangle(brush, rectangle);
            brush.Dispose();
            formGraphics.Dispose();
        }

        private static Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }
    }
}
