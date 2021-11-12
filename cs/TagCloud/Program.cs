using TagCloud.Layouting;
using TagCloud.Saving;
using TagCloud.Visualization;
using TagCloud_TestDataGenerator;

namespace TagCloud
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bitmapSaver = new BitmapSaver();
            var layouter = new CircularLayouter();
            var visualizer = new Visualizer(new Drawer());

            var tagCloud = new TagCloud(layouter, bitmapSaver, visualizer);

            foreach (var size in RectangleSizeGenerator.GetNextNFixedSizes(1500))
                tagCloud.PutNextTag(size);

            tagCloud.SaveBitmap();
        }
    }
}