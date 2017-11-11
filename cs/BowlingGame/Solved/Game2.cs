using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
	public class Game
	{
		private int score = 0;
		private int prevPins = 0;
		private bool isFirstThrowInFrame = true;
		private PrevRollsState state = PrevRollsState.Simple; 
		public void Roll(int pins)
		{
			score += GetRollScore(state, pins);
			var isSpare = !isFirstThrowInFrame && pins + prevPins == 10;
			var isStrike = pins == 10;
			state = GetNextState(state, isSpare, isStrike);
			isFirstThrowInFrame = isStrike || !isFirstThrowInFrame;
			prevPins = pins;
		}

		private PrevRollsState GetNextState(PrevRollsState prevRollsState, bool isSpare, bool isStrike)
		{
			var prevWasStrike = 
				prevRollsState == PrevRollsState.NonStrikeThenStrike 
				||prevRollsState == PrevRollsState.StrikeThenStrike;
			if (isStrike)
				return prevWasStrike 
					? PrevRollsState.StrikeThenStrike 
					: PrevRollsState.NonStrikeThenStrike;
			else if (isSpare)
				return PrevRollsState.Spare;
			else
				return prevWasStrike 
					? PrevRollsState.StrikeThenSimple 
					: PrevRollsState.Simple;
		}

		private int GetRollScore(PrevRollsState prevRollsState, int pins)
		{
			switch (prevRollsState)
			{
				case PrevRollsState.Spare:
				case PrevRollsState.NonStrikeThenStrike:
				case PrevRollsState.StrikeThenSimple:
					return 2*pins;
				case PrevRollsState.StrikeThenStrike:
					return 3*pins;
				case PrevRollsState.Simple:
					return pins;
				default:
					throw new Exception(prevRollsState.ToString());
			}
		}

		public int GetScore()
		{
			return score;
		}
	}

	internal enum PrevRollsState
	{
		Simple,
		Spare,
		NonStrikeThenStrike,
		StrikeThenStrike,
		StrikeThenSimple
	}


	[TestFixture]
	public class Game_should : ReportingTest<Game_should>
	{
		// ReSharper disable once UnusedMember.Global
		public static string Names = "Egorov"; // Ivanov Petrov

		[Test]
		public void HaveZeroScore_BeforeAnyRolls()
		{
			new Game()
				.GetScore()
				.Should().Be(0);
		}

		[TestCase(new[] { 3 }, ExpectedResult = 3, TestName = "single throw")]
		[TestCase(new[] { 4, 2 }, ExpectedResult = 6, TestName = "two simple throws")]
		[TestCase(new[] { 4, 2, 8 }, ExpectedResult = 14, TestName = "three simple throws")]
		[TestCase(new[] { 4, 2, 8, 1 }, ExpectedResult = 15, TestName = "four simple throws")]
		[TestCase(new[] { 8, 2 }, ExpectedResult = 10, TestName = "spare")]
		[TestCase(new[] { 8, 2, 1 }, ExpectedResult = 12, TestName = "simple throw after spare")]
		[TestCase(new[] { 8, 2, 1, 2 }, ExpectedResult = 14, TestName = "two simple throws after spare")]
		[TestCase(new[] { 8, 2, 9, 1 }, ExpectedResult = 29, TestName = "two spares")]
		[TestCase(new[] { 8, 2, 9, 1, 1 }, ExpectedResult = 31, TestName = "simple throw after two spares")]
		[TestCase(new[] { 10 }, ExpectedResult = 10, TestName = "strike")]
		[TestCase(new[] { 10, 1 }, ExpectedResult = 12, TestName = "simple throw after strike")]
		[TestCase(new[] { 10, 1, 2 }, ExpectedResult = 16, TestName = "two simple throws after strike")]
		public int GetScore_After(int[] throws)
		{
			var game = new Game();
			foreach (var pins in throws)
				game.Roll(pins);
			return game.GetScore();
		}
	}
}
