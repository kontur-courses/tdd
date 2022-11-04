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
        public FrequencyTags tags = new FrequencyTags(
            "q, w, e, r, t, y, u, i, o, p, [, ], a, s, d, f, g, h, j, k, l, ;, z, x, c, v, b, n, m, git, git, git, git, git, kontur, kontur, kontur, kontur, kontur, kontur, kontur, kontur, kontur, youtube, youtube, youtube, youtube, inst, C#, C#, Java, JS, React, a, f, f, e, e, e, r, t, r, ht, ht, we, wew, wew, eww, ere, erer, thth, rhyeh, eryerger, ergerg, 3rg, r3g, fwf, wef, wefwef, 3, 3r, r32, Jaba SC, ef, some, thing"
                .Split(", "));

        public readonly IDictionary<string, int> sizeDictionary = new Dictionary<string, int>();

        public DivideTags(int sizeAvgTagSize, FrequencyTags? tags = null)
        {
            if (tags == null)
                tags = this.tags;
            if (sizeAvgTagSize == 0)
                throw new ArgumentNullException();

            var repeatDictionary = tags.GetDictionary();

            foreach (var tagKey in repeatDictionary.Keys)
            {
                var tagCoefficient = (double)repeatDictionary[tagKey] / tags.Count;
                sizeDictionary[tagKey] = (int)(sizeAvgTagSize * tagCoefficient);
            }
        }
    }
}
