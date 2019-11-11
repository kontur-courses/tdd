using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.TagsCloudVisualization
{
    class DebugVisualization : ITagsCloudVisualization<Rectangle>
    {
        public Bitmap DrawRectangles(List<Rectangle> rectangles, int imageWidth, int imageHeight)
        {
            var image = new Bitmap(900, 900);
            using (var drawPlace = Graphics.FromImage(image))
            {
                var blackPen = new Pen(new SolidBrush(Color.Black), 3);
                var redPen = new Pen(new SolidBrush(Color.Red), 3);
                foreach (var rectangle in rectangles)
                {
                    drawPlace.DrawRectangle(rectangles
                        .Any(rec => rec != rectangle && rectangle.IntersectsWith(rec)) ? redPen : blackPen, rectangle);
                }
            }
            return image;
        }
    }
}
