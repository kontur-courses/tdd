using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICloudVisualizer
    {
        Bitmap CreateImage(string path);
    }
}