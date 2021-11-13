using System;
using System.Drawing;

namespace TagsCloud.Visualization.Models
{
    public class WordWithBorder : IDisposable
    {
        public Word Word { get; set; }
        public Font Font { get; set; }
        public Rectangle Border { get; set; }

        public void Dispose()
        {
            Font?.Dispose();
        }
    }
}