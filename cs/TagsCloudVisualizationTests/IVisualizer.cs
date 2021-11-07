using System.Drawing;

namespace TagsCloudVisualizationTests
{
    public interface IVisualizer
    {
        public void Draw(Graphics graphics);
        public Size GetBitmapSize();
    }
}