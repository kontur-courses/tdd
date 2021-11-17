using System;

namespace TagsCloudVisualization
{
    public class CloudLayouterParameters
    {
        private const float Pi2 = MathF.PI * 2;

        public CloudLayouterParameters(
            float startAngle = MathF.PI / 2,
            float minAngle = MathF.PI / 180 * 5,
            int stepLength = 10,
            int countRadiusBoostsForChangeAngle = 5,
            int maxRadius = 10000
        )
        {
            StepAngle = StartAngle = startAngle;
            MaxRadius = maxRadius;
            CurrentRadius = StepLength = stepLength;
            CountRadiusBoostsForChangeAngle = countRadiusBoostsForChangeAngle;
            MinAngle = minAngle;
        }

        public int MaxRadius { get; }
        public int CountRadiusBoostsForChangeAngle { get; }
        public int StepLength { get; }
        public float StartAngle { get; }
        public float MinAngle { get; }

        public int CurrentRadius { get; private set; }
        public float NextAngle => StepAngle * NextStep;
        public float StepAngle { get; private set; }

        
        private int CurrentStep { get; set; }
        private int NextStep => ++CurrentStep;

        private int countRadiusBoosts;
        private int CountRadiusBoosts
        {
            get => ++countRadiusBoosts;
            set => countRadiusBoosts = value;
        }

        public void BoostRadius()
        {
            CurrentRadius += StepLength;
            CurrentStep = 0;

            if (CountRadiusBoosts % CountRadiusBoostsForChangeAngle == 0 && NextAngle > MinAngle)
                StepAngle /= 2;

            if (CurrentRadius > MaxRadius)
                throw new Exception("The radius exceeds the maximum allowed value\n" +
                                    $"MaxRadius = {MaxRadius} CurrentRadius = {CurrentRadius}");
        }

        public void ResetRadius()
        {
            StepAngle = StartAngle;
            CurrentStep = 0;
            CountRadiusBoosts = 0;
            CurrentRadius = StepLength;
        }

        public bool IsNextAngleLessThanPi2() => NextAngle < Pi2;
    }
}