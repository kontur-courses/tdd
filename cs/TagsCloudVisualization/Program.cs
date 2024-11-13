namespace TagsCloudVisualization; 

public class Program
{
    private const int RectangleCount = 32;
    private const string ImagesPath = @"..\..\..\Images\";
    
    static void Main(string[] args)
    {
        TagsCloudRunner.Create(RectangleCount, ImagesPath).Run();
    }
}