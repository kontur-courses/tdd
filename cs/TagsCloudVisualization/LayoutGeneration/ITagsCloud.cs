using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.LayoutGeneration
{
    public interface ITagsCloud
    {
        List<Rectangle> Rectangles { get; set; }
    }
}