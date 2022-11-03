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
        public FrequencyTags tags = new FrequencyTags(File
            .ReadAllText("C:\\Users\\Lodgent\\source\\repos\\WinFormsApp1\\WinFormsApp1\\git_tags.txt").Split(", "));
        public readonly IDictionary<string, int> sizeDictionary = new Dictionary<string, int>();

        public DivideTags(int sizeAvgTagSize, FrequencyTags? tags = null)
        {
            if (tags==null) 
                tags=this.tags;
            if (sizeAvgTagSize == 0)
                throw new ArgumentNullException();
            var screenSize = sizeAvgTagSize * tags.Count;
            var repeatDictionary = tags.GetDictionary();
            
            foreach (var tagKey in repeatDictionary.Keys)
            {
                var tagCoefficient = (double)repeatDictionary[tagKey] / tags.Count;
                sizeDictionary[tagKey] =(int)(screenSize * tagCoefficient);
            }
        }
    }
}
