using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud2.Interfaces;

namespace TagCloud2
{
    public class SpiralTagCloud
    {
        public readonly ITagCloudEngine Engine;
        public readonly ITagCloduBitmapDrawer Drawer;
        public readonly IDataParser Parser;

        private double _minFrequancyFraction, _maxFrequancyFraction;
        private readonly int _maxFontSize;
        private readonly int _minFontSize;
        
        private List<Tuple<string, int>> _tagWithSize = new();
        public List<Tuple<string, int>> TagWithSize { get => _tagWithSize; }

        public SpiralTagCloud(
            ITagCloudEngine engine,
            ITagCloduBitmapDrawer drawer,
            IDataParser parser,
            int minFontSize, 
            int maxFontSize)
        {
            Engine = engine;
            Drawer = drawer;
            Parser = parser;
            _minFontSize = minFontSize;
            _maxFontSize = maxFontSize;
        }
        
        public void CreateTagCloud()
        {
            var wordsWithDict = Parser.GetWordsWithFrequency();
            _minFrequancyFraction = Parser.MinValue;
            _maxFrequancyFraction = Parser.MaxValue;
            foreach ((string tag, double frequancy) tuple in wordsWithDict)
            {
                var coef = (tuple.frequancy - _minFrequancyFraction) /
                           (_maxFrequancyFraction - _minFrequancyFraction);
                
                var fontSize = (int)Math.Round(_minFontSize + ((_maxFontSize - _minFontSize) * coef));
                var stringSize = Drawer.GetStringInRectangleSize(tuple.tag, fontSize);
                
                _tagWithSize.Add(Tuple.Create(tuple.tag, fontSize));
                
                Engine.PutNextRectangle(stringSize.ToSize());
            }
        }

        public Rectangle PutNextRectangle(Size sizeOfRectangle)
        {
            return Engine.PutNextRectangle(sizeOfRectangle);
        }
    }
}