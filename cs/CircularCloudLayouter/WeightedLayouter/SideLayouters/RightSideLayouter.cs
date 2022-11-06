using System.Drawing;
using CircularCloudLayouter.WeightedLayouter.Forming;

namespace CircularCloudLayouter.WeightedLayouter.SideLayouters;

public class RightSideLayouter : WeightedSideLayouter
{
    public RightSideLayouter(Point center, FormFactor formFactor) : base(center, formFactor)
    {
    }

    protected override double RatioCoefficient => 1;

    public override Rectangle GetNextRectangle(Size rectSize)
    {
        var resPos = FindNextRectPos(rectSize.Height, Center.Y);
        return new Rectangle(
            Center.X + resPos.Relative,
            resPos.Absolute,
            rectSize.Width, rectSize.Height
        );
    }

    public override void UpdateWeights(Rectangle rect)
    {
        var weight = rect.Right - Center.X;
        if (weight < 0)
            return;
        SideWeights.UpdateGreaterWeights(new WeightedSegment(rect.Top, rect.Bottom, weight));
    }
}