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
        private int score;
        private int strikeFactor = 1;
        private int spareFactor = 1;

        private List<int> rolls = new List<int>();
        private int currentFrame = 0;

        public void Roll(int pins)
        {
            rolls.Add(pins);
//
//            if (pins == 10 && rolls.Count % 2 == 1)
//                rolls.Add(0);

            currentFrame += (rolls.Count % 2 == 0) ? 1 : 0;
        }

        public int GetScore()
        {
            var tempSum = 0;
            for (int i = 0; i < rolls.Count; i++)
            {
                tempSum += rolls[i];
                score += rolls[i]* strikeFactor*spareFactor;
                strikeFactor -= (strikeFactor > 1) ? 1 : 0;
                spareFactor -= (spareFactor > 1) ? 1 : 0;
                if (tempSum == 10)
                {
                    spareFactor++;
                    tempSum = 0; 
                }

                if (rolls[i] == 10)
                    strikeFactor++;


                //  4 6 0 1 = 
//                score += currentSum*strikeFactor;
//
//                if (currentSum == 10 && strikeFactor < 3)
//                    strikeFactor++;
//
//                if (rolls.ElementAtOrDefault(i + 1) == 0)
//                    strikeFactor = 1;
            }

            return score;

        }
    }


    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        private Game game;

        private void Frame(params int[] points)
        {
            foreach (var point in points)
            {
                game.Roll(point);
            }
        }

        [SetUp]
        public void SetUp()
        {
            game = new Game();
        }
        

        [TestCase(4, 6, 3, ExpectedResult = 16)]
        [TestCase(10,3, ExpectedResult = 16)]
        [TestCase(10, ExpectedResult = 10)]
        [TestCase(4, ExpectedResult = 4)]
        [TestCase(1,4, ExpectedResult = 5)]
        [TestCase(10,1,1,ExpectedResult = 14)]
        [TestCase(ExpectedResult = 0)]
        [TestCase(4,6,0,1,ExpectedResult = 11)]
        [TestCase(10,10,10,10,5,ExpectedResult = 105)]
        [TestCase(3,7,10,5,ExpectedResult = 40)]
        public int GetScore(params int[] rolls)
        {
            Frame(rolls);
            return game.GetScore();
        }
        
    }
}