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
            var template = new Template();
            var fakeImage = new Bitmap(1,1); 
            var graphics = Graphics.FromImage(fakeImage);
            foreach (var (word, font) in words)
            {
                var wordSize = graphics.MeasureString(word, font).ToSize() + new Size(1, 1);
                var wordRectangle = cloudLayouter.PutNextRectangle(wordSize);
                template.Add(new WordParameter(word, wordRectangle, font));
            }

            return template;
        }
        
    }
}