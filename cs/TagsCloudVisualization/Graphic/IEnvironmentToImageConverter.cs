using System.Drawing;
using TagsCloudVisualization.Infrastructure.Environment;

namespace TagsCloudVisualization.Graphic
{
    public interface IEnvironmentToImageConverter<T>
    {
        public Image GetEnvironmentImage(Environment<T> environment);
    }
}