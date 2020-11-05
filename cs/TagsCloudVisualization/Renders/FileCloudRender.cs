using System.Drawing;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization.Renders
{
    public class FileCloudRender : IRender
    {
        private readonly string fileName;
        private readonly IVisualizer visualizer;
        private int safeZone;

        public FileCloudRender(IVisualizer visualizer, string fileName, int safeZone = 24)
        {
            this.visualizer = visualizer;
            this.fileName = fileName;
            this.safeZone = safeZone;
        }

        public void Render()
        {
            var leftUpBound = visualizer.Cloud.LeftUpBound;
            var rightDownBound = visualizer.Cloud.RightDownBound;

            var image = new Bitmap(
                rightDownBound.X - leftUpBound.X,
                rightDownBound.Y - leftUpBound.Y);
            var graphics = Graphics.FromImage(image);
            visualizer.Draw(graphics);
            image.Save(fileName);
        }
    }
}
