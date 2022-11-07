using System.Collections.Generic;
using System.Drawing;
using System.Linq;


// ReSharper disable once CheckNamespace
namespace CircularCloudLayouter
{
    public class DrawerCloudLayouterForTests : DrawerCloudLayouter
    {
        public  DrawerCloudLayouterForTests() : base(Color.Black){}

        public override void Draw(Graphics graphics, Rectangle[] rectangles)
        {
            graphics.Clear(Color.White);
            var colorRectangles = GetColorRectangles(rectangles);
            var countRed = colorRectangles.Count(r => r.Pen.Color == Color.Red);
            var countBlue = colorRectangles.Count(r => r.Pen.Color == Color.Blue);
            graphics.DrawString(
                $"number of intersecting rectangles:{countRed}",
                new Font(SystemFonts.DefaultFont.FontFamily, 12),
                Brushes.Red,
                10,
                10);
            graphics.DrawString(
                $"number of non-intersecting rectangles{ countBlue}",
                new Font(SystemFonts.DefaultFont.FontFamily, 12),
                    Brushes.Blue,
                    10,
                    30);

            colorRectangles.ForEach(r => graphics.DrawRectangle(r.Pen, r.Rectangle));
        }

        private static List<ColoredRectangle> GetColorRectangles(Rectangle[] rectangles)
        {
            var result = rectangles.Select(r => new ColoredRectangle(new Pen(Color.Blue), r)).ToList();

            for (var i = 0; i < rectangles.Length; i++)
            {
                for (var j = i + 1; j < rectangles.Length; j++)
                {
                    if (rectangles[i].AreIntersected(rectangles[j]))
                    {
                        result[i].Pen = new Pen(Color.Red);
                        result[j].Pen = new Pen(Color.Red);
                    }
                }
            }
            return result;
        }

        private class ColoredRectangle
        {
            public Pen Pen { get; set; }

            public Rectangle Rectangle { get; }


            public ColoredRectangle(Pen pen, Rectangle rectangle)
            {
                Pen = pen;
                Rectangle = rectangle;
            }
        }
    }
}