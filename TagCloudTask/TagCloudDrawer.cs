// using System;
// using System.Drawing;
// using System.Linq;
//
// namespace TagCloud
// {
//     public class TagCloudDrawer
//     {
//         private readonly DataParser _dataParser;
//         private TagCloudBuilder _builder;
//         private Font _font;
//         private readonly int _minFontSize, _maxFontSize;
//         public Bitmap Bitmap { get; }
//         public Graphics Graphics { get; }
//
//         public TagCloudDrawer(
//             int minFontSize, 
//             int maxFontSize,
//             TagCloudBuilder builder, 
//             DataParser dataParser,
//             Font font)
//         {
//             this._minFontSize = minFontSize;
//             this._maxFontSize = maxFontSize;
//             _builder = builder;
//             _dataParser = dataParser;
//             _font = font;
//             
//             //Bitmap = new Bitmap(_builder.width, _builder.height);
//             //Graphics = Graphics.FromImage(Bitmap);
//         }
//         
//         //public 
//
//         private int GetFontSize(double frequency)
//         {
//             var fontSizeDiff = _maxFontSize - _minFontSize;
//             var maxMinusDiff = _maxFontSize - fontSizeDiff;
//             var freqMin = _dataParser.FrequencyDict.Values.Min();
//             var freqMax = _dataParser.FrequencyDict.Values.Max();
//             var magicCoef = (frequency-freqMin)/(freqMax-freqMin);
//             
//             var result = (int)(fontSizeDiff+(maxMinusDiff*magicCoef));
//             return result;
//         }
//     }
// }