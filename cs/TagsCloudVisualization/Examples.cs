using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Examples
    {
        private static readonly List<string> Words = new List<string>()
        {
            "camping", "cooking", "crafts", "cube", "circus", "christmas", "dance", "desert", "design", "democracy",
            "death", "dogs", "drama", "discovery", "dragons", "dream",
            "electricity", "easter", "entertainment", "emotions", "english", "education", "engineering", "egypt",
            "elephants", "fashion", "food", "fitness", "family", "film",
            "flowers", "flying", "ghost", "geography", "government", "gold", "games", "galaxy", "green", "gym",
            "gardening", "god", "health", "history", "home", "horse", "harmony", "hair"
        };

        public static List<(string, Font)> GenerateFirstExample(string fontName)
        {
            var tags = new List<(string, Font)>();
            for (var i = 5; i < 500; i++)
            {
                tags.Add(("3", new Font(fontName, 20)));
            }

            return tags;
        }

        public static List<(string, Font)> GenerateSecondExample(string fontName)
        {
            var tags = new List<(string, Font)>();
            for (var i = 30; i < 200; i++)
            {
                tags.Add((i.ToString(), new Font(fontName, 1700f / i)));
            }

            return tags;
        }

        public static List<(string, Font)> GenerateThirdExample(string fontName)
        {
            var random = new Random();
            return Words.Select(word => (word, new Font(fontName, random.Next(10, 40)))).ToList();
        }

        public static void AddWord(string word)
        {
            Words.Add(word);
        }

        public static void ClearWords()
        {
            Words.Clear();
        }
    }
}