using NUnit.Framework;

namespace BowlingGame.Solved
{
    //Перевод на C# оригинального решения от дядюшки Боба
    public class Game
    {
        private readonly int[] rolls = new int[21];
        private int currentRoll;

        public void Roll(int pins)
        {
            rolls[currentRoll++] = pins;
        }

        public int GetScore()
        {
            int score = 0;
            int frameIndex = 0;
            for (int frame = 0; frame < 10; frame++)
            {
                if (IsStrike(frameIndex))
                {
                    score += 10 + GetStrikeBonus(frameIndex);
                    frameIndex++;
                }
                else if (IsSpare(frameIndex))
                {
                    score += 10 + GetSpareBonus(frameIndex);
                    frameIndex += 2;
                }
                else
                {
                    score += GetSumOfBallsInFrame(frameIndex);
                    frameIndex += 2;
                }
            }
            return score;
        }

        private bool IsStrike(int frameIndex)
        {
            return rolls[frameIndex] == 10;
        }

        private int GetSumOfBallsInFrame(int frameIndex)
        {
            return rolls[frameIndex] + rolls[frameIndex + 1];
        }

        private int GetSpareBonus(int frameIndex)
        {
            return rolls[frameIndex + 2];
        }

        private int GetStrikeBonus(int frameIndex)
        {
            return rolls[frameIndex + 1] + rolls[frameIndex + 2];
        }

        private bool IsSpare(int frameIndex)
        {
            return rolls[frameIndex] + rolls[frameIndex + 1] == 10;
        }
    }
    
    [TestFixture]
	public class Game_Tests
    {
        private Game game;

        [SetUp]
        public void SetUp()
        {
            game = new Game();
        }

        [Test]
        public void TestGutterGame()
        {
            RollMany(20, 0);
            Assert.AreEqual(0, game.GetScore());
        }

        [Test]
        public void TestAllOnes()
        {
            RollMany(20,1);
            Assert.AreEqual(20, game.GetScore());
        }

        [Test]
        public void TestOneSpare()
        {
            RollSpare();
            game.Roll(3);
            RollMany(17,0);
            Assert.AreEqual(16,game.GetScore());
        }

        [Test]
        public void TestOneStrike()
        {
            RollStrike();
            game.Roll(3);
            game.Roll(4);
            RollMany(16, 0);
            Assert.AreEqual(24, game.GetScore());
        }

        [Test]
        public void TestPerfectGame()
        {
            RollMany(12,10);
            Assert.AreEqual(300, game.GetScore());
        }

        private void RollMany(int n, int pins)
        {
            for (int i = 0; i < n; i++)
                game.Roll(pins);
        }

        private void RollStrike()
        {
            game.Roll(10);
        }

        private void RollSpare()
        {
            game.Roll(5);
            game.Roll(5);
        }
    }
}
