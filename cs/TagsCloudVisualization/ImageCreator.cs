using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class ImageCreator
    {
        private readonly string projectDirectory 
            = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        private readonly Bitmap bitmap;
        private readonly Graphics gp;
        private readonly int shiftX;
        private readonly int shiftY;
        
        public ImageCreator(int imageWidth, int imageHeight, Point center)
        {
            shiftX = center.X;
            shiftY = center.Y;
            
            bitmap = new Bitmap(imageWidth, imageHeight);
            gp = Graphics.FromImage(bitmap);
            gp.FillRectangle(Brushes.White, new Rectangle(0,0, imageWidth, imageHeight));
        }

        public void DrawSpiral(List<Point> rects)
        {
            for (var i = 1; i < rects.Count; i++)
            {
                gp.DrawLine(Pens.Black, rects[i].X + shiftX,rects[i].Y + shiftY , rects[i - 1].X + shiftX, rects[i - 1].Y + shiftY );
            }
        }
        
        public void DrawRectangles(List<Rectangle> rects)
        {
            foreach (var rect in rects)
            {
                gp.DrawRectangle(Pens.Black, new Rectangle(rect.X + shiftX, rect.Y + shiftY, rect.Width, rect.Height));
            }
        }

        public void SaveImage(string path = null)
        {
            bitmap.Save(path ?? string.Concat(projectDirectory, @"\Images\img.png"));
        }
    }
}