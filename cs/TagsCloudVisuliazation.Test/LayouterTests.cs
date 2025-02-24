using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisuliazation.Test.Extension;

namespace TagsCloudVisuliazation.Test;

public class LayouterTests
{
    [TestCase(3, -2, TestName = "X ShouldBeMoreThan 0")]
    [TestCase(-3, 2, TestName = "Y ShouldBeMoreThan 0")]
    public void CircularCloudLayouter_CheckConstructor(int y, int x)
    {
        Action action = () => new CircularCloudLayouter(new Point(y, x));
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CircularCloudLayouter_Rectangles_ShouldBeHave_Neighbor()
    {
        var layoter = new CircularCloudLayouter(new Point(250, 250));

        CheckAllRectanglesHasNeighbor(layoter);
    }

    [TestCase(30, 30, 300, 30)]
    [TestCase(30, 30, 30, 300)]
    [TestCase(1, 1, 30, 300)]
    [TestCase(1, 1, 1, 1)]
    public void CircularCloudLayouter_Rectangle_ShouldBe_Sealing(int widthRec1, int heightRec1, int widthRec2,
        int heightRec2)
    {
        var layoter = new CircularCloudLayouter(new Point(250, 250));
        var list = new List<Rectangle>();
        list.Add(layoter.PutNextRectangle(new Size(widthRec1, heightRec1)));

        var rec = layoter.PutNextRectangle(new Size(widthRec2, heightRec2));


        rec.HasNeighbor(list).Should().BeTrue();
    }

    private void CheckAllRectanglesHasNeighbor(CircularCloudLayouter layoter)
    {
        var list = new List<Rectangle>();
        for (var i = 0; i < 30; i++)
        {
            var cur = layoter.PutNextRectangle(new Size(30, 30));
            if (list.Count != 0) cur.HasNeighbor(list).Should().BeTrue();

            list.Add(cur);
        }
    }
}