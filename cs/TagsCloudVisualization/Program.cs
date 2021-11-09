using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main()
        {
            CreatePictureWithRandomRectangles(100, 1920, 1080);
            CreatePictureWithRandomRectangles(200, 1920, 1080);
            CreatePictureWithSameRectangles(50, 1920, 1080, new Size(50, 50));
        }

        private static void CreatePictureWithRandomRectangles(int count, int width, int height)
        {
            var bmpVisualizer = new BitmapVisualizer(width, height);
            bmpVisualizer.GenerateRandomRectangles(count);
            bmpVisualizer.DrawRectangles(Color.Black, Color.Red);
            bmpVisualizer.SaveToFile($"{count}-randomRectangles.png");
        }

        private static void CreatePictureWithSameRectangles(int count, int width, int height, Size rectangleSize)
        {
            var bmpVisualizer = new BitmapVisualizer(width, height);
            bmpVisualizer.GenerateRectanglesWithSize(count, rectangleSize);
            bmpVisualizer.DrawRectangles(Color.Black, Color.Red);
            bmpVisualizer.SaveToFile(
                $"{count}RectanglesWithSize-width{rectangleSize.Width}-height{rectangleSize.Height}.png");
        }
    }
}