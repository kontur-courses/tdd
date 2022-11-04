using System;
using System.Collections.Generic;
using System.Linq;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        public List<int> Scores;
        public int Attempt = 0;
        public int Frame = 1;
        public int Strike = 0;
        public bool IsSpare = false;
        public bool IsDoubleStrike = false;

        public Game()
        {
            Scores = new List<int>();
        }
        
        public void Roll(int pins)
        {
            if (pins > 10 || pins < 0)
                throw new ArgumentException();

            if (Frame == 11 && (Strike != 0 || IsSpare))
            {
                if (IsDoubleStrike)
                {
                    Scores.Add(pins);
                    Scores.Add(pins);
                    IsDoubleStrike = false;
                }
                Scores.Add(pins);
                Strike = 0;
                IsSpare = false;
                return;
            }

            if (Frame > 10)
                throw new Exception("Game is over");
            
            Attempt += 1;

            if (IsSpare)
            {
                Scores.Add(pins);
                IsSpare = false;
            }

            if (Strike > 0)
            {
                if (IsDoubleStrike)
                {
                    Scores.Add(pins);
                    IsDoubleStrike = false;
                }
                Scores.Add(pins);
                Strike -= 1;
            }

            Scores.Add(pins);

            if (Attempt % 2 == 0)
            {
                Frame += 1;
                if (pins + Scores[Scores.Count - 2] == 10)
                    IsSpare = true;
            }
            else if (pins == 10)
            {
                if (Strike == 1)
                    IsDoubleStrike = true;
                
                Strike = 2;
                Frame += 1;
                Attempt += 1;
            }
                
        }

        public int GetScore()
        {
            if (Scores.Count == 0)
                return 0;

            return Scores.Sum();
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void ReturnScore_AfterOneRoll()
        {
            var game = new Game();
            game.Roll(2);
            game.GetScore().Should().Be(2);
        }
        
        [Test]
        public void ReturnScoreWithSpareBonus_AfterThreeRolls()
        {
            var game = new Game();
            game.Roll(2);
            game.Roll(8);
            game.Roll(5);
            game.GetScore().Should().Be(20);
        }
        
        [Test]
        public void ReturnScoreWithStrikeBonus_AfterThreeRolls()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(2);
            game.Roll(2);
            game.GetScore().Should().Be(18);
        }
        
        [Test]
        public void ReturnScoreWithStrikeAndSpareBonus()
        {
            var game = new Game();
            game.Roll(2);
            game.Roll(8);
            game.Roll(10);
            game.Roll(2);
            game.Roll(2);
            game.GetScore().Should().Be(38);
        }
        
        [Test]
        public void ReturnScoreWithSpareBonusAttempt_AfterMoreThanTenRolls()
        {
            var game = new Game();
            for (var i = 0; i < 9; i++)
            {
                game.Roll(2);
                game.Roll(2);
            }
            
            game.Roll(2);
            game.Roll(8);
            game.Roll(2);

            game.GetScore().Should().Be(48);
        }
        
        [Test]
        public void ReturnScoreWithStrikeBonusAttempt_AfterMoreThanTenRolls()
        {
            var game = new Game();
            for (var i = 0; i < 9; i++)
            {
                game.Roll(2);
                game.Roll(2);
            }
            
            game.Roll(10);
            game.Roll(2);

            game.GetScore().Should().Be(48);
        }
        
        [Test]
        public void ReturnScore_OnAllRollsAreStrike()
        {
            var game = new Game();
            
            for (var i = 0; i < 10; i++)
                game.Roll(10);

            game.Roll(10);
            game.GetScore().Should().Be(300);
        }
        
        [TestCase(-1)]
        [TestCase(11)]
        public void ThrowArgumentException_OnInvalidPinsNumbers(int value)
        {
            Assert.Throws<ArgumentException>(
                () => new Game().Roll(value)
            );
        }

        [Test]
        public void ThrowException_AfterMoreThanTenFrame()
        {
            var game = new Game();
            for (var i = 0; i < 10; i++)
            {
                game.Roll(2);
                game.Roll(2);
            }
            
            var ex = Assert.Catch<Exception>(
                () => game.Roll(1)
            );
            
            Assert.AreEqual(ex.Message, "Game is over");
        }
    }
}
