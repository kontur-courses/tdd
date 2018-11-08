using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.CloudLayouts;

namespace TagsCloudVisualization.CloudVisualizers
{
    public class CloudVisualizer
    {
        public Pen Pen { get; set; }
        public ICloudLayout Cloud { get; set; }

    public CloudVisualizer(Pen pen, ICloudLayout cloud)
        {
            this.Pen = pen;
            this.Cloud = cloud;
        }

        public Bitmap GenerateImage(Size imageSize)
        {
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(image);
            var imageCenter = new Point(image.Width / 2, image.Height / 2);
            foreach (var rectangle in Cloud.GetListOfRectangles())
            {
                var rectToDraw = new Rectangle(rectangle.Location + (Size)imageCenter, rectangle.Size);
                graphics.DrawRectangle(Pen, rectToDraw);
            }

            return image;
        }
    }
}