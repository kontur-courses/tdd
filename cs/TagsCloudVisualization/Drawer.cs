using System.Drawing;

namespace TagsCloudVisualization
{
    public class Drawer
    {
        public static void CreateImage(int width, int height, IEnumerable<Rectangle> rectangles, string filename)
        {
            var b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(Color.White);
                foreach (var r in rectangles)
                {
                    g.DrawRectangle(new Pen(Color.Red), r);
                }
            }

            b.Save(filename + ".bmp");
        }
    }
}
