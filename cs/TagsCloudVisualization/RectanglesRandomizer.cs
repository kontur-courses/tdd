using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectanglesRandomizer
    {
        public static List<Size> GetSortedRectangles(int maxWidth, int maxHeight, int count)
        {
            if (maxHeight <= 0 || maxWidth <= 0 || count <= 0)
                throw new ArgumentException(
                    "Sides of the rectangle and rectangle count should not be non-positive");

            var rectangles = new List<Size>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                rectangles.Add(new Size(random.Next(1, maxWidth), random.Next(1, maxHeight)));
            }

            return rectangles.OrderByDescending(rect => rect.Height * rect.Width).ToList();
        }
    }
}
