using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Visualization
    {
        public List<Rectangle> RectangleList { get; set; }
        public Pen ColorPen { get; set; }
        public Visualization(List<Rectangle> rectangleList, Pen colorPen)
        {
            RectangleList = rectangleList;
            ColorPen = colorPen;
        }
        public void DrawImage(Size imageSize, string path)
        {
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            image = DrawRectangles(image);

            SaveImage(image, path);
        }

        private Bitmap DrawRectangles(Bitmap image)
        {
            var graphics = Graphics.FromImage(image);
            foreach (var rectangle in RectangleList)
            {
                graphics.DrawRectangle(ColorPen, rectangle);
            }
            graphics.Dispose();
            return image;
        }

        private static void SaveImage(Bitmap image, string path)
        {
            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            
        }
    }
}
