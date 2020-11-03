using System;
using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public class RectanglesVisualiser : BaseCloudVisualiser
    {
        private const float BrushSize = 1;

        private readonly VisualisationDrawer drawer;

        public RectanglesVisualiser(Point sourceCenterPoint, VisualisationDrawer drawer) : base(
            sourceCenterPoint)
        {
            this.drawer = drawer ?? throw new ArgumentNullException(nameof(drawer));
        }

        public void Draw(Rectangle rectangle) => base.Draw(rectangle);

        protected override void DrawPrepared(RectangleF rectangle) => drawer.Invoke(Graphics, rectangle);

        public static void DrawRectangle(Graphics g, Color color, RectangleF rectangle) =>
            g.DrawRectangle(new Pen(color, BrushSize), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        public delegate void VisualisationDrawer(Graphics g, RectangleF rectangle);
    }
}