using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Visualization
{
    public class Visualizer : IVisualizer
    {
        private readonly IMarkupDrawer markupDrawer;
        private readonly ICloudDrawer cloudDrawer;

        public Visualizer(IMarkupDrawer markupDrawer, ICloudDrawer cloudDrawer)
        {
            this.markupDrawer = markupDrawer;
            this.cloudDrawer = cloudDrawer;
        }

        public void VisualizeCloud(Graphics g, Point cloudCenter, List<Rectangle> rectangles)
        {
            cloudDrawer.DrawCloud(g, cloudCenter, rectangles);
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
