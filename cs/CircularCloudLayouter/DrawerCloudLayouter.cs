using System.Drawing;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CircularCloudLayouter
{
    public class DrawerCloudLayouter
    {
        private readonly Pen pen;

        public DrawerCloudLayouter(Color color)
        {
            pen = new Pen(color);
        }

        public virtual void Draw(Graphics graphics, Rectangle[] rectangles)
        {
            graphics.Clear(Color.White);
            graphics.DrawRectangles(pen, rectangles);
        }
        
    }
}