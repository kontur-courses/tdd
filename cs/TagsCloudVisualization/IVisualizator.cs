using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IVisualizator
    {
        Bitmap Generate();
    }
}