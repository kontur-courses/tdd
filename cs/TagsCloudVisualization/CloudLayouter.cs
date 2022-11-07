using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
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