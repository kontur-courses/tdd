using System.Drawing;

namespace TagCloud.Saving
{
    public interface IBitmapSaver
    {
        void Save(Bitmap bitmap, bool openAfterSave);
    }
}