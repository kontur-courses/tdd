using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public readonly struct WordToDraw
    {
        public WordToDraw(string word, Font font, Brush brush)
        {
            Word = word;
            Font = font;
            Brush = brush;
        }

        public string Word { get; }
        public Font Font { get; }
        public Brush Brush { get; }
    }
}