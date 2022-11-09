using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    public interface IImageFromTestSaver
    {
        public bool TrySaveImageToFile(string testName, Image image, out string path);
    }
}