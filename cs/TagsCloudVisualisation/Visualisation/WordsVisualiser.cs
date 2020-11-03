using System;
using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public sealed class WordsLayouterAsset : BaseCloudVisualiser
    {
        private readonly ICircularCloudLayouter layouter;
        private WordToDraw currentWord;

        public WordsLayouterAsset DrawWord(WordToDraw toDraw)
        {
            var graphics = Graphics ?? Graphics.FromImage(new Bitmap(100, 100));
            var wordSize = graphics.MeasureString(currentWord.Word, currentWord.Font);
            var computedPosition = layouter.PutNextRectangle(Size.Ceiling(wordSize));

            if (wordSize.Height > computedPosition.Size.Height && wordSize.Width > computedPosition.Size.Width)
                throw new ArgumentException("Actual word size is larger than computed values");

            var offset = (wordSize - computedPosition.Size) / 2;
            var wordPosition = new RectangleF(computedPosition.X - offset.Width, computedPosition.Y - offset.Height,
                wordSize.Width, wordSize.Height);

            currentWord = toDraw;
            Draw(wordPosition);
            return this;
        }

        public WordsLayouterAsset(ICircularCloudLayouter layouter) : base(layouter.CloudCenter)
        {
            this.layouter = layouter;
        }

        protected override void DrawPrepared(RectangleF rectangle) =>
            Graphics.DrawString(currentWord.Word, currentWord.Font, currentWord.Brush, rectangle);
    }
}