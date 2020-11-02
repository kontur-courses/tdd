using System.Drawing;

namespace TagsCloudVisualization
{
    public abstract class TagCloudLayouter
    {
        protected TagCloudLayouter(Point center)
        {
        }

        public abstract Rectangle PutNextRectangle(Size rectangleSize);
    }
}