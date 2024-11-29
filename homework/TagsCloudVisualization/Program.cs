using System.Drawing;
using System.Drawing.Imaging;
using TagsCloud;

namespace TagsCloudVisualization;

public static class Program
{
    private const string StandardInputPath = "in.txt";
    private const string StandardOutputPath = "out.png";

    public static void Main()
    {
        var mode = ReadMode();
        Console.Clear();

        try
        {
            var sizes = mode switch
            {
                ProgramMode.ReadFromFile => ReadSizesFromFile(StandardInputPath),
                ProgramMode.Generate => GenerateSizes(new Random(), ReadGenerationSettings()),
                _ => throw new Exception("Invalid program mode."),
            };

            Run(sizes, StandardOutputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void Run(IEnumerable<Size> sizes, string outputPath)
    {
        var layouter = new SpiralCircularCloudLayouter(Point.Empty);
        var visualizer = new RectanglesVisualizer();
        var rectangles = sizes.Select(layouter.PutNextRectangle);

        visualizer.AddRectangles(rectangles);
        visualizer.GetImage().Save(outputPath, ImageFormat.Png);

        Console.WriteLine($"Image saved to {outputPath}.");
        Console.ReadKey();
    }

    private static ProgramMode ReadMode()
    {
        Console.WriteLine("Select mode:");
        Console.WriteLine();
        Console.WriteLine("[0]: Read rectangle sizes from in.txt");
        Console.WriteLine("[1]: Generate rectangle sizes");

        while (true)
        {
            var keyPressed = Console.ReadKey();

            switch (keyPressed.Key)
            {
                case ConsoleKey.D0:
                    return ProgramMode.ReadFromFile;
                case ConsoleKey.D1:
                    return ProgramMode.Generate;
                default:
                    Console.SetCursorPosition(0, Console.CursorTop);
                    break;
            }
        }
    }

    private static SizesGenerationSettings ReadGenerationSettings()
    {
        Console.WriteLine("Input values:");
        Console.WriteLine("<rectangles count> <min width> <max width> <min height> <max height>");

        while (true)
        {
            var input = Console.ReadLine()!;

            try
            {
                return SizesGenerationSettings.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private static IEnumerable<Size> GenerateSizes(Random random, SizesGenerationSettings generationSettings)
    {
        for (var i = 0; i < generationSettings.Count; i++)
        {
            var width = random.Next(generationSettings.MinWidth, generationSettings.MaxWidth);
            var height = random.Next(generationSettings.MinHeight, generationSettings.MaxHeight);
            yield return new Size(width, height);
        }
    }

    private static IEnumerable<Size> ReadSizesFromFile(string path)
    {
        var sizeParser = new SizeParser();
        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            if (sizeParser.TryParse(line, out var size))
            {
                yield return size;
            }
            else
            {
                Console.WriteLine($"Unable to parse line '{line}'");
            }
        }
    }
}
