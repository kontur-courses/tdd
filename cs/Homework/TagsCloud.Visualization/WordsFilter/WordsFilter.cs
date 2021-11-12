using System.Collections.Generic;

namespace TagsCloud.Visualization.WordsFilter
{
    public class WordsFilter : IWordsFilter
    {
        // TODO Move to config file and read on init
        private readonly HashSet<string> prepositions = new() {"a", "and", "or", "to", "in", "into", "on", "for", "by", "during"};
        
        public bool IsWordValid(string word) => prepositions.Contains(word);
    }
}