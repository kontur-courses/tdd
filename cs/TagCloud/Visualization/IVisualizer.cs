using System.Drawing;

namespace TagCloud.Visualization
{
    public interface IVisualizer
    {
        void VisualizeCloud();
        void VisualizeDebuggingMarkup(Graphics g, Size imgSize, Point cloudCenter, int cloudCircleRadius);
    }
}
