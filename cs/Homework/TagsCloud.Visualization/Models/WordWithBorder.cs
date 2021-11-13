using System;
using System.Drawing;

namespace TagsCloud.Visualization.Models
{
    public class WordWithBorder : IDisposable
    {
        public Word Word { get; init; }
        public Font Font { get; init; }
        public Rectangle Border { get; init; }

        public void Dispose()
        {
            Font?.Dispose();
        }
    }
}