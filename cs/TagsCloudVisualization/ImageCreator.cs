using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class ImageCreator
    {
        private readonly string projectDirectory 
            = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        private readonly Bitmap bitmap;
        private readonly Graphics gp;

        public ImageCreator(int imageWidth, int imageHeight)
        {
            bitmap = new Bitmap(imageWidth, imageHeight);
            gp = Graphics.FromImage(bitmap);
            
            gp.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 34, 43)), new Rectangle(0,0, imageWidth, imageHeight));
        }

        public void DrawLines(List<Point> points)
        {
            gp.DrawLines(Pens.Black, points.ToArray());
        }
        
        public void DrawRectangles(IEnumerable<Rectangle> rects)
        {
            gp.DrawRectangles(new Pen(Color.FromArgb(255, 217,92,6)), rects.ToArray());
        }

        public void SaveImage(string path = null)
        {
            bitmap.Save(path ?? string.Concat(projectDirectory, @"\Images\img.png"));
        }
    }
}