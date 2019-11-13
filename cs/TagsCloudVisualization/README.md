Изображения находятся по пути:
	TagsCloudVisualization\Images

Сигнатура метода генератора:
	GetRandomSizesList(int minWidth, int maxWidth, int minHeight, int maxHeight, int numberOfSizes, Random random)

Eсть 3 изображения:
	1.BigWidthSmallHeight.png - прямоугольники с большой шириной и маленькой высотой
		Параметры метода генератора: Generator.GetRandomSizesList(50, 100, 10, 20, 50, new Random());
	2.RandomWidthAndHeight.png - прямоугольники со случайными размерами
		Параметры метода генератора: Generator.GetRandomSizesList(10, 50, 10, 50, 200, new Random());
	3.Squares.png - все прямоугольники - квадраты
		Параметры метода генератора: Generator.GetRandomSizesList(20, 20, 20, 20, 100, new Random());