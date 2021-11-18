using System;

namespace TagsCloudVisualization
{
    public class CloudLayoutParameters
    {
        private const float Pi2 = MathF.PI * 2;
        private const float MinAngle = MathF.PI / 180 * 5;
        private const int Step = 10;
        private const int MaxRadius = 10000;

        public CloudLayoutParameters(int countBoostsForChangeAngle = 5, float startAngle = MathF.PI / 2)
        {
            Radius = Step;
            this.startAngle = startAngle;
            this.countBoostsForChangeAngle = countBoostsForChangeAngle;
        }

        private readonly float startAngle;
        private readonly int countBoostsForChangeAngle;
        private int stepCount;

        public int Radius { get; private set; }
        public float StepAngle { get; private set; }
        public float NextAngle => StepAngle * NextStep;
        public float Angle => stepCount * StepAngle;

        private int NextStep => ++stepCount;


        private int countRadiusBoosts;
        private int CountRadiusBoosts
        {
            get => ++countRadiusBoosts;
            set => countRadiusBoosts = value;
        }

        public void BoostRadius()
        {
            Radius += Step;
            stepCount = 0;

            if (CountRadiusBoosts % countBoostsForChangeAngle == 0 && NextAngle > MinAngle)
            {
                StepAngle /= 2;
            }

            if (Radius > MaxRadius)
            {
                throw new Exception("The radius exceeds the maximum allowed value\n" +
                                    $"MaxRadius = {MaxRadius} CurrentRadius = {Radius}");
            }
        }

        public void ResetRadius()
        {
            StepAngle = startAngle;
            stepCount = 0;
            CountRadiusBoosts = 0;
            Radius = Step;
        }

        public bool IsValidNextAngle() => NextAngle < Pi2;
    }
}