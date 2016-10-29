using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    public interface ITagsCloudVisualizer
    {
        Bitmap GetImageCloud(List<Tag> tags, int width, int height, Color backgroundСolor);
    }
}