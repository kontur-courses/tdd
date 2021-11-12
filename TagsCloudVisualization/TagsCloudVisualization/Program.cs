namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var amount = 1;
            for (int i = 0; i < amount; i++)
                DemoImageGenerator.GenerateCircularTagCloud(100, new ArchimedeanSpiral());

            var spiral = new ArchimedeanSpiral();
            for (int i = 0; i < 5; i++)            
                DemoImageGenerator.GenerateSpiral(i, spiral);            
        }
    }
}
