using System;
using System.Linq;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        //private int[] scores = new int[20];
        //private int currentFrameScore = 0;
        //private int scoresPointer = 0;
        private int[,] scores = new int[10,2];
        private int pointer = 0;
        private int rollPointer = 0;
        
        public void Roll(int pins)
        {
            scores[pointer, rollPointer] = pins;
            if (WasStrike() || rollPointer == 0 && WasSpare())
            {
                if (WasStrike() && rollPointer == 0)
                    rollPointer = 1;
                scores[pointer - 1, 0] += pins;
            }

            if (rollPointer == 1)
                pointer += 1;
            rollPointer = (rollPointer + 1) % 2;
            /*
            if (scoresPointer > 1 &&
                scores[scoresPointer - 1] == 10 &&
                scoresPointer % 2 == 0 ||
                scoresPointer > 2 &&
                scores[scoresPointer - 2] == 10 &&
                scoresPointer % 2 == 0)
            {
                scores[scoresPointer - 1] += pins;
                scoresPointer++;
            }
            else if (scoresPointer > 1 && scores[scoresPointer - 1] + scores[scoresPointer - 2] == 10)
                scores[scoresPointer - 1] += pins;
            scores[scoresPointer] = pins;
            scoresPointer++;
            */
        }

        public bool WasStrike()
        {
            if (pointer <= 0)
                return false;
            return scores[pointer - 1, 0] == 10 || scores[pointer - 1, 1] == 10;
        }

        public bool WasSpare()
        {
            if (pointer <= 0)
                return false;
            return scores[pointer - 1, 0] + scores[pointer - 1, 0] == 10;
        }

        public int GetScore()
        {
            int s = 0;
            for (var i = 0; i < 10; i++)
            for (var j = 0; j < 2; j++)
                s += scores[i, j];
            return s;
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
        public void GetScore_ReturnFirstPinsCount_AfterOneRoll()
        {
            var game = new Game();
            game.Roll(5);
            game.GetScore().Should().Be(5);
        }

        [Test]
        public void GetScore_ReturnScoreWithSpare_TenScoreAfterTwoRolls()
        {
            var game = new Game();
            game.Roll(5);
            game.Roll(5);
            game.Roll(6);
            game.GetScore().Should().Be(22);
        }

        /*[Test]
        public void GetScore_ReturnScoreWithStrike_TenScore_AfterOneRoll()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(5);
            game.Roll(5);
            game.GetScore().Should().Be(30);
        }
        */
        
    }
}