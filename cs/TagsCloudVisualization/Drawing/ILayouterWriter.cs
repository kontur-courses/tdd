using TagsCloudVisualization.Geom;

namespace TagsCloudVisualization.Drawing
{
    public interface ILayouterWriter
    {
        void WriteLayout(CircularCloudLayouter layouter);
    }
}
