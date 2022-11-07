using System;


namespace BowlingGame
{
    public class Game
    {
        private int _currentThrow = 0;
        private int[] throws = new int[21];

        public void Roll(int pins)
        {
            if (pins > 10 || pins < 0)
                throw new ArgumentException();

            throws[_currentThrow++] = pins;
        }

        private int CountScore()
        {
            int totalScore = 0;

            for (int frame = 0; frame < 19; frame++)
            {
                if (throws[frame] == 10)
                {
                    totalScore += 10;
                    totalScore += throws[frame + 1] + throws[frame + 2];
                }
                else if (throws[frame] + throws[frame + 1] == 10)
                {
                    totalScore += 10 + throws[frame + 1]; 
                }
                else
                {
                    totalScore += throws[frame] + throws[frame + 1];
                }

            }

            return totalScore;
        }

        public int GetScore()
        {
            return CountScore();
        }
    }

    
}
