using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CloudVisualizator : ICloudVisualizator
    {
        private Bitmap bitmap;
        private Graphics graphic;
        private Point imageCenter;
        private Point cloudCenter;
        public CloudVisualizator(Size imageSize, Point cloudCenter)
        {
            bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            graphic = Graphics.FromImage(bitmap);
            imageCenter = new Point(imageSize.Width / 2, imageSize.Height / 2);
            this.cloudCenter = cloudCenter;
        }

        public void DrawRectangle(Pen pen, Rectangle rectangle)
        {
            rectangle.X += imageCenter.X - cloudCenter.X;
            rectangle.Y += imageCenter.Y - cloudCenter.Y;
            graphic.DrawRectangle(pen, rectangle);
        }

        public void DrawRectangles(Pen pen, List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
                DrawRectangle(pen, rectangle);
        }

        public void SaveImage(string path)
        {
            bitmap.Save(path);
        }
    }
}
