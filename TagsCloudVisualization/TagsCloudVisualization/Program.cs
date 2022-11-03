using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var graphics = CreateGraphics(out var g);
            var arithmeticSpiral = new ArithmeticSpiral(new Point(500, 500), 2);
            var circularCloudLayouter = new CircularCloudLayouter(new DivideTags(12).sizeDictionary);
            var rectangles = new List<Rectangle>();
            var point = arithmeticSpiral.GetPoint();
            DrawFirstTag(circularCloudLayouter, point, rectangles, g);
            var nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();

            while (true)
            {
                point = arithmeticSpiral.GetPoint();
                if (nextSizeRectangle.Item2.IsEmpty)
                    break;
                if (!Contains(rectangles, point, nextSizeRectangle))
                {
                    DrawRectangle(point, rectangles, g, circularCloudLayouter, ref nextSizeRectangle);
                    arithmeticSpiral = new ArithmeticSpiral(new Point(500, 500), 2);
                }
            }

            graphics.Save("123");
        }

        private static Bitmap CreateGraphics(out Graphics g)
        {
            var bitmap =
                new Bitmap(Image.FromFile(
                    "C:\\Users\\Lodgent\\Desktop\\git clones\\tdd\\TagsCloudVisualization\\whitef.jpg"));
            g = Graphics.FromImage(bitmap);
            return bitmap;
        }
        private static void DrawFirstTag(CircularCloudLayouter circularCloudLayouter, Point point, List<Rectangle> rectangles, Graphics g)
        {
            var f = circularCloudLayouter.GetRectangleOptions();
            var fr = new Rectangle(point - f.Item2 / 2, f.Item2);
            rectangles.Add(fr);
            Draw(point, g, f, fr);
        }
        private static bool Contains(List<Rectangle> rectangles, Point point, Tuple<string, Size, Font> nextSizeRectangle)
        {
            return rectangles
                .Select(x =>
                    x.IntersectsWith(new Rectangle(point - nextSizeRectangle.Item2 / 2, nextSizeRectangle.Item2)))
                .Contains(true);
        }

        private static void DrawRectangle(Point point, List<Rectangle> rectangles, Graphics g,
            CircularCloudLayouter circularCloudLayouter,
            ref Tuple<string, Size, Font> nextSizeRectangle)
        {
            var rectangle = new Rectangle(point - nextSizeRectangle.Item2 / 2, nextSizeRectangle.Item2);
            rectangles.Add(rectangle);
            Draw(point, g, nextSizeRectangle, rectangle);
            nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();

        }

        private static void Draw(Point point, Graphics g, Tuple<string, Size, Font> nextSizeRectangle,
            Rectangle rectangle)
        {
            g.DrawString(nextSizeRectangle.Item1, nextSizeRectangle.Item3, new SolidBrush(Color.Red),
                point - nextSizeRectangle.Item2 / 2);
            g.DrawRectangle(new Pen(Color.Green), rectangle);
        }

        

        
    }
}