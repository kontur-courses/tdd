using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class CloudVisualizator
    {
        public static Bitmap Visualize(VisualizatorTheme.Theme theme, IEnumerable<Rectangle> rectangles,
            int width = 1000, int height = 1000)
        {
            var visualizationTheme = new VisualizatorTheme(theme);
            var result = new Bitmap(width, height);
            var graphics = Graphics.FromImage(result);
            graphics.FillRectangle(visualizationTheme.BackgroundBrush, new Rectangle(0, 0, width, height));
            foreach (var rectangle in rectangles)
                graphics.FillRectangle(visualizationTheme.GetRandomRectangleBrush(), rectangle);
            return result;
        }
    }
}