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
            var bitmapSaver = new BitmapToDesktopSaver();
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var visualizer = new Visualizer(new MarkupDrawer(), new CloudDrawer());

            var tagCloud = new TagCloud(layouter, bitmapSaver, visualizer);

            foreach (var size in DataGenerator.GetNextSize())
                tagCloud.PutNextTag(size);

            tagCloud.MarkupVisualize();
            //tagCloud.Visualize();
            tagCloud.Save();
        }
    }
}