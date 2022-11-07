using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}
