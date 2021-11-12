using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class PainterOfRectangles
    {
        private Size pictSize;

        public PainterOfRectangles(Size sizeOfPicture)
        {
            pictSize = sizeOfPicture;
        }

        public void CreateImage(List<Rectangle> rectangles, string filename = "rectangles")
        {
            using Bitmap bmp = new Bitmap(pictSize.Width, pictSize.Height);

            using Graphics graphics = Graphics.FromImage(bmp);

            foreach (var rectangle in rectangles)
            {
                graphics.DrawRectangle(new Pen(Color.Blue, .5f), rectangle);
        private bool IsCorrectSizeImage(List<Rectangle> rectangles)
        {
            if (rectangles.Max(rectangle => rectangle.X) > pictSize.Height ||
                rectangles.Max(rectangle => rectangle.X) > pictSize.Width)
            {
                return false;
            }

            return true;
        }
    }
}