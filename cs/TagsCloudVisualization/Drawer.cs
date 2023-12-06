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
            using (var brush = new SolidBrush(Color.White))
            {
                foreach (var rectangle in rectangles)
                    gr.FillRectangle(brush, rectangle);
            }


            return image;
        }
    }
}
