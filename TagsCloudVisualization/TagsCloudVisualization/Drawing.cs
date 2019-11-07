using System;
using System.Drawing;
using System.IO;

namespace UserInterface
{
    public class DrawingClass
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly string fileName;

        public DrawingClass(int imageWidth, int imageHeight, string fileName)
        {
            if (imageHeight <= 0 || imageWidth <= 0)
                throw new ArgumentException("Size of image should be not negative");
            if (fileName.Length == 0)
                throw new ArgumentException("File name must be not empty");
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.fileName = fileName;
        }

        public void DrawTagCloud(CircularCloudLayouter tagCloud)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            var drawingObj = Graphics.FromImage(image);
            foreach (var curRec in tagCloud.RectanglesList)
            {
                drawingObj.FillRectangle(new SolidBrush(Color.AliceBlue),
                    new Rectangle(curRec.LeftBottomVertex.X, curRec.LeftBottomVertex.Y,
                        curRec.Size.Width, curRec.Size.Height));
                drawingObj.DrawRectangle(new Pen(new SolidBrush(Color.Magenta)),
                    new Rectangle(curRec.LeftBottomVertex.X, curRec.LeftBottomVertex.Y,
                        curRec.Size.Width, curRec.Size.Height));
            }
            var imagePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + fileName;
            Console.WriteLine("Saved file has path:");
            Console.WriteLine(imagePath);
            image.Save(imagePath);
        }
    }
}