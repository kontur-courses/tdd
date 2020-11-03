using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public interface ILayouter
    {
        Rectangle PutNextRectangle(Size size);
    }
}
