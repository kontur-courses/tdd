using System.Collections.Generic;

namespace TagsCloudVisualization.WordsAnalyzer
{
    public interface IWordsAnalyzer
    {
        IEnumerable<WeightedWord> Сonsider(IEnumerable<string> words);
    }
}
