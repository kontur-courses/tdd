namespace TagsCloudVisualization.Tests;

public static class TestCaseData
{
    public static object[][] IntersectionTestSource()
    {
        return
        [
            [
                500, 500,
                new[]
                {
                    (20, 10),
                    (40, 20),
                    (60, 30),
                    (80, 40)
                }
            ],
            [
                500, 500,
                new[]
                {
                    (10, 10),
                    (10, 10),
                    (20, 10),
                    (20, 10)
                }
            ],
            [
                600, 600,
                new[]
                {
                    (20, 10),
                    (30, 15),
                    (50, 10),
                    (60, 30),
                    (30, 10),
                    (40, 20)
                }
            ]
        ];
    }

    public static object[][] FirstRectanglePositionTestSource()
    {
        return
        [
            [
                500, 500,
                new[]
                {
                    (20, 30),
                    (30, 45),
                    (40, 60)
                }
            ],
            [
                510, 550,
                new[]
                {
                    (10, 40),
                    (10, 30),
                    (10, 20)
                }
            ],
            [
                300, 800,
                new[]
                {
                    (10, 30)
                }
            ]
        ];
    }

    public static object[][] DensityTestSource()
    {
        return
        [
            [
                500, 500, 3025,
                new[]
                {
                    (20, 20),
                    (30, 30),
                    (40, 40)
                }
            ],
            [
                500, 500, 3025,
                new[]
                {
                    (20, 20),
                    (40, 40),
                    (30, 30)
                }
            ],
            [
                600, 400, 4225,
                new[]
                {
                    (40, 40),
                    (30, 30),
                    (20, 20),
                    (10, 10)
                }
            ],
            [
                400, 550, 3025,
                new[]
                {
                    (20, 20),
                    (30, 30),
                    (40, 40),
                    (10, 10)
                }
            ]
        ];
    }

    public static object[][] CenterTestSource()
    {
        return
        [
            [
                500, 500,
                new[]
                {
                    (10, 20),
                    (10, 60),
                    (10, 60),
                    (10, 20)
                }
            ],
            [
                300, 500,
                new[]
                {
                    (10, 40),
                    (10, 50),
                    (20, 30),
                    (40, 30)
                }
            ],
            [
                520, 410,
                new[]
                {
                    (10, 20),
                    (20, 30),
                    (30, 40),
                    (40, 50),
                    (50, 40),
                    (40, 30),
                    (30, 20),
                    (20, 10)
                }
            ]
        ];
    }
}