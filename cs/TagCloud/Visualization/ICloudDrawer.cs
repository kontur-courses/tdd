using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloud.Visualization
{
    public interface ICloudDrawer
    {
        void DrawCloud(Point cloudCenter, List<Rectangle> rectangles);
    }
}
