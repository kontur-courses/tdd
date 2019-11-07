using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterVisualizer
    {
        public void SaveImage(IEnumerable<Rectangle> rectangles, string path)
        {
            var bitmap = new Bitmap(700, 700);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                foreach (var rectangle in rectangles)
                    g.DrawRectangle(Pens.Red, rectangle);
            }
            
            bitmap.Save(path);
        }
    }
}