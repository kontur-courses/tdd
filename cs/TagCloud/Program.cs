using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagCloud
{
    internal class Program
    {
        private static DirectoryInfo _directoryToSaveImage;

        static void Main(string[] args)
        {
            _directoryToSaveImage = GetDirectoryToSaveImage();

            var rectanglesSizes = RectangleSizeGenerator.GetConstantSizes(600, new Size(25, 10));
            GenerateAndSaveCloudImage(rectanglesSizes, "Equivalent_rectangles_cloud.png");

            rectanglesSizes = RectangleSizeGenerator.GetRandomOrderedSizes(300, new Size(20, 10), new Size(80, 40));
            GenerateAndSaveCloudImage(rectanglesSizes, "Horizontal_rectangles_cloud.png");

            rectanglesSizes = RectangleSizeGenerator.GetRandomOrderedSizes(300, new Size(10, 20), new Size(40, 80));
            GenerateAndSaveCloudImage(rectanglesSizes, "Vertical_rectangles_cloud.png");
        }


        private static DirectoryInfo GetDirectoryToSaveImage()
        {
            var currentDirectory = Environment.CurrentDirectory;

            var directoryToSaveImage = Path.Combine(currentDirectory, "TagCloudImages");

            if (!Directory.Exists(directoryToSaveImage))
                return Directory.CreateDirectory(directoryToSaveImage);

            return new DirectoryInfo(directoryToSaveImage);
        }

        private static void GenerateAndSaveCloudImage(IEnumerable<Size> rectanglesSizes, string fileName)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            var imageGenerator = new CloudImageGenerator(layouter, Color.Black);

            var bitmap = imageGenerator.GenerateBitmap(rectanglesSizes);

            ImageSaver.SaveBitmap(bitmap, _directoryToSaveImage, fileName);
        }
    }
}
