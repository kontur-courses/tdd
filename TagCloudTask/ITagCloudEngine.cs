using System.Drawing;

namespace TagCloud
{
    public interface ITagCloudEngine
    {
        public Rectangle GetNextRectangle(Size sizeOfRectangle);
    }
}