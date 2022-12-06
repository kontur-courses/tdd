using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Drawing.Bitmap;

namespace TagsCloudVisualization
{
    public class TagCloudVisualizer
    {
        public void MakePicture()
        {
            //1. получение прямоугольников
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var size = new Size(50, 50);
            var rectangleList = new List<Rectangle>();
            for (int i = 0; i < 10; i++)
            {
                rectangleList.Add(layouter.PutNextRectangle(size));
            }
            
            //2. Нахождение ширины и высоты
            var minX = Int32.MaxValue;
            var maxX = Int32.MinValue;
            var minY = Int32.MaxValue;
            var maxY = Int32.MinValue;
            foreach (var rectangle in rectangleList)
            {
                minX = Math.Min(rectangle.Left, minX);
                maxX = Math.Max(rectangle.Right, maxX);
                minY = Math.Min(rectangle.Bottom, minY);
                maxY = Math.Max(rectangle.Top, maxY);
            }
            var width = maxX - minX;
            var height = maxY - minY;
            
            var bitmap = new Bitmap(1000, 1020);
            Graphics.FromImage(bitmap);
            bitmap.Save("D:\\шпора-2022\\tdd\\cs\\TagsCloudVisualization\\1.png");

        }
    }
}