using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point CenterPoint;
        private readonly SpiralGenerator spiralGenerator;
        private readonly List<Rectangle> createdRectangles = new();
        public CircularCloudLayouter(Point center)
        {
            CenterPoint = center;
            spiralGenerator = new SpiralGenerator(center);
        }

        public CircularCloudLayouter(Point center, int radiusDelta, double angleDelta)
        {
            CenterPoint = center;
            spiralGenerator = new SpiralGenerator(center, radiusDelta, angleDelta);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Rectangle can't have negative width or height");

            // spiralGenerator.ResetSpiral();

            while (true)
            {
                var nextPoint = spiralGenerator.GetNextPoint();

                var rectangleLocation = new Point(nextPoint.X - rectangleSize.Width / 2,
                    nextPoint.Y - rectangleSize.Height / 2);

                var newRectangle = new Rectangle(rectangleLocation, rectangleSize);
                if (createdRectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle))) continue;

                createdRectangles.Add(newRectangle);
                return newRectangle;
            }
        }

        public void CreateLayoutImage(string fileName, string? filePath = null)
        {
            var (imageWidth, imageHeight) = DetermineImageWidthAndImageHeight();

            var bitmap = new Bitmap(imageWidth, imageHeight);

            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Wheat);

            var blackPen = new Pen(Color.Black);

            var offsettedRectangles = createdRectangles.ToArray();

            for (var i = 0; i < offsettedRectangles.Length; i++)
            {
                offsettedRectangles[i].Offset(imageWidth / 2, imageHeight / 2);
            }

            filePath ??= AppDomain.CurrentDomain.BaseDirectory + @"\Images";

            Directory.CreateDirectory(filePath);

            graphics.DrawRectangles(blackPen, offsettedRectangles);
            bitmap.Save(filePath + @$"\{fileName}.png", ImageFormat.Png);
            
            //Console.WriteLine($"Image is saved to {filePath}" + @$"\{fileName}.png");
        }

        private (int imageWidth, int imageHeight) DetermineImageWidthAndImageHeight()
        {
            var imageWidth = 0;
            var imageHeight = 0;

            foreach (var rectangle in createdRectangles)
            {
                imageWidth = Math.Max(Math.Abs(rectangle.Right), Math.Abs(rectangle.Left));
                imageHeight = Math.Max(Math.Abs(rectangle.Top), Math.Abs(rectangle.Bottom));
            }

            imageWidth = 2 * imageWidth + 100;
            imageHeight = 2 * imageHeight + 100;

            return (imageWidth, imageHeight);
        }
    }
}
