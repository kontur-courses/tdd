using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace TagsCloudVisualization
{
    public class TagCloudDrawing
    {
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly string fileName;
        private readonly CircularCloudLayouter tagCloud =
            new CircularCloudLayouter(new Point(1, 2));

        public TagCloudDrawing(int imageWidth, int imageHeight, string fileName, 
            Point tagCloudCenter)
        {
            if (imageHeight <= 0 || imageWidth <= 0)
                throw new ArgumentException("Size of image should be positive");
            if (fileName.Length == 0)
                throw new ArgumentException("File name must be not empty");
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.fileName = fileName;
            tagCloudCenter = new Point(tagCloudCenter.X + imageWidth / 2,
                tagCloudCenter.Y + imageHeight / 2);
            if (0 > tagCloudCenter.X || tagCloudCenter.X > imageWidth ||
                0 > tagCloudCenter.Y || tagCloudCenter.Y > imageHeight)
                throw new ArgumentException("Center of tag cloud must be in the picture");
            tagCloud = new CircularCloudLayouter(tagCloudCenter);
        }


        public void DrawTagCloud(IEnumerable<Size> sizes)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            var drawingObj = Graphics.FromImage(image);
            foreach (var curRectangleSize in sizes)
            {
                var curRectangle = tagCloud.PutNextRectangle(curRectangleSize);
                drawingObj.FillRectangle(new SolidBrush(Color.AliceBlue), curRectangle);
                drawingObj.DrawRectangle(new Pen(new SolidBrush(Color.Magenta)), curRectangle);
            }
            var imagePath = Path.Combine(new string[] {AppDomain.CurrentDomain.BaseDirectory, fileName});
            Console.WriteLine("Tag cloud visualization saved to file " + imagePath);
            image.Save(imagePath);
        }
    }
}