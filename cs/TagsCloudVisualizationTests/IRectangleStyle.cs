using System.Drawing;

namespace TagsCloudVisualizationTests
{
    public interface IRectangleStyle
    {
        public void Draw(Graphics graphics, Rectangle rectangle);
    }
}