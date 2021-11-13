using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class WordsHandler
    {
        private ICloudLayouter cloudLayouter;
        public WordsHandler(ICloudLayouter cloudLayouter)
        {
            this.cloudLayouter = cloudLayouter;
        }

        public Template Handle(List<(string, Font)> words)
        {
            throw new NotImplementedException();
        }
        
    }
}