using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.Figures
{
    class Rectangle : IFigure
    {
        private System.Drawing.Rectangle rect;

        public Rectangle(Point location, Size size) => rect = new System.Drawing.Rectangle(location, size);
        public Rectangle() => rect = new System.Drawing.Rectangle();

        public System.Drawing.Rectangle BaseRectangle { get => rect; }
        public Size Size { get => rect.Size; set => rect.Size = value; }
        public Point Center
        {
            get => new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            set => rect.Location = new Point(value.X - rect.Width / 2, value.Y - rect.Height / 2);
        }
        public Point Location { get => rect.Location; set => rect.Location = value; }

        public bool Contains(Point point) => rect.Contains(point);

        public bool IntersectsWith(IFigure figure)
        {
            if (figure is Rectangle rect)
                return this.rect.IntersectsWith(rect.BaseRectangle);

            for (var y = this.rect.Top; y <= this.rect.Bottom; y++)
                for (var x = this.rect.Left; x <= this.rect.Right; x++)
                    if (figure.Contains(new Point(x, y)))
                        return false;
            return true;
        }
    }
}