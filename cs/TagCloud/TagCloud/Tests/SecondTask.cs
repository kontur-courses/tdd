using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace TagCloud.Tests
{
    public class TextCreator
    {
        private const string Text1 = "Алгоритм для этой задачи можете выдумать самостоятельно " +
                                      "или развить следующую несложную идею: Можно располагать " +
                                      "прямоугольники по очереди вдоль достаточно плотной раскручивающейся " +
                                      "спирали с центром в точке center";

        private const string Text2 = "Сделай визуализацию получившейся раскладки. Сгенерируй 2-3 раскладки с" +
                                      "разными параметрами, сохрани изображения в репозитории и создай в " +
                                      "директории проекта README.md файл включающий эти изображения. Подсказки" +
                                      " Создать изображение new Bitmap(...). Получить объект Graphics для рисования " +
                                      "на изображении: Graphics.FromImage(bitmap). Сохранить изображение в" +
                                      " файл bitmap.Save(...;";

        [TestCase(Text1, "image1.png")]
        [TestCase(Text2, "image2.png")]
        public void Should_CreateImages(string text, string fname)
        {
            using (var drawer = new Drawer(new Size(1080, 720)))
            {
                var rnd = new Random();
                foreach (var word in text.Split(' ', '.', ',', ':').Where(s => !string.IsNullOrEmpty(s)))
                    if (word.Length > 0)
                        drawer.DrawWord(word, new Font("Courier New", rnd.Next(10, 40)));
                drawer.SaveImg(fname);
            }
        }

        [TestCase("image3.png")]
        [TestCase("image4.png")]
        public void Should_CreateCircleShape_With1000RandomGeneratedRectangles(string fname)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(600, 600));
            var rnd = new Random();
            OnFailDrawer.DrawOriginOrientedRectangles(
                new Size(1200, 1200), 
                Enumerable
                    .Range(0, 100)
                    .Select(num => new Size(rnd.Next(30, 100), rnd.Next(30, 100)))
                    .Select(size => cloudLayouter.PutNextRectangle(size)),
                fname
                );
        }
    }
}