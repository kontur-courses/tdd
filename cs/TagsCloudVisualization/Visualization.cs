using System.Drawing;

namespace TagsCloudVisualization
{
    public class Visualization
    {
        public Image Draw(Rectangle[] rectangles, Color color)
        {
            var layoutSize = rectangles.GetSize();
            var bitmap = new Bitmap(layoutSize.Width, layoutSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            
            graphics.TranslateTransform(layoutSize.Width / 2, layoutSize.Height / 2);
            
            graphics.DrawRectangles(new Pen(color), rectangles);
            return bitmap;
        }
    }
}