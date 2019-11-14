using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IRectangleCloudDrawer
    {
        void DrawCloud(IEnumerable<TagInfo> tags, string filename);
        void DrawRectangles(IEnumerable<Rectangle> rectangles, string filename);
    }
}