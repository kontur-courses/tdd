using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IVisualizerCloud
    {
        Bitmap GetImageCloud(List<Tuple<Rectangle, string, Font>> blocks, Color backgroundСolor);
        Bitmap GetImageCloud(List<Rectangle> rectangles, Color colorOfRectangle, Color backgroundСolor);
    }
}