using TagCloudTask.Layouting;
using TagCloudTask.Saving;
using TagCloudTask.Visualization;
using TestDataGenerator;

namespace TagCloudTask
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bitmapSaver = new BitmapSaver();
            var layouter = new CircularLayouter();
            var visualizer = new Visualizer(new Drawer());

            var tagCloud = new TagCloud(layouter, bitmapSaver, visualizer);

            foreach (var size in RectangleSizeGenerator.GetNextNFixedSizes(256))
                tagCloud.PutNextTag(size);

            tagCloud.SaveBitmap();
        }
    }
}