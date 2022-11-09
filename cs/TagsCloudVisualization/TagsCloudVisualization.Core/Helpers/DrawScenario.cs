using System.Drawing;

namespace TagsCloudVisualization.Core.Helpers
{
    public class DrawScenario
    {
        public Size ImageSize { get; set; } = new Size(1200, 1200);
        public int RectanglesCount { get; set; }
        public int MaxHeight { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MinWidth { get; set; }
        public string ImageName { get; set; }
        public string SavePath { get; set; }

        public DrawScenario(int rectanglesCount, int maxHeight, int minHeight, int maxWidth, int minWidth, string imageName, Size imageSize)
        {
            ImageSize = imageSize;
            RectanglesCount = rectanglesCount;
            MaxHeight = maxHeight;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MinWidth = minWidth;
            ImageName = imageName;
            SavePath = Path.Combine(Directory.GetCurrentDirectory(), $"{ImageName}.bmp");
        }
        public DrawScenario(int rectanglesCount, int maxHeight, int minHeight, int maxWidth, int minWidth, string imageName)
        {
            RectanglesCount = rectanglesCount;
            MaxHeight = maxHeight;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MinWidth = minWidth;
            ImageName = imageName;
            SavePath = Path.Combine(Directory.GetCurrentDirectory(), $"{ImageName}.bmp");
        }

        public void DrawAndSave()
        {
            var visualizer = new BitmapSaver(ImageSize);
            var circularCloud = new CircularCloudLayouter(new Point(ImageSize.Width / 2, ImageSize.Height / 2));
            var rectanglesCloud = GenerateRectanglesCloud(circularCloud);

            visualizer.Draw(rectanglesCloud);
            visualizer.Save(SavePath);
        }

        private IEnumerable<Rectangle> GenerateRectanglesCloud(CircularCloudLayouter circularCloud)
        {
            var rnd = new Random();

            var rectangles = new List<Rectangle>();
            for (var i = 0; i < RectanglesCount; i++)
            {
                var size = new Size(rnd.Next(MinWidth, MaxWidth), rnd.Next(MinHeight, MaxHeight));
                rectangles.Add(circularCloud.PutNextRectangle(size));
            }

            return rectangles;
        }

    }
}
