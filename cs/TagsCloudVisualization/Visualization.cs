using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class Visualization : IDisposable
    {
        public List<Rectangle> RectangleList { get; set; }

        private Pen ColorPen { get; }


        public Visualization(List<Rectangle> rectangleList, Pen colorPen)
        {
            RectangleList = rectangleList;
            ColorPen = colorPen;
        }

        public void DrawAndSaveImage(Size imageSize, string path, ImageFormat format)
        {
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            image = DrawRectangles(image);
            image.Save(path, format);
            Dispose();
        }

        private Bitmap DrawRectangles(Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (var rectangle in RectangleList)
                    graphics.DrawRectangle(ColorPen, rectangle);
                return image;
            }
        }

        public void Dispose()
        {
            ColorPen.Dispose();
        }
    }
}
