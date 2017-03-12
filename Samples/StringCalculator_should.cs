using System;
using NUnit.Framework;

namespace Samples
{
	[TestFixture]
	public class StringCalculator_should
	{
		private StringCalculator calc;

		[SetUp]
		public void SetUp()
		{
			calc = new StringCalculator();
		}

		[Test]
		public void returnZero_onEmptyInput()
		{
			var result = calc.Add("");
			Assert.AreEqual(0, result);
		}

		[TestCase("", ExpectedResult = 0)]
		[TestCase("42", ExpectedResult = 42)]
		[TestCase("42,13", ExpectedResult = 55)]
		[TestCase("1,2,3,4,5", ExpectedResult = 15)]
		public int sum_ComaSeparatedNumbers(string numbers)
		{
			return calc.Add(numbers);
		}

		[TestCase("1\n2\n3", ExpectedResult = 6)]
		public int sum_NewlineSeparatedNumbers(string numbers)
		{
			return calc.Add(numbers);
		}

		[TestCase("1\n2,3", ExpectedResult = 6)]
		public int sum_NumbersSeparatedByBothCommaAndNewline(string numbers)
		{
			return calc.Add(numbers);
		}

		[TestCase("//;\n1;2;3", ExpectedResult = 6)]
		[TestCase("//|\n4|5|61", ExpectedResult = 70)]
		public int sumDelimiterFromFirstSpecialLine(string numbers)
		{
			return calc.Add(numbers);
		}

		[Test]
		public void throwException_onNegativeNumbers()
		{
			Assert.Throws<ArgumentException>(() => calc.Add("-1"));
		}

		[Test]
		public void listAllNegativesInExceptionMessage_whenFail()
		{
			var ex = Assert.Throws<ArgumentException>(
				() => calc.Add("-1,2,-5,-100,8"));
			StringAssert.Contains("-1, -5, -100", ex.Message);
		}
	}
}