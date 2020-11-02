using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Core;

namespace TagsCloudVisualization.Visualization
{
    public class CircularCloudLayouterVisualizer
    {
        private CircularCloudLayouter CloudLayouter { get; }
        private string WorkDirectory { get; }
        private readonly Brush brushForRect = Brushes.Yellow;
        private readonly Pen penForFrame = Pens.Black;

        public CircularCloudLayouterVisualizer(CircularCloudLayouter cloudLayouter)
        {
            CloudLayouter = cloudLayouter ?? throw new ArgumentNullException(nameof(cloudLayouter));
            var workDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\TagClouds";
            if (!Directory.Exists(workDir))
                Directory.CreateDirectory(workDir);
            WorkDirectory = workDir;
        }

        public void SaveInFile(string filename = "last_cloud.jpg", int imageWidth = 720, int imageHeight = 720)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename shouldn`t be null, empty or white space!");
            if (filename.Any(c => Path.GetInvalidPathChars().Contains(c)))
                throw new ArgumentException(@"Filename shouldn't contains \ / : * ? "" < > |");

            if (!filename.EndsWith(".jpg"))
                filename += ".jpg";

            var bmp = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(bmp);

            graphics.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight);
            foreach (var rectangle in CloudLayouter.Rectangles)
            {
                graphics.FillRectangle(brushForRect, rectangle);
                graphics.DrawRectangle(penForFrame, rectangle);
            }

            var path = Path.Combine(WorkDirectory, filename);
            bmp.Save(path, ImageFormat.Jpeg);

            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}