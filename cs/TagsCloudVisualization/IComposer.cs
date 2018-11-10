namespace TagsCloudVisualization
{
    public interface IComposer<out T>
    {
        T GetNextPoint();
    }
}
