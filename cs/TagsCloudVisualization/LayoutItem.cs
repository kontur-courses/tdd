using System.Drawing;

namespace TagsCloudVisualization
{
    internal class LayoutItem
    {
        public Rectangle Rectangle;

        public LayoutItem() { }

        public LayoutItem(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }
}
