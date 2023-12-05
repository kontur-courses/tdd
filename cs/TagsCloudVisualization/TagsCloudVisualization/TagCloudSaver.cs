using System.Drawing;

namespace TagsCloudVisualization
{
    public static class TagCloudSaver
    {
        private static Random random = new();

        public static void Save(List<Rectangle> rectangles, string fileName)
        {
            using var bitmap = new Bitmap(1000, 1000);
            foreach (var rect in rectangles)
            {
                DrawRectangle(rect, bitmap);
            }
            bitmap.Save(fileName);
        }

        private static void DrawRectangle(Rectangle rectangle, Bitmap bitmap)
        {
            var color = Color.FromKnownColor((KnownColor)(1 + random.Next(175)));
            for (var i = rectangle.Left; i <= rectangle.Right; i++)
            {
                bitmap.SetPixel(i, rectangle.Top, color);
                bitmap.SetPixel(i, rectangle.Bottom, color);
            }

            for (var i = rectangle.Top; i <= rectangle.Bottom; i++)
            {
                bitmap.SetPixel(rectangle.Left, i, color);
                bitmap.SetPixel(rectangle.Right, i, color);
            }
        }
    }
}
