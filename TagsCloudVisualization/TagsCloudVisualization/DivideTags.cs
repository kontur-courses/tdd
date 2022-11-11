using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TagsCloudVisualization
{

    class DivideTags
    {
        private FrequencyTags tags = new FrequencyTags(
            "q, w, e, r, t, y, u, i, o, p, [, ], a, s, d, f, g, h, j, k, l, ;, z, x, c, v, b, n, m, git, git, git, git, git, kontur, kontur, kontur, kontur, kontur, kontur, kontur, kontur, kontur, youtube, youtube, youtube, youtube, inst, C#, C#, Java, JS, React, a, f, f, e, e, e, r, t, r, ht, ht, we, wew, wew, eww, ere, erer, thth, rhyeh, eryerger, ergerg, 3rg, r3g, fwf, wef, wefwef, 3, 3r, r32, Jaba SC, ef, some, thing, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 1, 298, 299"
                .Split(", "));

        public readonly IDictionary<string, int> sizeDictionary;

        public DivideTags(int fontMax = 120, FrequencyTags? tags = null, int fontMin = 40)
        {
            fontMin = fontMin == 40 ? (int)Math.Round((double)fontMax / 3) : 40;
            if (fontMax <= 0 || fontMin <= 0)
                throw new ArgumentNullException("sizeAvgTagSize must be > 0");
            if (fontMin >= fontMax)
                throw new ArgumentNullException("fontMax must be larger than fontMin");
            sizeDictionary = new Dictionary<string, int>();
            tags ??= this.tags;
            var repeatDictionary = tags.GetDictionary();
            foreach (var tagKey in repeatDictionary.Keys)
            {
                sizeDictionary[tagKey] = (int)Math.Round(repeatDictionary[tagKey] == repeatDictionary.Last().Value
                    ? (int)Math.Round((double)fontMin)
                    : (repeatDictionary[tagKey] / (double)repeatDictionary.First().Value) * (fontMax - fontMin) +
                      (double)fontMin);
            }
        }
    }
}
