using System.Drawing;

namespace TagsCloudVisualization.Abstractions
{
    public interface ICentripetalCompactor
    {
        public Rectangle Compact(
            Rectangle candidate,
            List<Rectangle> placed,
            Point center);
    }
}
