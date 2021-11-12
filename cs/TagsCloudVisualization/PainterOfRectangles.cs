using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class PainterOfRectangles : IPainter
    {
        private Size pictSize;

        public PainterOfRectangles(Size sizeOfPicture)
        {
            pictSize = sizeOfPicture;
        }

        public void CreateImage(List<Rectangle> rectangles, ICommandImage command)
        {
            if (command == null)
                throw new ArgumentException("Parametr command is null");

            if (!IsCorrectSizeImage(rectangles))
            {
                throw new Exception("Размеры изображения не подходят, чтобы вписать прямоугольники");
            }

            using Bitmap bmp = new Bitmap(pictSize.Width, pictSize.Height);

            using Graphics graphics = Graphics.FromImage(bmp);

            using Pen penRectangle = new Pen(Color.Blue, .5f);

            foreach (var rectangle in rectangles)
            {
                graphics.DrawRectangle(penRectangle, rectangle);
            }

            using Image imageRectangles = new Bitmap(bmp);

            command.Execute(imageRectangles);
        }

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