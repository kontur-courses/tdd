using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.FileManagers
{
    public interface IFileSaver
    {
        void SaveImage(Bitmap bitmap, string fileName, ImageFormat imageFormat, string path);
    }
}