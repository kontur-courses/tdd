using System.Drawing;
using TagsCloudVisualization.Infrastructure.Environment;

namespace TagsCloudVisualization.Graphic
{
    public interface IEnvironmentToImageConverter
    {
        public Image EnvironmentImage<T>(Environment<T> environment);
    }
}