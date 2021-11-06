using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Visualization
{
    public class CloudDrawer : ICloudDrawer
    {
        public void DrawCloud(Graphics g, Point cloudCenter, List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                g.DrawRectangle(Pens.Black, rectangle);
            }
        }
    }
}
