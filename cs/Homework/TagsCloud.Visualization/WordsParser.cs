using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloud.Visualization
{
    public class WordsParser
    {
        private readonly HashSet<string> prepositions = new() {"a", "and", "or", "to", "in", "into", "on", "for", "by", "during"};
        private const string WordsPattern = @"\W+";

        public Dictionary<string, int> CountWordsFrequency(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            
            return Regex.Split(text.ToLower(), WordsPattern)
                .Where(s => s.Length > 3 && !prepositions.Contains(s))
                .GroupBy(s => s)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}