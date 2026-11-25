using System.Drawing;

namespace TagsCloudVisualization.Abstractions
{
    public interface ISpiral
    {
        public Point Current { get; }

        public Point Next();

        public void Reset();

    }
}
