using System.Collections.Generic;
using System.Drawing;
using System.Linq;


// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class CloudLayouterDrawerForTests : ICloudLayouterDrawer
    {
        private int numberIntersectingRectangles;

        public void Draw(Graphics graphics, Rectangle[] rectangles)
        {
            graphics.Clear(Color.White);
            var colorRectangles = GetColorRectangles(rectangles);

            graphics.DrawString(
                $"number of intersecting rectangles: {numberIntersectingRectangles}",
                new Font(SystemFonts.DefaultFont.FontFamily, 12),
                Brushes.Red,
                10,
                10);

            graphics.DrawString(
                "number of non-intersecting rectangles: " +
                $"{colorRectangles.Count - numberIntersectingRectangles}",
                new Font(SystemFonts.DefaultFont.FontFamily, 12),
                Brushes.Blue,
                10,
                30);

            colorRectangles.ForEach(r => graphics.DrawRectangle(r.Item1, r.Item2));
        }

        private List<(Pen, Rectangle)> GetColorRectangles(Rectangle[] rectangles)
        {
            var result = rectangles
                .Select(r => (Pen: new Pen(Color.Blue), Rectangle: r))
                .ToList();

            for (var i = 0; i < rectangles.Length; i++)
            {
                for (var j = i + 1; j < rectangles.Length; j++)
                {
                    if (!rectangles[i].AreIntersected(rectangles[j])) continue;

                    numberIntersectingRectangles++;
                    result[i].Pen.Color = Color.Red;
                    result[j].Pen.Color = Color.Red;
                }
            }

            return result;
        }
    }
}