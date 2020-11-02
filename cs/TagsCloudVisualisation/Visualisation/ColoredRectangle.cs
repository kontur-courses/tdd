using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public readonly struct ColoredRectangle
    {
        public readonly Rectangle Rectangle;
        public readonly Color Color;

        public ColoredRectangle(Rectangle rectangle, Color color)
        {
            Rectangle = rectangle;
            Color = color;
        }

        public static implicit operator Rectangle(ColoredRectangle coloredRectangle) => coloredRectangle.Rectangle;
        public override string ToString() => $"{Rectangle} {Color}";
    }
}