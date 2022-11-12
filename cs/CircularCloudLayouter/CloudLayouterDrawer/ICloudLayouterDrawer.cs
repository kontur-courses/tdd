using System.Drawing;


// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public interface ICloudLayouterDrawer
    {
        void Draw(Graphics graphics);

        CloudLayouter CloudLayouter { get; }
    }
}