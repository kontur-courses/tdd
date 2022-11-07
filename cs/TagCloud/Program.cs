using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagCloud
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var imageSize = new Size(750, 750);

            var centralPoint = new Point(imageSize.Width / 2, imageSize.Height / 2);

            CircularCloudLayouter layouter = new CircularCloudLayouter(centralPoint);

            DirectoryInfo directoryToSaveImage = GetDirectoryToSaveImage();


            #region Equivalent squares cloud

            var sizes = RectangleSizeGenerator.GetConstantSizesList(500, new Size(25, 25));

            layouter.PutRectangles(sizes);

            CloudImageHandler imageHandler = new CloudImageHandler(imageSize, layouter);

            imageHandler.SaveImage(Path.Combine(directoryToSaveImage.FullName, "Equivalent_squares_cloud.png"));

            #endregion

            #region Horizontal rectangles cloud

            sizes = RectangleSizeGenerator.GetRandomSizesOrderedList(300, new Size(20, 10), new Size(80, 40));

            layouter = new CircularCloudLayouter(centralPoint);

            layouter.PutRectangles(sizes);

            imageHandler = new CloudImageHandler(imageSize, layouter);

            imageHandler.SaveImage(Path.Combine(directoryToSaveImage.FullName, "Horizontal_rectangles_cloud.png"));

            #endregion

            #region Vertical rectangles cloud

            sizes = RectangleSizeGenerator.GetRandomSizesOrderedList(300, new Size(10, 20), new Size(40, 80));

            layouter = new CircularCloudLayouter(centralPoint);

            layouter.PutRectangles(sizes);

            imageHandler = new CloudImageHandler(imageSize, layouter);

            imageHandler.SaveImage(Path.Combine(directoryToSaveImage.FullName, "Vertical_rectangles_cloud.png"));

            #endregion
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
    }
}
