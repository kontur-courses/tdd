using System;
using System.Drawing;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace CircularCloudLayouter
{
    public class CloudLayouterForm : Form
    {
        private readonly DrawerCloudLayouter drawer;
        private readonly Rectangle[] rectangles;

        public CloudLayouterForm(DrawerCloudLayouter drawer, Rectangle[] rectangles)
        {

            this.drawer = drawer;
            this.rectangles = rectangles;
        }


        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawer.Draw(e.Graphics, rectangles);
        }
    }
}