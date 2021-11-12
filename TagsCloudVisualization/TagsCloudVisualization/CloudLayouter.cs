using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public abstract class CloudLayouter
    {
        protected List<Rectangle> _rectangles;
        public List<Rectangle> Rectangles => _rectangles.ToList();

        public CloudLayouter()
        {
            _rectangles = new List<Rectangle>();
        }

        public abstract Rectangle PutNextRectangle(Size rectangleSize);
    }
}
