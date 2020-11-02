using System;
using System.Drawing;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.CloudSamples
{
    public static class VisualizationSamples
    {
        private static readonly Point Center = new Point(320, 360);

        public static void SampleWithSameRectangleSize()
        {
            var cloudLayouter = new CircularCloudLayouter(Center);

            for (var i = 0; i < 49; i++)
                cloudLayouter.PutNextRectangle(new Size(50, 50));

            var visualizer = new CircularCloudLayouterVisualizer(cloudLayouter);
            visualizer.SaveInFile("same_rectangle_cloud_sample");
        }

        public static void SampleWithIncreasingRectangleSize()
        {
            var cloudLayouter = new CircularCloudLayouter(Center);

            for (var i = 0; i < 80; i++)
                cloudLayouter.PutNextRectangle(new Size(i, i));

            var visualizer = new CircularCloudLayouterVisualizer(cloudLayouter);
            visualizer.SaveInFile("increasing_rectangle_cloud_sample");
        }

        public static void SampleWithRandomRectangleSize()
        {
            var cloudLayouter = new CircularCloudLayouter(Center);
            var rnd = new Random();

            for (var i = 0; i < 50; i++)
                cloudLayouter.PutNextRectangle(new Size(rnd.Next(100), rnd.Next(100)));

            var visualizer = new CircularCloudLayouterVisualizer(cloudLayouter);
            visualizer.SaveInFile("random_rectangle_cloud_sample");
        }
    }
}