using System.Drawing;

namespace TagCloud
{
    public interface ITagCloud
    {
        void PutNextTag(Size tagSize);

        string SaveBitmapTo(bool shouldShowLayout, bool shouldShowMarkup, bool openAfterSave);
    }
}