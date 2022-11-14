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
                var spiral = new ArchimedeanSpiral(center);
                var cloud = new CircularCloudLayouter(spiral);
                Utilities.GenerateRectangleSize(count, minSize, maxSize).ToList()
                    .ForEach(rectSize => cloud.PutNextRectangle(rectSize));

                var imageCreator = new ImageCreator(Width, Height);
                imageCreator.Graphics.DrawRectangles(Pens.Black, cloud.Rectangles.ToArray());

                var fullPath = Path.Combine(projectPath, ImagesDirectoryName, filename);
                imageCreator.Save(fullPath);
            }
        }
    }
}