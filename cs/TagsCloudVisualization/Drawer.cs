using System.Drawing;

namespace TagsCloudVisualization
{
    public class Drawer
    {
        public static Image GetImage(Point centerPoint, IEnumerable<Rectangle> rectangles)
        {
            var image = new Bitmap(centerPoint.X * 2, centerPoint.Y * 2);
            var gr = Graphics.FromImage(image);
            gr.Clear(Color.Black);
            gr.DrawRectangles(new Pen(Color.White), rectangles.ToArray());

            return image;
        }
    }
}
