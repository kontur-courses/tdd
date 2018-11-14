using System.Drawing;

namespace TagsCloudVisualization.Drawing
{
    public interface IWriter
    {
        void Write(Bitmap image, string name);
    }
}
