using System.Drawing;

namespace TagCloudTask.Saving
{
    public interface IBitmapSaver
    {
        string Save(Bitmap bitmap, bool openAfterSave);
    }
}