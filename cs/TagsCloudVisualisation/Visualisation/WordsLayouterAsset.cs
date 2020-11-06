using System;
using System.Drawing;

namespace TagsCloudVisualisation.Visualisation
{
    public sealed class WordsLayouterAsset : BaseCloudVisualiser
    {
        private readonly ICircularCloudLayouter layouter;
        private readonly int scale;
        private WordToDraw currentWord;

        public void DrawWord(WordToDraw toDraw)
        {
            toDraw = new WordToDraw(toDraw.Word, WordToDraw.MultiplyFontSize(toDraw.Font, scale), toDraw.Brush);

            var graphics = Graphics ?? Graphics.FromHwnd(IntPtr.Zero);
            var wordSize = graphics.MeasureString(toDraw.Word, toDraw.Font);
            if (graphics != Graphics)
                graphics.Dispose();

            var computedPosition = layouter.PutNextRectangle(Size.Ceiling(wordSize));

            if (wordSize.Height > computedPosition.Size.Height || wordSize.Width > computedPosition.Size.Width)
                throw new ArgumentException("Actual word size is larger than computed values");

            var offset = (wordSize - computedPosition.Size) / 2;
            var wordPosition = new RectangleF(computedPosition.X - offset.Width, computedPosition.Y - offset.Height,
                wordSize.Width, wordSize.Height);

            currentWord = toDraw;
            PrepareAndDraw(wordPosition);
        }

        public WordsLayouterAsset(ICircularCloudLayouter layouter, int scale) : base(layouter.CloudCenter)
        {
            this.layouter = layouter;
            this.scale = scale;
            OnDraw += rect => Graphics.DrawString(currentWord.Word, currentWord.Font, currentWord.Brush, rect);
        }
    }
}