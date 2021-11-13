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
                "повезти", "решать", "опытный", "путь", "понадобиться", "подойти", "крошечный", "год", "знак",
                "разобраться", "целовать", "исполнить", "беседовать", "армия", "столкнуться", "первоначальный", "средневековый",
                "команда", "махнуть", "терпеть", "стиль", "бежать", "срочный", "прошлый", "комиссия", "легкий", "отказать", "население", 
                "столица", "возрасти", "молоко", "должен", "упомянуть", "видный", "лагерь", "сохраниться", "избежать", "собраться", "поцеловать",
                "палец", "фотография", "приводить", "нормативный", "статистический", "татарский", "выборы", "засмеяться", "придавать", 
                "медицинский", "решительный", "хозяин", "акт", "шутка", "бить", "свидетель", "ценность", "окончание", "оптимальный",
                "поговорить", "орган", "будущий", "средневековый", "лекарственный", "ответственность", "цифра", "сниться", "исходить", "обстановка", "хотеть"
            };
            
            var random = new Random();
            var image = new Bitmap(2000, 2000);
            var center = new Point(image.Width / 2, image.Height / 2);
            var layouter = new CircularCloudLayouter(center);
            var brush = Graphics.FromImage(image);

            Console.WriteLine("1 - облако слов из листа");
            Console.WriteLine("2 - случайный размер от 5 до 70 по каждой оси");
            Console.WriteLine("3 - фиксированный размер 40");
            var answer = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            
            if (answer == 1)
            {
                foreach (var word in words)
                {
                    var fontSize = random.Next(16, 48);
                    var rectangle = layouter.PutNextRectangle(new Size(word.Length * fontSize, (int)(fontSize * 1.3)));
                    brush.DrawRectangle(new Pen(Color.Black), rectangle);
                    brush.DrawString(word, new Font(FontFamily.GenericMonospace, fontSize),
                        new SolidBrush(Color.Goldenrod), rectangle);
                }
            }
            else
            {
                for (var i = 0; i < 100; i++)
                {
                    Rectangle rectangle;
                    
                    if (answer == 2)
                        rectangle = layouter.PutNextRectangle(new Size(random.Next(5, 70), random.Next(5, 70)));
                    else
                        rectangle = layouter.PutNextRectangle(new Size(40, 40));
                    
                    brush.FillRectangle(new SolidBrush(Color.Green), rectangle);
                    brush.DrawString(i.ToString(), new Font(FontFamily.GenericMonospace, 6), 
                        new SolidBrush(Color.Red), rectangle);
                    brush.DrawRectangle(new Pen(Color.Black), rectangle);
                }
            }

            brush.DrawEllipse(new Pen(Color.Red, 3), center.X, center.Y, 3, 3);
            image.Save("Sample.png");
            
        }
    }
}