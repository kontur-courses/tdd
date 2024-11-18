using System.Drawing;

namespace TagsCloudVisualization.Savers;

public interface IBitmapSaver
{
    public void SaveBitmap(Bitmap bitmap,  string filename);
}