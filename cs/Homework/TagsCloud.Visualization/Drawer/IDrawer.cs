using System.Drawing;

namespace TagsCloud.Visualization.Drawer
{
    public interface IDrawer
    {
        Image Draw(Rectangle[] rectangles);
    }
}