using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        private readonly Color backgroundColor = Color.White;
        private readonly Color rectangleBorderColor = Color.Black;
        private readonly Size imageSize;
        private readonly List<Rectangle> rectangleStorage;
        private readonly ImageSaver imageSaver;
        private readonly Size defaultSizeForImage = new Size(500, 500);

        public CircularCloudVisualizer(List<Rectangle> rectangles)
        {
            rectangleStorage = rectangles;
            imageSize = CalculateImageSize();
            rectangleStorage = PlaceCloudInImage();
            imageSaver = new ImageSaver();
        }

        public void CreateImage(string? filePath = null, bool withSaveSteps = false)
        {
            var rectangles = rectangleStorage.ToArray();

            using var image = new Bitmap(imageSize.Width, imageSize.Height);
            using var graphics = Graphics.FromImage(image);

            graphics.Clear(backgroundColor);

            var pen = new Pen(rectangleBorderColor);

            for (var i = 0; i < rectangles.Length; i++)
            {
                var nextRectangle = rectangles[i];
                graphics.DrawRectangle(pen, nextRectangle);
                if (withSaveSteps)
                {
                    imageSaver.SaveImage(image, filePath);
                }
            }
            imageSaver.SaveImage(image, filePath);
        }

        private List<Rectangle> PlaceCloudInImage()
        {
            var deltaForX = CalculateDeltaForMoveByAxis(r => r.Left, r => r.Right, imageSize.Width);
            var deltaForY = CalculateDeltaForMoveByAxis(r => r.Top, r => r.Bottom, imageSize.Height);

            return rectangleStorage
                .Select(r => new Rectangle(new Point(r.Left + deltaForX, r.Y + deltaForY), r.Size))
                .ToList();

        }

        private int CalculateDeltaForMoveByAxis(
            Func<Rectangle, int> selectorForMin,
            Func<Rectangle, int> selectorForMax,
            int sizeByAxis)
        {
            if (rectangleStorage.Count == 0) return 0;
            var minByAxis = rectangleStorage.Min(selectorForMin);
            var maxByAxis = rectangleStorage.Max(selectorForMax);
            return minByAxis < 0
                ? -1 * minByAxis
                : maxByAxis > sizeByAxis
                ? sizeByAxis - maxByAxis
                : 0;
        }

        private Size CalculateImageSize()
        {
            if (rectangleStorage.Count == 0) return defaultSizeForImage;
            var width = rectangleStorage.Max(r => r.Right) - rectangleStorage.Min(r => r.Left);
            var height = rectangleStorage.Max(r => r.Bottom) - rectangleStorage.Min(r => r.Top);
            var sizeForRectangles = Math.Max(Math.Max(width, height), defaultSizeForImage.Width);
            return new Size(sizeForRectangles, sizeForRectangles);
        }
    }
}