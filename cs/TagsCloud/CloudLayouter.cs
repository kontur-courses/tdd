using System.Drawing;

namespace TagsCloud
{
    public abstract class CloudLayouter<T> : ICloudLayouter<T>
    {
        public List<T> Figures { get; }

        protected CloudLayouter()
        {
            Figures = new List<T>();
        }

        public abstract T PutNextRectangle(Size rectangleSize);
    }
}