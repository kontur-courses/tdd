using System.Drawing;

namespace TagCloud
{
    public interface ITagCloud
    {
        void PutNextTag(Size tagSize);

        string SaveBitmap(bool shouldShowLayout, bool shouldShowMarkup, bool openAfterSave);
    }
}