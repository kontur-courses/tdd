using System;
using System.Drawing;
using TagsCloudVisualization.Renders;
using TagsCloudVisualization.TagClouds;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cloud = new CircleTagCloud(new Point(-300, 300));
            var visualizer = new SolidVisualizer(cloud, Color.Orange);
            for (var i = 0; i < 20; i++)
                cloud.PutNextRectangle(new Size(40, 4));
            new FileCloudRender(cloud, visualizer, "yeah.png").Render();
            Console.WriteLine("Hello Viz!");
        }
    }
}