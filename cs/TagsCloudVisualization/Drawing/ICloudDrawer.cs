using System.Drawing;

namespace TagsCloudVisualization.CloudDrawer;

public interface ICloudDrawer
{
    void DrawCloud(string filename, Pen pen);
}