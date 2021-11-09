using System.Drawing;

namespace TagsCloudVisualization
{
    public enum Placement
    {
        VerticalSide,
        HorizontalSide,
        Corner
    }
    
    public class PlacementLocation
    {
        public PointF Location;
        public PointF LeftVertex;
        public PointF RightVertex;
        public Placement Placement;

        public PlacementLocation(PointF leftVertex, PointF location, PointF rightVertex, Placement placement)
        {
            Location = location;
            LeftVertex = leftVertex;
            RightVertex = rightVertex;
            Placement = placement;
        }
    }
}