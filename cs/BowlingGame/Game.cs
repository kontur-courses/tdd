using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
	public class Game
	{
		public int scores = 0;
		private int previousRoll = 0;
		private int count = 1;
		private bool spareOrStrike = false;

		public void Roll(int pins)
		{
			if (pins > 10 || pins + previousRoll > 10)
				throw new ArgumentException();
			
			scores += spareOrStrike? 2*pins : pins;
			spareOrStrike = false;
			if (pins == 10 || pins + previousRoll == 10)
			{
				spareOrStrike = true;
			}
			if (count % 2 == 0 && count != 18)
				previousRoll = 0;
			else
			{
				previousRoll = pins;
			}
			count++;
			if (pins == 10)
				Roll(0);
		}

		public int GetScore()
		{
			return scores;
		}
	}

	[TestFixture]
	public class Game_should : ReportingTest<Game_should>
	{
		private static void DoRolls(Game game, int[] rolls)
		{
			foreach (var roll in rolls)
				game.Roll(roll);
		}

		[Test]
		public void HaveManyRolls()
		{
			var game = new Game();
			var rolls = new[] {7,2,9};
			DoRolls(game, rolls);
			game.GetScore().Should().Be(18);
		}

		[Test]
		public void HaveStrikeRolls()
		{
			var game = new Game();
			var rolls = new[] {10, 1};
			DoRolls(game, rolls);
			game.GetScore().Should().Be(12);
		}

		[Test]
		public void HaveOneRoll()
		{
			var game = new Game();
			game.Roll(5);
			game.GetScore().Should().Be(5);
		}


		[Test]
		public void ThrowException_WhenAnyRollsMoreThat10()
		{
			var game = new Game();
			Action act = () => game.Roll(11);
			act.Should().Throw<ArgumentException>();
		}

		[Test]
		public void ThrowException_WhenSumOfTwoFirstRollsMoreThat10()
		{
			var game = new Game();
			game.Roll(4);
			Action act = () => game.Roll(7);
			act.Should().Throw<ArgumentException>();
		}

		[Test]
		public void GetBonusPointForSpare()
		{
			var game = new Game();
			var rolls = new[] { 6,4,5};
			DoRolls(game, rolls);
			game.scores.Should().Be(20);
		}
		[Test]
		public void FullGame()
		{
			var game = new Game();
			var rolls = new[] { 1,4,4,5,6,4,5,5,10,0,1,7,3,6,4,10,2,8,6 };
			DoRolls(game, rolls);
			game.GetScore().Should().Be(130);
		}
		[Test]
		public void StartGame()
		{
			var game = new Game();
			var rolls = new[] { 1,4,4,5,6,4,5,5 };
			DoRolls(game, rolls);
			game.GetScore().Should().Be(39);
		}
	}
}