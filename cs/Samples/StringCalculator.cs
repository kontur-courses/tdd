using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples
{
	public class StringCalculator
	{
		public int Add(string expr)
		{
			var delimiters = ParseDelimiters(ref expr);
			var parsedNumbers = ParseNumbers(expr, delimiters);
			FailOnNegatives(parsedNumbers);
			return parsedNumbers.Any()
				? parsedNumbers.Sum() : 0;
		}

		public static List<int> ParseNumbers(string expr, char[] delimiters)
		{
			return expr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
				.Select(int.Parse).ToList();
		}

		public static void FailOnNegatives(List<int> numbers)
		{
			var negatives = numbers.Where(n => n < 0).ToList();
			if (negatives.Any())
				throw new ArgumentException(
					"negatives not allowed: " + string.Join(", ", negatives));
		}

		public static char[] ParseDelimiters(ref string text)
		{
			if (!text.StartsWith("//") || text.Length <= 2)
				return new[] { ',', '\n' };
			var delimiter = text[2];
			text = text.Split('\n')[1];
			return new[] { delimiter };
		}
	}
}
