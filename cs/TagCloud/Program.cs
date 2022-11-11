using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagCloud
{
    internal class Program
    {
        private static readonly Size ImageSize = new Size(750, 750);

        private static DirectoryInfo _directoryToSaveImage;

        static void Main(string[] args)
        {
            _directoryToSaveImage = GetDirectoryToSaveImage();

            var rectanglesSizes = RectangleSizeGenerator.GetConstantSizes(600, new Size(25, 10));
            CreateAndSaveCloudImage(rectanglesSizes, "Equivalent_rectangles_cloud.png");

            rectanglesSizes = RectangleSizeGenerator.GetRandomOrderedSizes(300, new Size(20, 10), new Size(80, 40));
            CreateAndSaveCloudImage(rectanglesSizes, "Horizontal_rectangles_cloud.png");

            rectanglesSizes = RectangleSizeGenerator.GetRandomOrderedSizes(300, new Size(10, 20), new Size(40, 80));
            CreateAndSaveCloudImage(rectanglesSizes, "Vertical_rectangles_cloud.png");
        }

        private static DirectoryInfo GetSolutionDirectory()
        {
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

            while (currentDirectory != null && !currentDirectory.GetFiles("*.sln").Any())
            {
                currentDirectory = currentDirectory.Parent;
            }

            return currentDirectory;
        }

        private static DirectoryInfo GetDirectoryToSaveImage()
        {
            DirectoryInfo solutionDirectory = GetSolutionDirectory();

            if (solutionDirectory.GetDirectories("TagCloudImages").Any())
                return solutionDirectory.GetDirectories("TagCloudImages").First();

            return solutionDirectory.CreateSubdirectory("TagCloudImages");
        }

        private static void CreateAndSaveCloudImage(IReadOnlyList<Size> rectanglesSizes, string fileName)
        {
            var centralPoint = new Point(ImageSize.Width / 2, ImageSize.Height / 2);

            var layouter = new CircularCloudLayouter(centralPoint);

            var imageCreator = new CloudImageGenerator(layouter, ImageSize, Color.Black);

            var image = imageCreator.Generate(rectanglesSizes);

            image.Save(Path.Combine(_directoryToSaveImage.FullName, fileName));
        }
    }
}
