using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var words = new List<string>
            {
                "морской", "экологический", "рыба", "дипломатический", "губернатор", "миг", "велеть", "министерство", "расположить", "агрессивный", 
                "шахматный", "изложить", "появляться", "выслушать", "проверка", "страстной", "каменный", "испытать", "вырваться", "скромный", "занятый",
                "повезти", "решать", "опытный", "путь", "понадобиться", "подойти", "крошечный", "год", "знак"
            };
            
            var random = new Random();
            var image = new Bitmap(1920, 1080);
            var circ = new CircularCloudLayouter(new Point(image.Width / 2, image.Height / 2));
            var brush = Graphics.FromImage(image);
            var pen = new Pen(Color.Black);
            
            brush.DrawEllipse(new Pen(Color.Red, 3), image.Width / 2, image.Height / 2, 3, 3);
            foreach (var word in words)
            {
                var fontSize = random.Next(16, 32);
                var rectangle = circ.PullNextRectangle(new Size(word.Length * fontSize, (int)(fontSize * 1.3)));
                brush.DrawRectangle(pen, rectangle);
                brush.DrawString(word, new Font(FontFamily.GenericMonospace, fontSize), new SolidBrush(Color.Goldenrod), rectangle);
            }
            image.Save("Sample.png");
        }
    }
}