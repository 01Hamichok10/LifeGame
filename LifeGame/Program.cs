namespace LifeGame
{
    internal class Program
    {
        //Метод для изменение размеров поля
        static char[,] EditField()
        {
            int x = 0, y = 0;
            Console.Clear();
            Console.WriteLine("Задайте кол-во клеток по вертикали:");
            bool isCorrect = false;
            while (!isCorrect)
            {
                try
                {
                    y = Convert.ToInt32(Console.ReadLine());
                    if (y < 1) throw new Exception("Введите число больше нуля");
                    isCorrect = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("Задайте кол-во клеток по горизонтали:");
            isCorrect = false;
            while (!isCorrect)
            {
                try
                {
                    x = Convert.ToInt32(Console.ReadLine());
                    if (x < 1) throw new Exception("Введите число больше нуля");
                    isCorrect = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine($"Создано поле размерами {y} на {x} клеток");

            char[,] result = new char[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    result[i, j] = ' ';
                }
            }
            return result;
        }

        //Метод для вывода поля на экран
        static void PrintField(char[,] Field)
        {
            int x = Field.GetLength(0);
            int y = Field.GetLength(1);
            for (int i = 0; i < x + 2; i++)
            {
                Console.Write("#");
            }
            Console.WriteLine();
            for (int i = 0; i < y; i++)
            {
                Console.Write("#");
                for (int j = 0; j < x; j++)
                {
                    Console.Write($"{Field[j, i]}");
                }
                Console.Write("#\n");
            }
            for (int i = 0; i < x + 2; i++)
            {
                Console.Write("#");
            }
            Console.WriteLine();
        }

        // Метод для изменения точек на поле
        static void EditDot(ref char[,] Field)
        {
            Console.Clear();
            int x = 0, y = 0;
            int XF = Field.GetLength(0);
            int YF = Field.GetLength(1);
            bool isCorrect = false;
            PrintField(Field);
            Console.WriteLine("Введите координату по вертикали точки, которую хотите изменить:");
            while (!isCorrect)
            {
                try
                {
                    y = Convert.ToInt32(Console.ReadLine());
                    if (y > YF || y <= 0) throw new Exception("Вы выходите за границы массива!");
                    isCorrect = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("Введите координату по горизонтали точки, которую хотите изменить:");
            isCorrect = false;
            while (!isCorrect)
            {
                try
                {
                    x = Convert.ToInt32(Console.ReadLine());
                    if (x > XF || x <= 0) throw new Exception("Вы выходите за границы массива!");
                    isCorrect = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (Field[x - 1, y - 1] == ' ') { Field[x - 1, y - 1] = '@'; }
            else if (Field[x - 1, y - 1] == '@') { Field[x - 1, y - 1] = ' '; }
        }

        //Метод для подсчёта кол-ва живых соседей у клетки
        static int Neighbors(char[,] Field, int x, int y)
        {
            int count = 0;
            int rows = Field.GetLength(0);
            int cols = Field.GetLength(1);

            //Координаты отхождения от клетки, для удобного отслеживания координат
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols)
                {
                    if (Field[nx, ny] == '@')
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        //Глубокое копирование массива
        static char[,] ArrayDeepCopy(char[,] original)
        {
            int rows = original.GetLength(0);
            int cols = original.GetLength(1);

            char[,] copy = new char[rows, cols];

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    copy[x, y] = original[x, y];
                }
            }

            return copy;
        }

        //Метод для воспроизводства игры
        static void DoGame(ref char[,] Field)
        {
            Console.Clear();
            PrintField(Field);
            int gens = 0;
            Console.WriteLine("Сколько поколений должно пройти?");
            try
            {
                gens = Convert.ToInt32(Console.ReadLine());
                if (gens < 0) throw new Exception("Число должно быть больше 0");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            for (int i = 0; i < gens; i++)
            {
                Console.Clear();
                PrintField(Field);
                Thread.Sleep(1000);
                char[,] newField = new char[Field.GetLength(0), Field.GetLength(1)];
                newField = ArrayDeepCopy(Field);
                for (int x = 0; x < Field.GetLength(0); x++)
                {
                    for (int y = 0; y < Field.GetLength(1); y++)
                    {
                        if (Field[x, y] == ' ' && Neighbors(Field, x, y) == 3) { newField[x, y] = '@'; }
                        else if (Field[x, y] == '@' && (Neighbors(Field, x, y) > 3) || (Neighbors(Field, x, y) < 2)) { newField[x, y] = ' '; }
                    }
                }
                Field = ArrayDeepCopy(newField);
            }
        }

        static void RandomPoints(ref char[,] Field)
        {
            Random rand = new Random();
            int live = 0;
            for (int x = 0; x < Field.GetLength(0); x++)
            {
                for (int y = 0; y < Field.GetLength(1); y++)
                {
                    live = rand.Next(0,2);
                    if (live == 0) Field[x, y] = ' '; else Field[x, y] = '@';
                }
            }
        }

        static void Main(string[] args)
        {
            char[,] Field = new char[0, 0]; // Создание поля для игры
            Exception exeption = null;     // Для вывода сообщений в меню о некорректных вводах
            int act = 0;                   // Для выбора действия
            bool isCorrect = false;        // Для проверки правильности действия
            bool BoundsDo = false;         // Для проверки заданности размера полю
            while (true)                   // Цикл программы бесконечен, для выключения используется 4 конструкция switch
            {
                act = 0;
                isCorrect = false;
                while (!isCorrect)
                {
                    Console.Clear();
                    Console.WriteLine("Меню игры <Жизнь>\nВыберите действие:");
                    Console.WriteLine("1.Задать размеры поля\n2.Отрегулировать точки\n3.Запустить симуляцию\n4.Случайно сгенерировать клетки\n5.Закрыть программу");
                    if (Field.GetLength(0) > 0 && Field.GetLength(1) > 0)
                    {
                        PrintField(Field);
                    }
                    if (exeption != null)
                    {
                        Console.WriteLine(exeption.Message);
                    }
                    try
                    {
                        act = Convert.ToInt32(Console.ReadLine());
                        if (act < 0 && act > 3)
                        {
                            throw new Exception($"Выберите действие от 1 до 3!");
                        }
                        exeption = null;
                        isCorrect = true;
                    }
                    catch (Exception ex)
                    {
                        exeption = ex;
                    }
                }
                // Выбор действия
                switch (act)
                {
                    case 1:
                        {
                            Field = EditField();
                            BoundsDo = true;
                            break;
                        }
                    case 2:
                        {
                            if (BoundsDo) { EditDot(ref Field); }
                            else { exeption = new Exception("Сначала задайте размеры массива"); }
                            break;
                        }
                    case 3:
                        {
                            if (BoundsDo) { DoGame(ref Field); }
                            else { exeption = new Exception("Сначала задайте размеры массива"); }
                            break;
                        }
                    case 4:
                        {
                            if (BoundsDo) { RandomPoints(ref Field); }
                            else { exeption = new Exception("Сначала задайте размеры массива"); }
                            break;
                        }
                    case 5:
                        {
                            Environment.Exit(0);
                            break;
                        }
                }
            }
        }
    }
}


