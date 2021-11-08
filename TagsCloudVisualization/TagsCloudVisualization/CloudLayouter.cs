using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public abstract class CloudLayouter
    {
        protected List<Rectangle> Rectangles;

        public CloudLayouter()
        {
            Rectangles = new List<Rectangle>();
        }

        public abstract Rectangle PutNextRectangle(Size rectangleSize);

        public IEnumerable<Rectangle> GetLaidRectangles()
        {
            foreach (var r in Rectangles)
                yield return r;
        }
    }
}
