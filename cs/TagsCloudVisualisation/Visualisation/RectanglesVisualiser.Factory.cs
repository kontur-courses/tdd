using System;
using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public delegate void VisualisationDrawer(Graphics g, Rectangle rectangle);
    public class DelegateHandledRectanglesVisualiser : RectanglesVisualiser
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