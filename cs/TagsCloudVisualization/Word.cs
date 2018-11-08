using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Word
    {
        public string Text { get; }
        public Font Font { get; }
        public Rectangle Rectangle { get; }

        public Word(string text, Font font, Rectangle rectangle)
        {
            Text = text;
            Font = font;
            Rectangle = rectangle;
        }
    }
}
