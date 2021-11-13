using System.Drawing;

namespace TagsCloud.Visualization.Models
{
    public class WordWithBorder
    {
        public Word Word { get; set; }
        public Font Font { get; set; }
        public Rectangle Border { get; set; }
    }
}