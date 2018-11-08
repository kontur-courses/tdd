using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; set; }

        public CircularCloudLayouter(Point center, Size imageSize)
        {
            CheckInputData(center, imageSize);

            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {

            return new Rectangle();
        }


        private static void CheckInputData(Point center, Size imageSize)
        {
            if (imageSize.Width <= 0)
                throw new ArgumentException("image width should be positive");
            if (imageSize.Height <= 0)
                throw new ArgumentException("image height should be positive");
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("both center coordinates should be non-negative");
            if (center.X >= imageSize.Width || center.Y >= imageSize.Height)
                throw new ArgumentException("center coordinates are not inside the image");
        }
    }
}
