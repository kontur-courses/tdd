Input should be in format: 
[pathToWords] [pathToPicture] [drawSettings = RectanglesWithNumeration] 
Paths starting from exe directory.

For example:
Exe in C:.../Test/TagCloud.Utility.exe
Input: words.txt result
In result:
Words should be in ../Test/words.txt
Picture will be saved in ../Test/result.png

Draw Settings:
To change draw setting write in input number of chosed option
Options:
Only Words == 0
Words In Rectangles == 1
Only Rectangles == 2
Rectangles With Numeration == 3

All words will be in 3 tags groups : 
Big: with size 80px per letter in width, 150px in height
Average: with size 60px per letter in width, 100px in height
Small: with size 30px per letter in width, 50px in height

Descending rectangles:
![Descending](https://github.com/Rozentor/tdd/blob/master/cs/TagCloudUtility/descendingRectanglesTest.png?raw=true")

Random rectangles:
![Descending](https://github.com/Rozentor/tdd/blob/master/cs/TagCloudUtility/randomCloudTest.png?raw=true")

Similar rectangles
![Descending](https://github.com/Rozentor/tdd/blob/master/cs/TagCloudUtility/similarCloudTest.png?raw=true")

Kolobok test:
![Descending](https://github.com/Rozentor/tdd/blob/master/cs/TagCloudUtility/result.png?raw=true")
