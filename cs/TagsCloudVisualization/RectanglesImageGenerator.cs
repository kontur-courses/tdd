using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectanglesImageGenerator
    {
        public void Generate(int rectanglesAmount, int step, int widthMultiplier, int heightMultiplier,
            int centerX, int centerY, int distanceBetweenSpiralPoints, string imageName)
        {
            var layouter = new CircularCloudLayouter(new Point(centerX, centerY),
                new SpiralPointsGenerator().GetPoints(1));
            if (step > 0)
            {
                for (var size = 1; size <= rectanglesAmount; size += step)
                    AddRectangle(layouter, size, widthMultiplier, heightMultiplier);
            }
            else
            {
                for (var size = rectanglesAmount; size > 0; size += step)
                    AddRectangle(layouter, size, widthMultiplier, heightMultiplier);
            }
            new RectanglesDrawer().GenerateImage(layouter.Rectangles, imageName);
        }

        private static void AddRectangle(CircularCloudLayouter layouter, int size, int widthMultiplier, int heightMultiplier)
        {
            var width = size * widthMultiplier;
            var height = size * heightMultiplier;
            layouter.PutNextRectangle(new Size(width, height));
        }
    }
}