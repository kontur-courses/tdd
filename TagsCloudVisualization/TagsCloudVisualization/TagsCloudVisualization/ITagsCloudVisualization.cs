using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.TagsCloudVisualization
{
    internal interface ITagsCloudVisualization<T>
    {   
        Bitmap DrawRectangles(List<T> objectsToDraw, int imageWidth, int imageHeight);
    }
}
