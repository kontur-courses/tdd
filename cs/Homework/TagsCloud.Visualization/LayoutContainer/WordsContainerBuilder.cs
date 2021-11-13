using System.Collections.Generic;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsSizeService;

namespace TagsCloud.Visualization.LayoutContainer
{
    public class WordsContainerBuilder
    {
        private readonly FontFactory.FontFactory fontFactory;
        private readonly ICloudLayouter layouter;
        private readonly List<WordWithBorder> words = new();
        private readonly IWordsSizeService wordsSizeService;

        public WordsContainerBuilder(
            ICloudLayouter layouter,
            IWordsSizeService wordsSizeService,
            FontFactory.FontFactory fontFactory)
        {
            this.layouter = layouter;
            this.wordsSizeService = wordsSizeService;
            this.fontFactory = fontFactory;
        }

        public WordsContainerBuilder Add(Word word, int min, int max)
        {
            var font = fontFactory.GetFont(word, min, max);
            var size = wordsSizeService.CalculateSize(word, font);
            var rectangle = layouter.PutNextRectangle(size);
            words.Add(new WordWithBorder {Word = word, Font = font, Border = rectangle});
            return this;
        }

        public WordsContainer Build()
        {
            return new() {Items = words};
        }
    }
}