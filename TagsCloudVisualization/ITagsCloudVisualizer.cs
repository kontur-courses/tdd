using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ITagsCloudVisualizer
    {
        Bitmap GetImageCloud(List<Tag> tags, Color backgroundСolor);
    }
}