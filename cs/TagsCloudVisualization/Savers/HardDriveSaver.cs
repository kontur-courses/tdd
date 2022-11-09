using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Savers
{
    public class HardDriveSaver : IBitmapSaver
    {
        public void Save(Bitmap bitmap, string filename)
        {
            filename += ".png";
            bitmap.Save(filename, ImageFormat.Png);
        }
    }
}