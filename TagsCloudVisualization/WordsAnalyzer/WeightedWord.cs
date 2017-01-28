namespace TagsCloudVisualization.WordsAnalyzer
{
    public class WeightedWord
    {
        public int Weight { get; set; }
        public string Word { get; set; }

        public WeightedWord(string word, int weight)
        {
            Weight = weight;
            Word = word;
        }
    }
}