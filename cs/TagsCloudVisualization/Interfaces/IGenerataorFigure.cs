using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface IGenerataorFigure
    {
        public Size GetSize(int minValue, int maxValue);
    }
}