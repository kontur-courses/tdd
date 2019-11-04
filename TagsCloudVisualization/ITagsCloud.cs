using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    interface ITagsCloud
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
