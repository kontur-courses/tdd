using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class FrequencyTags
    {
        private IDictionary<string, int> repeatDictionary = new Dictionary<string, int>();
        public IDictionary<string, int> GetDictionary() => repeatDictionary;

        public int Count;

        public FrequencyTags(string[] splitStrings)
        {
            if (splitStrings == null)
                throw new ArgumentNullException();
            foreach (var splitString in splitStrings)
            {
                if (!repeatDictionary.ContainsKey(splitString))
                    repeatDictionary[splitString] = 0;
                repeatDictionary[splitString]++;
                Count++;
            }

            repeatDictionary = repeatDictionary.OrderByDescending(order => order.Value)
                .ToDictionary(x => x.Key, y => y.Value);
        }
    }
}
