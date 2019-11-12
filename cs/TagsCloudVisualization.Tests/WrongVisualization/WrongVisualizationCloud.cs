using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.TagClouds;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.Tests.WrongVisualization
{
    public class WrongVisualizationCloud : TagCloud
    {
        private static readonly Dictionary<TagType, TagStyle> tagStyleByTagType = new Dictionary<TagType, TagStyle>
        {
            [TagType.Normal] = new TagStyle(Color.LightGray, null),
            [TagType.SecondWrong] = new TagStyle(Color.Crimson, null),
            [TagType.FirstWrong] = new TagStyle(Color.Teal, null)
        };

        private readonly Rectangle[] allRectangles;
        private readonly (Rectangle, Rectangle) wrongRectanglesPair;

        public WrongVisualizationCloud((Rectangle, Rectangle) wrongRectanglesPair,
                                       IEnumerable<Rectangle> allRectangles = null)
        {
            if (wrongRectanglesPair.Item1.Size.IsEmpty || wrongRectanglesPair.Item2.Size.IsEmpty)
                throw new ArgumentException("One of rectangles has empty size", nameof(wrongRectanglesPair));

            this.allRectangles = allRectangles?.ToArray() ?? new Rectangle[0];
            this.wrongRectanglesPair = wrongRectanglesPair;
        }

        protected override Color BackgroundColor => Color.White;

        protected override Action<Tag> GetTagDrawer(Graphics graphics) =>
            tag =>
            {
                using var pen = new Pen(tag.Style.TextColor) { Alignment = PenAlignment.Inset };
                graphics.DrawRectangle(pen, tag.TagBox);
            };

        protected override IEnumerable<Tag> GetTags(string[] cloudStrings, Graphics graphics,
                                                    ICloudLayouter circularCloudLayouter) =>
            from rectangle in allRectangles
            let tagType = wrongRectanglesPair.Item1 == rectangle ? TagType.FirstWrong :
                          wrongRectanglesPair.Item2 == rectangle ? TagType.SecondWrong :
                                                                   TagType.Normal
            let tagStyle = tagStyleByTagType[tagType]
            select new Tag(null, tagStyle, rectangle);
    }
}