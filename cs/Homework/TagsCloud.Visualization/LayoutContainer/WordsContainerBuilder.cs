using System.Collections.Generic;
using TagsCloud.Visualization.FontService;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsSizeService;

namespace TagsCloud.Visualization.LayoutContainer
{
    public class WordsContainerBuilder
    {
        private readonly IFontService fontService;
        private readonly ICloudLayouter layouter;
        private readonly List<WordWithBorder> words = new();
        private readonly IWordsSizeService wordsSizeService;

        public WordsContainerBuilder(ICloudLayouter layouter, IFontService fontService,
            IWordsSizeService wordsSizeService)
        {
            this.layouter = layouter;
            this.fontService = fontService;
            this.wordsSizeService = wordsSizeService;
        }

        public WordsContainerBuilder Add(Word word)
        {
            var font = fontService.CalculateFont(word);
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