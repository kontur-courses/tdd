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

            // Необходимо чтобы значительно увеличить плотность, очень сильно жертвуем производительностью
            // spiralGenerator.ResetSpiral();

            while (true)
            {
                var nextPoint = spiralGenerator.GetNextPoint();

                var locationForRect = new Point(nextPoint.X - rectangleSize.Width / 2,
                    nextPoint.Y - rectangleSize.Height / 2);

                var newRect = new Rectangle(locationForRect, rectangleSize);
                if (createdRectangles.Any(rectangle => rectangle.IntersectsWith(newRect))) continue;

                createdRectangles.Add(newRect);
                return newRect;
            }
        }

        public void CreateImageOfLayout(string fileName, string? filePath = null)
        {
            var imageWidth = 2 * createdRectangles.Max(rect => Math.Max(Math.Abs(rect.Right), Math.Abs(rect.Left))) + 100;
            var imageHeight = 2 * createdRectangles.Max(rect => Math.Max(Math.Abs(rect.Top), Math.Abs(rect.Bottom))) + 100;
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

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            graphics.DrawRectangles(blackPen, offsettedRectangles);
            bitmap.Save(filePath + @$"\{fileName}.png", ImageFormat.Png);

            // Оставил это под комментом, использовал для удобства при запуске Main в LayoutExamples, но вдруг пригодится ;)
            //Console.WriteLine($"Image is saved to {filePath}" + @$"\{fileName}.png");
        }
    }
}
