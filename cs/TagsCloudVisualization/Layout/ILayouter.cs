using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Layout
{
    internal interface ILayouter
    {
        RectangleF PutNextRectangle(SizeF rectangleSize);
    }
}
