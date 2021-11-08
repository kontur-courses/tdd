using System.Drawing;

namespace TagsCloudVisualizationTests
{
    public class ColoredFillStyle : IRectangleStyle
    {
        private readonly ColoredStyle coloredStyle = new ColoredStyle();

        public void Draw(Graphics graphics, Rectangle rectangle)
        {
            graphics.FillRectangle(new SolidBrush(coloredStyle.GetNextColor()), rectangle);
            graphics.DrawRectangle(new Pen(Color.White, 1), rectangle);
        }
    }
}