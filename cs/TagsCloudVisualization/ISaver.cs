using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public interface ISaver
    {
        void SaveImage(Bitmap bitmap, string fileName, ImageFormat imageFormat, string path);
    }
}