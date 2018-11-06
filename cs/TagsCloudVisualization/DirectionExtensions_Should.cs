using FluentAssertions;
using NUnit.Framework;
using static TagsCloudVisualization.Direction;

namespace TagsCloudVisualization
{
    public class DirectionExtensions_Should
    {
        [TestCase(Down, new[] {Down, Left, Up, Right})]
        [TestCase(Left, new[] {Left, Up, Right, Down})]
        [TestCase(Up, new[] {Up, Right, Down, Left})]
        [TestCase(Right, new[] {Right, Down, Left, Up})]
        public void HasCorrectClockwiseCycle(Direction start, Direction[] expectedCycle)
        {
            var cycle = start.ToClockwiseCycle();

            cycle.Should().BeEquivalentTo(expectedCycle);
        }

        [TestCase(Down, new[] {Down, Right, Up, Left})]
        [TestCase(Left, new[] {Left, Down, Right, Up})]
        [TestCase(Up, new[] {Up, Left, Down, Right})]
        [TestCase(Right, new[] {Right, Up, Left, Down})]
        public void HasCorrectCounterClockwiseCycle(Direction start, Direction[] expectedCycle)
        {
            var cycle = start.ToCounterClockwiseCycle();

            cycle.Should().BeEquivalentTo(expectedCycle);
        }
    }
}
