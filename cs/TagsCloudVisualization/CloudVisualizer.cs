using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CloudVisualizer
    {
        private ICloudLayouter cloudLayouter;
        public CloudVisualizer(ICloudLayouter cloudLayouter)
        {
            this.cloudLayouter = cloudLayouter;
        }

        public Bitmap GetAndSaveImage(string imageName)
        {
            var cloud = cloudLayouter.CloudRectangle;
            var offsetPoint = new Point(-cloud.X, -cloud.Y);
            var image = new Bitmap(cloud.Width, cloud.Height);
            var graphics = Graphics.FromImage(image);
            var random = new Random();
            foreach (var rectangle in cloudLayouter.GetRectangles())
            {
                var pen = new Pen(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                rectangle.Offset(offsetPoint);

                graphics.DrawRectangle(pen, rectangle);
            }

            graphics.Dispose();
            image.Save(imageName);

            return image;
        }
    }
}
