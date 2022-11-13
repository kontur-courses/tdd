using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class CloudLayouterForm : Form
    {
        private readonly ICloudLayouterDrawer cloudLayouterDrawer;
        private readonly string path = "result//result.png";
        private readonly Rectangle[] rectangles;

        public CloudLayouterForm(Size formSize, Rectangle[] rectangles,
            ICloudLayouterDrawer cloudLayouterDrawer)
        {
            Size = formSize;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.rectangles = rectangles;
            this.cloudLayouterDrawer = cloudLayouterDrawer;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            using (var bitmap = new Bitmap(Size.Width, Size.Height))
            {
                var g = Graphics.FromImage(bitmap);

                cloudLayouterDrawer.Draw(g, rectangles);

                Directory.CreateDirectory("result");
                bitmap.Save(path, ImageFormat.Png);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            cloudLayouterDrawer.Draw(e.Graphics, rectangles);
        }
    }
}