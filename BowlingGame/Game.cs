using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
	public class Game
	{
		public void Roll(int pins)
		{

		}

		public int GetScore()
		{
			throw new NotImplementedException();
		}
	}


	[TestFixture]
	public class Game_should : ReportingTest<Game_should>
	{
		// ReSharper disable once UnusedMember.Global
		public static string Names = "ВАШИ ФАМИЛИИ ЧЕРЕЗ ПРОБЕЛ"; // Ivanov Petrov

		[Test]
		public void HaveZeroScore_BeforeAnyRolls()
		{
			new Game()
				.GetScore()
				.Should().Be(0);
		}
	}
}
