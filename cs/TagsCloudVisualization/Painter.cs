using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Painter
    {
        private readonly Bitmap image;
        private readonly Graphics graphics;
        private readonly RectangleGenerator generator;
        private readonly Pen rectanglePen;
        private readonly Pen spiralPen;
        private readonly CircularCloudLayouter layouter;

        public Painter(Size imageSize, Pen rectanglePen, Pen spiralPen, RectangleGenerator generator)
        {
            image = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(image);
            this.generator = generator;
            this.rectanglePen = rectanglePen;
            this.spiralPen = spiralPen;
            layouter = new CircularCloudLayouter(new Point(imageSize / 2));
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            graphics.DrawRectangle(rectanglePen, rectangle);
        }

        private void DrawSpiral(List<Point> spiralPoints)
        {
            graphics.DrawCurve(spiralPen, spiralPoints.ToArray());
        }

        public void CreatePicture(int rectanglesCount, string saveTo)
        {
            var rectanglesSizes = new List<Size>();

            for (var i = 0; i < rectanglesCount; i++)
                rectanglesSizes.Add(generator.GetRandomRectangle());

            rectanglesSizes.ForEach(t => DrawRectangle(layouter.PutNextRectangle(t)));

            DrawSpiral(layouter.SpiralPoints);
            image.Save(saveTo);
        }

        public static void DrawToFile(Size size, List<Rectangle> rectangles, string filename)
        {
            var b = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(b))
            {
                rectangles.ForEach(t => g.DrawRectangle(new Pen(Color.Red), t));
            }
            b.Save(filename + ".png");
        }
    }
}
