using System.Drawing;

namespace TagsCloudVisualization;

public interface ICircularCloudLayoutTracer
{
    void TraceRectangle(Rectangle rectangle);

    void TraceCirclePoint(Point point);

    void TraceShifting(Point from, Point to);
}