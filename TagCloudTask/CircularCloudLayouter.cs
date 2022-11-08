using System;
using System.Drawing;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private Point _center;
        private ITagCloudEngine _engine;
        
        public CircularCloudLayouter(Point center, ITagCloudEngine engine)
        {
            if (engine is null)
                throw new ArgumentNullException();

            _center = center;
            _engine = engine;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return _engine.GetNextRectangle(rectangleSize);
        }
    }
}