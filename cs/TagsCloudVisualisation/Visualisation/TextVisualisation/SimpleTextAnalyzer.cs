using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualisation.Visualisation.TextVisualisation
{
    public class SimpleTextAnalyzer : ITextAnalyzer
    {
        public int TotalWordsCount => wordsWithCount?.Count ?? 0;
        private readonly char[] separators;
        private readonly Dictionary<string, uint> wordsWithCount = new Dictionary<string, uint>();

        public SimpleTextAnalyzer(char[] separators)
        {
            this.separators = separators;
        }

        public void RegisterText(string text)
        {
            foreach (var word in text.Split(separators).Where(x => !string.IsNullOrEmpty(x)).Select(w => w.ToLower()))
                wordsWithCount[word] = wordsWithCount.GetValueOrDefault(word, 0U) + 1;
        }

        public IEnumerable<string> GetSortedWords() => wordsWithCount.OrderByDescending(x => x.Value)
            .Select(x => x.Key);
    }
}