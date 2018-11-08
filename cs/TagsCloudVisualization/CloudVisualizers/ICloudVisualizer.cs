using System.Drawing;
using TagsCloudVisualization.CloudLayouts;

namespace TagsCloudVisualization.CloudVisualizers
{
    public interface ICloudVisualizer
    {
        ICloudLayout Cloud { get; set; }
        Bitmap GenerateImage(Size imageSize);
    }
}