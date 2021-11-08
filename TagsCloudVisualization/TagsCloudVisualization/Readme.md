## Реализация раскладки слов для TagCloud


Раскладка на 1000 тэгов

![tag_cloud_00](https://user-images.githubusercontent.com/82332119/140610227-484a6c85-0354-423a-9e36-2a80ab5e0972.png)
#### Тест заполняемости показывает средний результат в 80%

![06_Passed](https://user-images.githubusercontent.com/82332119/140610273-30a02ebb-5297-4413-b305-8d75c5342493.png)

#### 2 проблемы в процессе решения:

Раскладка одинаковых квадратов в количестве n^2 стремится к кругу. Хотя с точки зрения плотности квадрат оптимальнее.

![07_Failed](https://user-images.githubusercontent.com/82332119/140610310-e1cce987-11e2-493d-9a69-69b8a075ce86.png)
![01_Failed](https://user-images.githubusercontent.com/82332119/140612683-f7033224-01a0-4cdf-ba08-364be1c9c8ea.png)


[Проблема решена] Из-за оптимизации перебора плотность может быть нарушена в случае большого различия в размерах некоторых прямоугольников
![08_Passed](https://user-images.githubusercontent.com/82332119/140610349-b950cd77-86ed-4e9b-8d5f-e4066f39333a.png)


