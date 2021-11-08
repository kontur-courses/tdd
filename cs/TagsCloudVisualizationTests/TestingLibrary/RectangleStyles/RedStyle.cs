using System.Drawing;
using TagsCloudVisualizationTests.Interfaces;

namespace TagsCloudVisualizationTests.TestingLibrary.RectangleStyles
{
    public class RedPenStyle : IRectangleStyle
    {
        private readonly Pen red = new Pen(Color.Red, 1);

        public void Draw(Graphics graphics, Rectangle rectangle) =>
            graphics.DrawRectangle(red, rectangle);
    }
}