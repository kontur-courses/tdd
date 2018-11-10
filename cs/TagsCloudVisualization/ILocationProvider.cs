namespace TagsCloudVisualization
{
    public interface ILocationProvider<out T>
    {
        T GetNextLocation();
    }
}
