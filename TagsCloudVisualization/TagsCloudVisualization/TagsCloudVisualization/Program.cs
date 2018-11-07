using System;
using System.Drawing;
using TagsCloudVisualization.Curves;

namespace TagsCloudVisualization
{
	public class CloudWordsForm
	{
		static void Main()
		{
			var curve = GetCurveFromUser();
			var number = GetNumberOfRectanglesFromUser();
			var maxLength = GetMaxLengthRectangleFromUser();
			var cloud = new CircularCloudLayouter(curve);
			var rectangles = GetData(cloud, number, maxLength);
			var picture = RectangleTagsCloudVisualizer.GetPicture(rectangles, Color.Aqua);
			picture.Save("CloudTags.png");
		}

		private static int GetMaxLengthRectangleFromUser()
		{
			Console.WriteLine("����� ������������ ����� �����?");
			var input = Console.ReadLine();
			int.TryParse(input, out var maxLength);
			if (maxLength <= 0)
			{
				Console.WriteLine("����� ������ ���� ����� � �������������, ��������� ��� ���");
				maxLength = GetMaxLengthRectangleFromUser();
			}

			return maxLength;
		}

		private static int GetNumberOfRectanglesFromUser()
		{
			Console.WriteLine("������� ����?");
			var input = Console.ReadLine();
			int.TryParse(input, out var number);
			if (number <= 0)
			{
				Console.WriteLine("����� ������ ���� ����� � �������������, ��������� ��� ���");
				number = GetNumberOfRectanglesFromUser();
			}

			return number;
		}

		private static ICurve GetCurveFromUser()
		{
			var center = new Point(500, 400);
			ICurve curve;
			Console.WriteLine("����� ����� ������ �������?\n�� �����: �������, ������ ��� �������");
			var input = Console.ReadLine();

			switch (input)
			{
				case "spiral":
				case "�������":
					curve = new Spiral(0.5, Math.PI / 18, center);
					break;
				case "heart":
				case "������":
					curve = new Heart(0.2, Math.PI / 36, center);
					break;
				case "astroid":
				case "�������":
					curve = new Astroid(0.2, Math.PI / 36, center);
					break;
				default:
					Console.WriteLine("�� ������� ���������� �����, ���������� ��� ���");
					curve = GetCurveFromUser();
					break;
			}

			return curve;
		}

		public static Rectangle[] GetData(CircularCloudLayouter cloud, int number, int maxLength)
		{
			var rnd = new Random();
			for (var i = 0; i < number; i++)
				cloud.PutNextRectangle(new Size(rnd.Next(5, maxLength), rnd.Next(5, 30)));

			return cloud.GetRectangles();
		}
	}
}