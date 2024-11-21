using System.Drawing;

namespace TagsCloudVisualization.Visualisation
{
    public interface IVisualiser : IDisposable
    {
        public void DrawRectangle(Bitmap bitmap, Rectangle rectangle);
    }
}
