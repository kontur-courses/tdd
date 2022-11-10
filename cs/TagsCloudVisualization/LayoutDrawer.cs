using System.Drawing;

namespace TagsCloudVisualization
{
    public class LayoutDrawer
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private readonly Pen pen;

        public LayoutDrawer(int width, int height, Color color, int penWidth)
        {
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(color, penWidth);
        }
        
        public void DrawLayout(CloudGenerator cg)
        {
            foreach (var rectangle in cg.GetNextRectangle())
            {
                graphics.DrawRectangle(pen, rectangle);
            }
            bitmap.Save("Ex1.png");
        }
    }
}

