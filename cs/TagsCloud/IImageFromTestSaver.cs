using System.Drawing;

namespace TagsCloud
{
    public interface IImageFromTestSaver
    {
        public bool TrySaveImageToFile(string testName, Image image, out string path);
    }
}