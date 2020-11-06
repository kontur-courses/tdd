using System.Drawing;

namespace TagsCloudVisualization
{
    public class PlacementInfo
    {
        public Rectangle Rectangle { get; }
        public bool ToVertical { get; }
        public bool ToHorizontal { get; }
        public double Distance { get; }

        public PlacementInfo(bool toVertical, Size size, Point location, double distance)
        {
            ToVertical = toVertical;
            ToHorizontal = !toVertical;
            Rectangle = new Rectangle {Size = size, Location = location};
            Distance = distance;
        }
        
        public PlacementInfo(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }
}