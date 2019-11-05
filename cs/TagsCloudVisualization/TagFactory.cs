using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagFactory : ITagFactory
    {
        private const string MutualFont = "Bahnschrift SemiLight";

        private static readonly Dictionary<TagType, TagStyle> tagStyleByTagType = new Dictionary<TagType, TagStyle>
        {
            [TagType.Central] = new TagStyle(Color.White, new Font(MutualFont, 60)),
            [TagType.Large] = new TagStyle(Color.FromArgb(255, 102, 0), new Font(MutualFont, 22)),
            [TagType.Medium] = new TagStyle(Color.FromArgb(212, 85, 0), new Font(MutualFont, 18)),
            [TagType.Small] = new TagStyle(Color.FromArgb(160, 90, 44), new Font(MutualFont, 13))
        };

        public IEnumerable<Tag> GetTags(string[] cloudStrings, Graphics graphics,
                                        CircularCloudLayouter circularCloudLayouter)
        {
            for (var i = 0; i < cloudStrings.Length; i++)
            {
                var text = cloudStrings[i];
                var tagType = i == 0 ? TagType.Central : (TagType)(i % 3);
                var tagStyle = tagStyleByTagType[tagType];

                var sizeF = graphics.MeasureString(text, tagStyle.Font);
                var size = new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));

                var tagBox = circularCloudLayouter.PutNextRectangle(size);

                yield return new Tag(text, tagStyle, tagBox);
            }
        }
    }
}