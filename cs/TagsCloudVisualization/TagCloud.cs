using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Console = System.Console;

namespace TagsCloudVisualization
{
    class TagCloud
    {
        static void Main()
        {
            Console.Write("Enter tags count for visualize: ");
            int.TryParse(Console.ReadLine(), out var tagsCount);

            var tagCloudCenter = ReadCloudCenter();

            var tags = TagsGenerator.GenerateRectanglesSizes(tagsCount);
            var circularCloudLayout = GetCircularCloudLayout(tags, tagCloudCenter);

            var directoryToSave = GetDirectoryToSave();
            var filename = GetFilename(tagsCount, tagCloudCenter);
            const int imageWidth = 2048;
            const int imageHeight = 1024;

            TagCloudVisualizer.Visualize(circularCloudLayout, directoryToSave, filename, imageWidth, imageHeight);

            Console.WriteLine("Visualization has been saved to " + Path.Combine(directoryToSave.FullName, filename));

            Console.ReadKey();
        }

        private static DirectoryInfo GetDirectoryToSave()
        {
            var userPictureDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            var visualizationsDirectory = Directory.CreateDirectory(Path.Combine(userPictureDirectory, @"TagsCloud\Visualizations"));

            return visualizationsDirectory;
        }

        private static string GetFilename(int tagsCount, Point center)
        {
            var randomPart = Path.GetRandomFileName();

            return $"{randomPart}count{tagsCount}x{center.X}y{center.Y}";
        }

        private static IEnumerable<Rectangle> GetCircularCloudLayout(IEnumerable<Size> tags, Point center)
        {
            var circularLayouter = new CircularCloudLayouter(center);

            return tags.Select(circularLayouter.PutNextRectangle);
        }

        private static Point ReadCloudCenter()
        {
            Console.Write("Enter tag cloud center X coordinate: ");
            int.TryParse(Console.ReadLine(), out var x);

            Console.Write("Enter tag cloud center Y coordinate: ");
            int.TryParse(Console.ReadLine(), out var y);

            return new Point(x, y);
        }
    }
}