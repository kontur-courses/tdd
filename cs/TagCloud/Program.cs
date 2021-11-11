using System.Drawing;
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
            var layouter = new CircularLayouter(new Point(0, 0));
            var visualizer = new Visualizer(new Drawer());

            var tagCloud = new TagCloud(layouter, bitmapSaver, visualizer);

            foreach (var size in RectangleSizeGenerator.GetNextNFixedSize(1500))
                tagCloud.PutNextTag(size);

            tagCloud.SaveToBitmap(true, true);
        }
    }
}