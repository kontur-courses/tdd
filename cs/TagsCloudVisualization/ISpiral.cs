using System.Drawing;

namespace TagsCloudVisualization
{
    interface ISpiral
    {
        Rectangle GetRectangleInCurrentSpiralPosition(Size rectangleSize);
    }
}
