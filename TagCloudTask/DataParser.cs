using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagCloud
{
    public class DataParser
    {
        public Dictionary<string, int> FrequencyDict = new();
        public string Filepath { get; }
        private int _totalWords;

        public DataParser(string filepath="")
        {
            Filepath = filepath;
        }

        public void ParseText(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text is null or empty");
            
            var words = text
                .Split(
                    new char[]
                    {
                        ' ', '\n', '.', ','
                    },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.ToLower());
                
            foreach (var word in words)
            {
                _totalWords++;
                if (FrequencyDict.ContainsKey(word))
                    FrequencyDict[word]++;
                else
                    FrequencyDict.Add(word, 1);
            }
        }

        public void ParseFile()
        {
            using StreamReader reader = File.OpenText(Filepath);
            var tempLine = "";
            while ((tempLine = reader.ReadLine()) != null)
            {
                ParseText(tempLine);
            }
        }

        IEnumerable<ValueTuple<string, double>> GetWordsWithFrequency()
        {
            FrequencyDict = FrequencyDict
                .OrderByDescending(x => x.Value)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value
                );
            return FrequencyDict.Select(pair => (pair.Key, (double)pair.Value / _totalWords));
        }
    }
}