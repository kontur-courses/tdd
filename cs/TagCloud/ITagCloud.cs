using System.Drawing;

namespace TagCloud
{
    public interface ITagCloud
    {
        void PutNextTag(Size tagSize);

        void SaveToBitmap(bool shouldShowLayout, bool shouldShowMarkup);
    }
}