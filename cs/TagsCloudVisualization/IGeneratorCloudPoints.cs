using System.Drawing;


namespace TagsCloudVisualization
{
    public interface IGeneratorCloudPoints
    {
        Point GetNextPoint();
        Point GetCenterPoint();
    }
}
