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
                circularCloudDrawing.DrawString(str, font);
            circularCloudDrawing.SaveImage(filename);
        }

        private string _fontFamilyName = "Arial";
        
        public void GenerateFirstTagCloud()
        {
            GenerateTagCloud(new List<Tuple<string, Font>>
            {
                new Tuple<string, Font>("Привет", new Font(_fontFamilyName, 30)),
                new Tuple<string, Font>("В инете сижу", new Font(_fontFamilyName, 14)),
                new Tuple<string, Font>("Чем занимаешься?", new Font(_fontFamilyName, 17)),
                new Tuple<string, Font>("Как дела?", new Font(_fontFamilyName, 33)),
                new Tuple<string, Font>("Фаня", new Font(_fontFamilyName, 42)),
                new Tuple<string, Font>("Я", new Font(_fontFamilyName, 20)),
                new Tuple<string, Font>("Круто", new Font(_fontFamilyName, 22)),
                new Tuple<string, Font>("Вот так", new Font(_fontFamilyName, 19)),
                new Tuple<string, Font>("Играю", new Font(_fontFamilyName, 16)),
                new Tuple<string, Font>("Коготь", new Font(_fontFamilyName, 33)),
                new Tuple<string, Font>("Шарпей", new Font(_fontFamilyName, 42)),
                new Tuple<string, Font>("Трейсер", new Font(_fontFamilyName, 31)),
                new Tuple<string, Font>("Доктор", new Font(_fontFamilyName, 23)),
                new Tuple<string, Font>("Пшено", new Font(_fontFamilyName, 35)),
                new Tuple<string, Font>("Мяч", new Font(_fontFamilyName, 32)),
                new Tuple<string, Font>("Ирбит", new Font(_fontFamilyName, 16)),
                new Tuple<string, Font>("Екб", new Font(_fontFamilyName, 20)),
                new Tuple<string, Font>("Сон", new Font(_fontFamilyName, 40))
            }, new Size(600, 600), "1.png");
        }
        
        public void GenerateSecondTagCloud()
        {
            var list = new List<Tuple<string, Font>>();
            for (var i = 1; i < 100; i++) 
                list.Add(new Tuple<string, Font>((i ).ToString(), new Font(_fontFamilyName, i)));
            GenerateTagCloud(list, new Size(1600, 1600), "2.png");
        }
        
        public void GenerateThirdTagCloud()
        {
            var list = new List<Tuple<string, Font>>();
            for (var i = 1; i < 5000; i++) 
                list.Add(new Tuple<string, Font>((6).ToString(), new Font(_fontFamilyName, 15)));
            GenerateTagCloud(list, new Size(2400, 2400), "3.png");
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            var examples = new ExamplesCircularCloud();
            examples.GenerateFirstTagCloud();
            examples.GenerateSecondTagCloud();
            examples.GenerateThirdTagCloud();
        }
    }
}