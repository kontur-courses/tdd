using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudVisualization.Visualizer;
using TagsCloudVisualization.WordsAnalyzer;

namespace TagsCloudVisualization
{
    public class TagLayouter
    {
        private readonly CircularCloudLayouter cloudLayouter;


        private int minFont;
        public int MinFont
        {
            get { return minFont; }
            set
            {
                if (value <= 0 || value > MaxFont)
                {
                    throw new ArgumentException("invalid value for minimum font");
                }
                minFont = value;
            }
        }

        private int maxFont = int.MaxValue;
        public int MaxFont
        {
            get { return maxFont; }
            set
            {
                if (value <= 0 || value < MinFont)
                {
                    throw new ArgumentException("invalid value for maximum font");
                }
                maxFont = value;
            }
        }

        public TagLayouter(CircularCloudLayouter cloudLayouter, int minFont, int maxFont)
        {
            this.cloudLayouter = cloudLayouter;
            MinFont = minFont;
            MaxFont = maxFont;
        }

        public IEnumerable<Tag> GetTags(List<WeightedWord> words)
        {
            if (!words.Any())
            {
                yield break;
            }
            var maxWeight = words.Max(w => w.Weight);
            var minWeight = words.Min(w => w.Weight);
            foreach (var word in words)
            {
                yield return GetNextTag(word, minWeight, maxWeight);
            }
        }


        private Tag GetNextTag(WeightedWord weightedWord, int minWeight, int maxWeight)
        {
            var fontSize = GetFontSize(weightedWord.Weight, minWeight, maxWeight);
            var font = new Font(FontFamily.GenericSerif, fontSize);
            var frameSize = TextRenderer.MeasureText(weightedWord.Word, font);
            var frame = cloudLayouter.PutNextRectangle(frameSize);
            return new Tag(weightedWord.Word, frame, font);
        }

        private int GetFontSize(int currentWeight, int minWeight, int maxWeight)
        {
            var coef = (double)(currentWeight - minWeight) / (maxWeight - minWeight + 1);
            return (int)Math.Round(MinFont + coef * (MaxFont - MinFont));
        }
    }
}
