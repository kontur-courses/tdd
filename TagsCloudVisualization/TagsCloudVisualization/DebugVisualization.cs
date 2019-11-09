using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class DebugVisualization : IVisualization
    {
        public Bitmap DrawRectangles(List<Rectangle> rectangles)
        {
            var image = new Bitmap(900, 900);
            using (var drawPlace = Graphics.FromImage(image))
            {
                var color = Color.Black;
                foreach (var rectangle in rectangles)
                {
                    if (rectangle == rectangles.Last())
                        color = Color.Red;
                    drawPlace.DrawRectangle(new Pen(new SolidBrush(color), 3), rectangle);
                }
            }
            return image;
        }
    }
}
