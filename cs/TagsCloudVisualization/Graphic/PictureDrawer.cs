using System.Drawing;
using TagsCloudVisualization.Infrastructure.Environment;

namespace TagsCloudVisualization.Graphic
{
    public class PictureDrawer : IEnvironmentToImageConverter
    {
        public Image EnvironmentImage<T>(Environment<T> environment)
        {
            throw new System.NotImplementedException();
        }
    }
}