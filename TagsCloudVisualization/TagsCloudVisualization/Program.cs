
using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ExamplesCircularCloud
    {
        public void GenerateTagCloud(IEnumerable<Tuple<string, Font>> strings, Size size, string filename)
        {
            var circularCloudDrawing = new CircularCloudDrawing(size);
            foreach (var (str, font) in strings) 
                circularCloudDrawing.DrawStrings(str, font);
            circularCloudDrawing.SaveImage(filename);
        }

        public void GenerateFirstTagCloud()
        {
            GenerateTagCloud(new List<Tuple<string, Font>>
            {
                new Tuple<string, Font>("Привет", new Font("Arial", 30)),
                new Tuple<string, Font>("В инете сижу", new Font("Arial", 14)),
                new Tuple<string, Font>("Чем занимаешься?", new Font("Arial", 17)),
                new Tuple<string, Font>("Как дела?", new Font("Arial", 33)),
                new Tuple<string, Font>("Фаня", new Font("Arial", 42)),
                new Tuple<string, Font>("Я", new Font("Arial", 20)),
                new Tuple<string, Font>("Круто", new Font("Arial", 22)),
                new Tuple<string, Font>("Вот так", new Font("Arial", 19)),
                new Tuple<string, Font>("Играю", new Font("Arial", 16)),
                new Tuple<string, Font>("Коготь", new Font("Arial", 33)),
                new Tuple<string, Font>("Шарпей", new Font("Arial", 42)),
                new Tuple<string, Font>("Трейсер", new Font("Arial", 31)),
                new Tuple<string, Font>("Доктор", new Font("Arial", 23)),
                new Tuple<string, Font>("Пшено", new Font("Arial", 35)),
                new Tuple<string, Font>("Мяч", new Font("Arial", 32)),
                new Tuple<string, Font>("Ирбит", new Font("Arial", 16)),
                new Tuple<string, Font>("Екб", new Font("Arial", 20)),
                new Tuple<string, Font>("Сон", new Font("Arial", 40))
            }, new Size(600, 600), "1.png");
        }
        
        public void GenerateSecondTagCloud()
        {
            var list = new List<Tuple<string, Font>>();
            for (var i = 1; i < 31; i++) 
                list.Add(new Tuple<string, Font>((i * 10).ToString(), new Font("Arial", i)));
            GenerateTagCloud(list, new Size(600, 600), "2.png");
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            var examples = new ExamplesCircularCloud();
            examples.GenerateFirstTagCloud();
            examples.GenerateSecondTagCloud();
        }
    }
}