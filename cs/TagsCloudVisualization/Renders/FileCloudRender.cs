using System.Drawing;
using TagsCloudVisualization.TagClouds;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization.Renders
{
    public class FileCloudRender : IRender
    {
        private readonly TagCloud cloud;
        private readonly string fileName;
        private readonly IVisualizer visualizer;
        private int safeZone;

        public FileCloudRender(TagCloud cloud, IVisualizer visualizer, string fileName, int safeZone = 24)
        {
            this.cloud = cloud;
            this.visualizer = visualizer;
            this.fileName = fileName;
            this.safeZone = safeZone;
        }

        public void Render()
        {
            var leftUpBound = cloud.LeftUpBound;
            var rightDownBound = cloud.RightDownBound;

            var image = new Bitmap(
                rightDownBound.X - leftUpBound.X,
                rightDownBound.Y - leftUpBound.Y);
            var graphics = Graphics.FromImage(image);
            visualizer.Draw(graphics);
            image.Save(fileName);
        }
    }
}