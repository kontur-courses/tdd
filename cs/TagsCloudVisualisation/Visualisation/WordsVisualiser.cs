using System;
using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public sealed class WordsLayouterAsset : BaseCloudVisualiser
    {
        private readonly ICircularCloudLayouter layouter;
        private WordToDraw currentWord;

        public void DrawWord(WordToDraw toDraw)
        {
            var graphics = Graphics ?? Graphics.FromImage(new Bitmap(1, 1));
            var wordSize = graphics.MeasureString(toDraw.Word, toDraw.Font);
            var computedPosition = layouter.PutNextRectangle(Size.Ceiling(wordSize));

            if (wordSize.Height > computedPosition.Size.Height && wordSize.Width > computedPosition.Size.Width)
                throw new ArgumentException("Actual word size is larger than computed values");

            var offset = (wordSize - computedPosition.Size) / 2;
            var wordPosition = new RectangleF(computedPosition.X - offset.Width, computedPosition.Y - offset.Height,
                wordSize.Width, wordSize.Height);

            currentWord = toDraw;
            PrepareAndDraw(wordPosition);
        }

        public WordsLayouterAsset(ICircularCloudLayouter layouter) : base(layouter.CloudCenter)
        {
            this.layouter = layouter;
            OnDraw += rect => Graphics.DrawString(currentWord.Word, currentWord.Font, currentWord.Brush, rect);
        }
    }
}