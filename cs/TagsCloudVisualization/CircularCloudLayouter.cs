using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> storage;
    private readonly CloudCompressor compressor;
    private readonly CirclePositionDistributor distributor;

    public CircularCloudLayouter(Point center) : this(center, [])
    { }

    private CircularCloudLayouter(Point center, List<Rectangle> storage)
    {
        this.storage = storage;
        distributor = new(center);
        compressor = new(center, storage);
    }

    public static CircularCloudLayouter CreateLayouterWithStartRectangles(Point center, List<Rectangle> storage)
    {
        return new CircularCloudLayouter(center, storage);
    }

    public Rectangle PutNextRectangle(Size nextRectangle)
    {
        ValidateRectangleSize(nextRectangle);

        var inserted = PutRectangleWithoutIntersection(nextRectangle);
        var rectangleWithOptimalPosition =  compressor.CompressCloudAfterInsertion(inserted);

        storage.Add(rectangleWithOptimalPosition);

        return rectangleWithOptimalPosition;
    }
    
    public Rectangle PutRectangleWithoutIntersection(Size forInsertionSize)
    {
        bool isIntersected;
        Rectangle forInsertion;
        do
        {
            var possiblePosition = distributor.GetNextPosition();
            forInsertion = new Rectangle(possiblePosition, forInsertionSize);
            isIntersected = forInsertion.IntersectedWithAnyFrom(storage);
        }
        while (isIntersected);

        return forInsertion;
    }

    private static void ValidateRectangleSize(Size forInsertion)
    {
        if (forInsertion.Width <= 0 || forInsertion.Height <= 0)
            throw new ArgumentException($"Rectangle has incorrect size: width = {forInsertion.Width}, height = {forInsertion.Height}");
    }
}