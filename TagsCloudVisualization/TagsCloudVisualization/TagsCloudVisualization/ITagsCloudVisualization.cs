using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.TagsCloudVisualization
{
    internal interface ITagsCloudVisualization<T>
    {   
        Bitmap Draw(List<T> objectsToDraw, int imageWidth, int imageHeight);
    }
}
