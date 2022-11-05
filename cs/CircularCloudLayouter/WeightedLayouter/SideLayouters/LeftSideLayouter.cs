using System.Drawing;
using CircularCloudLayouter.WeightedLayouter.Forming;

namespace CircularCloudLayouter.WeightedLayouter.SideLayouters;

public class LeftSideLayouter : WeightedSideLayouter
{
    public LeftSideLayouter(Point center, FormFactor formFactor) : base(center, formFactor)
    {
    }

    public override double CalculateCoefficient() =>
        base.CalculateCoefficient() / FormFactor.WidthToHeightRatio;

    public override Rectangle GetNextRectangle(Size rectSize)
    {
        var resPos = FindNextRectPos(rectSize.Height, Center.Y);
        return new Rectangle(
            Center.X - rectSize.Width - resPos.Relative,
            resPos.Absolute,
            rectSize.Width, rectSize.Height
        );
    }

    public override void UpdateWeights(Rectangle rect)
    {
        var weight = Center.X - rect.Left;
        if (weight < 0)
            return;
        SideWeights.UpdateGreaterWeights(new WeightedSegment(rect.Top, rect.Bottom, weight));
    }
}