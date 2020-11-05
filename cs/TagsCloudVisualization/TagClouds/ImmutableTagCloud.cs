using System;
using System.Drawing;

namespace TagsCloudVisualization.TagClouds
{
    public class ImmutableTagCloud : TagCloud
    {
        public ImmutableTagCloud(Rectangle[] rectangles) : base(null)
        {
            foreach (var rectangle in rectangles)
                AddRectangle(rectangle);
        }

        public override Rectangle PutNextRectangle(Size size)
        {
            throw new NotImplementedException();
        }
    }
}
