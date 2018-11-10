using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Models;

namespace TagCloud.Utility.Models
{
    public class TagReader
    {
        public readonly TagGroups TagGroups;

        /// <summary>
        /// Creates Tag Reader with 3 groups:
        /// Big(10% of all words, with size (W: letter * 60, H: 150)
        /// Average(30% of all words, with size (W: letter * 40, H: 100)
        /// Small(60% of all words, with size (W: letter * 20, H: 50)
        /// </summary>
        public TagReader()
        {
            TagGroups = new TagGroups();
            TagGroups.AddSizeGroup("Big", new FrequencyGroup(0.9, 1), new Size(80, 150));
            TagGroups.AddSizeGroup("Average", new FrequencyGroup(0.6, 0.9), new Size(60, 100));
            TagGroups.AddSizeGroup("Small", new FrequencyGroup(0, 0.6), new Size(30, 50));
        }

        /// <summary>
        /// Creates Tag Reader with custom groups
        /// </summary>
        /// <param name="groups">Size groups</param>
        public TagReader(TagGroups tagGroups)
        {
            TagGroups = tagGroups;
        }

        public List<TagItem> GetTags(string pathToWords)
        {
            var words = WordReader
                .ReadAllWords(pathToWords)
                .Select(word => word.ToLower());
            var frequencyDictionary = GetFrequencyOfWord(words);

            return GetTagItems(frequencyDictionary);
        }

        /// <summary>
        /// Return list of tag items
        /// </summary>
        /// <param name="frequencyDictionary"> Words with their frequency</param>
        /// <returns></returns>
        private List<TagItem> GetTagItems(Dictionary<string, int> frequencyDictionary)
        {
            var items = new List<TagItem>();
            var maxRepeats = frequencyDictionary.Values.Max();
            foreach (var group in TagGroups.Groups)
            {
                var sizeGroup = group.Value;
                var wordsInGroup = frequencyDictionary
                    .Where(pair => sizeGroup.Contains((double)pair.Value / maxRepeats))
                    .Select(pair => pair.Key);
                var tags = wordsInGroup
                    .Select(word => new TagItem(word, sizeGroup.GetSizeForWord(word)))
                    .ToList();
                items.AddRange(tags);
            }

            return items;
        }

        private static Dictionary<string, int> GetFrequencyOfWord(IEnumerable<string> words)
        {
            var frequencyDictionary = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (frequencyDictionary.ContainsKey(word))
                    frequencyDictionary[word]++;
                else
                    frequencyDictionary.Add(word, 1);
            }

            return frequencyDictionary;
        }
    }
}