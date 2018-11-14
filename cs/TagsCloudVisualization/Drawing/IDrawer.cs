using System.Drawing;
using TagsCloudVisualization.Geom;

namespace TagsCloudVisualization.Drawing
{
    public interface IDrawer
    {
        Bitmap Draw(CircularCloudLayouter layouter, int imageWidth, int imageHeight);
    }
}
