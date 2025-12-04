using System.Drawing;

namespace TagCloud.SpiralGenerators;
public interface ISpiralPointGenerator
{
    Point GetNextPointOnSpiral();
}