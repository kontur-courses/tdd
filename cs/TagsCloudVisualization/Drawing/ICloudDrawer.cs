using System.Drawing;

namespace TagsCloudVisualization.Drawing;

public interface ICloudDrawer
{
    void DrawCloud(string filename, Pen pen);
}