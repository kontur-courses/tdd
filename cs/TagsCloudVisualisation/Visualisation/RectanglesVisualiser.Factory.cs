using System;
using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public delegate void VisualisationDrawer(Graphics g, Rectangle rectangle);

    public partial class RectanglesVisualiser
    {
        public static void DrawRectangle(Graphics g, Color color, Rectangle rectangle) =>
            g.DrawRectangle(new Pen(color, BrushSize), rectangle);

        public static void DrawText(Graphics g, Color color, Rectangle rectangle, string text, Font font) =>
            g.DrawString(text, font, new SolidBrush(color), rectangle);

        public static RectanglesVisualiser New(Point center, VisualisationDrawer drawer) =>
            new DelegateHandledRectanglesVisualiser(center, drawer);

        private class DelegateHandledRectanglesVisualiser : RectanglesVisualiser
        {
            private readonly VisualisationDrawer drawer;

            public DelegateHandledRectanglesVisualiser(Point sourceCenterPoint, VisualisationDrawer drawer) : base(
                sourceCenterPoint)
            {
                this.drawer = drawer ?? throw new ArgumentNullException(nameof(drawer));
            }

            protected override void DrawPrepared(Graphics g, Rectangle rectangle) => drawer.Invoke(g, rectangle);
        }
    }
}