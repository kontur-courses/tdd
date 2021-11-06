using System;
using System.Drawing;

namespace TagCloud.Visualization
{
    public class Visualizer : IVisualizer
    {
        private IMarkupDrawer markupDrawer;
        private ICloudDrawer cloudDrawer;

        public Visualizer(IMarkupDrawer markupDrawer, ICloudDrawer cloudDrawer)
        {
            this.markupDrawer = markupDrawer;
            this.cloudDrawer = cloudDrawer;
        }

        public void VisualizeCloud()
        {
            throw new NotImplementedException();
        }

        public void VisualizeDebuggingMarkup(Graphics g, Size imgSize,
            Point cloudCenter, int cloudCircleRadius)
        {
            markupDrawer.DrawCanvasBoundary(g, imgSize);
            markupDrawer.DrawAxis(g, imgSize, cloudCenter);
            markupDrawer.DrawCloudBoundary(g, imgSize, cloudCenter, cloudCircleRadius);
        }
    }
}
