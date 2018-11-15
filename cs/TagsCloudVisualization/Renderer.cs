using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class TagsCloudRenderer
    {
        public void RenderIntoFile(string filePath, List<Rectangle> rectangles, Size pictureSize)
        {
            var btm = new Bitmap(pictureSize.Width, pictureSize.Height);
            var obj = Graphics.FromImage(btm);
            foreach (var r in rectangles)
            {
                obj.DrawRectangle(new Pen(Color.Brown), r);
            }

            btm.Save(filePath);
        }
    }
}
