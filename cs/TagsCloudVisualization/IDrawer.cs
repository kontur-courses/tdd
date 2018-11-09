using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IDrawer
    {
        void GenerateImage(IEnumerable<Rectangle> rectangles, string imageName);
    }
}