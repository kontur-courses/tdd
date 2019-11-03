using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualization
    {
        private readonly Point center;

        public TagsCloudVisualization(Point center)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException();
            }
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }

        public Point GetCenter()
        {
            return this.center;
        }
    }
}
