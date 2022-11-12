using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Program
    {
        const int Width = 800;
        const int Height = 600;
        const string ImagesDirectoryName = "Images";

        static void Main()
        {
            var projectPath = Utilities.GetProjectPath();
            var center = new Point(Width / 2, Height / 2);

            foreach (var (count, minSize, maxSize, filename) in Utilities.GetTestData())
            {
                var cloud = new CircularCloudLayouter(center);
                Utilities.GenerateRectangleSize(count, minSize, maxSize)
                    .Select(rectSize => cloud.PutNextRectangle(rectSize))
                    .ToArray();

                var imageCreator = new ImageCreator(Width, Height);
                imageCreator.Graphics.DrawRectangles(Pens.Black, cloud.Rectangles.ToArray());

                var fullPath = Path.Combine(projectPath, ImagesDirectoryName, filename);
                imageCreator.Save(fullPath);
            }
        }
    }
}