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
        private VisualizerSettings Settings { get; }
        private readonly Brush brushForRect = Brushes.Yellow;
        private readonly Pen penForFrame = Pens.Black;

        public CircularCloudLayouterVisualizer(CircularCloudLayouter cloudLayouter)
        {
            CloudLayouter = cloudLayouter ?? throw new ArgumentNullException(nameof(cloudLayouter));
            Settings = VisualizerSettings.ReadSettingsFromConfig();
        }

        public void SaveInFile(string filename = "last_cloud.jpg")
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename shouldn`t be null, empty or white space!");
            if (filename.Any(c => Path.GetInvalidPathChars().Contains(c)))
                throw new ArgumentException(@"Filename shouldn't contains \ / : * ? "" < > |");

            if (!filename.EndsWith(".jpg"))
                filename += ".jpg";

            var bmp = DrawRectanglesInBitMap();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Settings.WorkDirectory, filename);
            bmp.Save(path);

            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        private Bitmap DrawRectanglesInBitMap()
        {
             var bmp = new Bitmap(Settings.ImageWidth, Settings.ImageHeight);
             var graphics = Graphics.FromImage(bmp);

            graphics.FillRectangle(Brushes.White, 0, 0, Settings.ImageWidth, Settings.ImageHeight);
            foreach (var rectangle in CloudLayouter.Rectangles)
            {
                graphics.FillRectangle(brushForRect, rectangle);
                graphics.DrawRectangle(penForFrame, rectangle);
            }

            return bmp;
        }
    }
}