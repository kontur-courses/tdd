using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public class Tag
    {
        public string Phrase { get; }
        public Font Font { get; }
        public Rectangle Rectangle { get; }

        public Tag(string phrase, Rectangle rectangle, Font font)
        {
            Phrase = phrase;
            Font = font;
            Rectangle = rectangle;
        }
    }
}
