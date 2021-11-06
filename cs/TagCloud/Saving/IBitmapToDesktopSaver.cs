using System.Drawing;

namespace TagCloud.Saving
{
    public interface IBitmapToDesktopSaver
    {
        void Save(Bitmap bitmap, bool openAfterSave);
    }
}
