using System.Drawing;
using System.Collections.Generic;
using TagsCloudVisualization;

public class CircularCloudLayouter : ITagCloudLayouter
{
    private readonly Point cloudCenter;
    private readonly IList<Rectangle> _rectangels = new List<Rectangle>();
    private readonly SpiralPointsGenerator spiralPointsGenerator;
    

    public CircularCloudLayouter(Point cloudCenter)
    {
        this.cloudCenter = cloudCenter;
        spiralPointsGenerator = new SpiralPointsGenerator(cloudCenter);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        foreach (var point in spiralPointsGenerator.GetPoints())
        {
            var rectangle = new Rectangle(point, rectangleSize);
            if (_rectangels.Any(x => x.IntersectsWith(rectangle)))
                continue;
            
            _rectangels.Add(rectangle);
            return rectangle;
        }

        return new Rectangle(cloudCenter, rectangleSize);
    }
    
    static void Main(string[] args)
    {
        var myCenter = new Point(0, 0);
        var test = new CircularCloudLayouter(myCenter);
        var ans = test.spiralPointsGenerator.GetPoints().Take(100000).ToList();
        //Console.WriteLine(test.spiralPointsGenerator.GetPoints().Take(100).ToList());
    }
}