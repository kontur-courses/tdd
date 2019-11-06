using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CloudVisualization
    {
        private Bitmap image;
        public CloudVisualization(int width, int height)
        {
            image = new Bitmap(width, height);
        }

        public Bitmap DrawRectangles(List<Rectangle> rectangles)
        {
            var random = new Random();
            using (var drowPlace = Graphics.FromImage(image))
            {
                foreach (var rectangle in rectangles)
                {
                    Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                    drowPlace.FillRectangle(new SolidBrush(color), rectangle);
                }
            }
            return image;
        }
    }
}
