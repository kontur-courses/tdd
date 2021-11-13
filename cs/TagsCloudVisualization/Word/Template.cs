using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Template
    {
        private readonly List<WordParameter> words;

        public Template()
        {
            
        }

        public Template(IEnumerable<WordParameter> words)
        {
            this.words = words.ToList();
        }

        public void Add(WordParameter wordParameter)
        {
            words.Add(wordParameter);
        }

        public IEnumerable<WordParameter> GetWords()
        {
            return words;
        }
    }
}