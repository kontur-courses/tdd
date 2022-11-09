using System.Drawing;

namespace TagsCloudVisualization.Core.Interfaces
{
    public interface IImgSaver
    {
        void Draw(IEnumerable<Rectangle> rectangles);
        void Save(string path);
    }
}
