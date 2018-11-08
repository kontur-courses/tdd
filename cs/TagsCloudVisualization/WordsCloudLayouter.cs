using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class WordsCloudLayouter
    {
        private CircularCloudLayouter layouter;
        private List<Word> words;
        public int Radius => layouter.Radius;

        public WordsCloudLayouter(Point centerPoint)
        {
            layouter = new CircularCloudLayouter(centerPoint);
            words = new List<Word>();
        }

        public Word PutNextWord(string text, Font font)
        {
            if (text.Length == 0)
                throw new ArgumentException("text length should be grater than zero");
            var word = new Word(text, font, layouter.PutNextRectangle(text.GetSurroundingRectangleSize(font)));
            words.Add(word);
            return word;
        }
    }
}
