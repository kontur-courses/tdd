using System;
using System.Drawing;
using TagsCloudVisualization.CloudLayouts;

namespace TagsCloudVisualization.CloudVisualizers
{
    public class CloudVisualizer : ICloudVisualizer
    {
        private readonly Pen pen;
        public ICloudLayout Cloud { get; set; }

        public CloudVisualizer(Pen pen, ICloudLayout cloud)
        {
            this.pen = pen;
            if (cloud == null) throw new ArgumentNullException();
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
                graphics.DrawRectangle(pen, rectToDraw);
            }

            return image;
        }
    }
}