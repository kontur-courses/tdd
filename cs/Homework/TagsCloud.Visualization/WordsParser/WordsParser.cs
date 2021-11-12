using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloud.Visualization.WordsFilter;

namespace TagsCloud.Visualization.WordsParser
{
    public class WordsParser
    {
        private readonly IWordsFilter wordsFilter;
        private const string WordsPattern = @"\W+";

        public WordsParser(IWordsFilter wordsFilter)
        {
            this.wordsFilter = wordsFilter;
        }
        
        public Dictionary<string, int> CountWordsFrequency(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            
            return Regex.Split(text.ToLower(), WordsPattern)
                .Where(wordsFilter.IsWordValid)
                .GroupBy(s => s)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}