using System;
using System.Diagnostics.SymbolStore;
using System.Drawing;
namespace Tumak
{
    class Program
    {
        static void PrintMatrix(int[,] arr) // Упр 6.2
        {
            int rows = arr.GetUpperBound(0) + 1;
            int columns = arr.GetUpperBound(1) + 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write($"{arr[i, j]} \t");
                }
                Console.WriteLine();
            }
        }
        static int [,] MultiplyMatrix(int[,] arr, int[,] arr2) // Упр 6.2
        {
            int rowsArr = arr.GetUpperBound(0) + 1;
            int columnsArr = arr.GetUpperBound(1) + 1;
            int rowsArr2 = arr2.GetUpperBound(0) + 1;
            int columnsArr2 = arr2.GetUpperBound(1) + 1;
            int[,] arr3 = new int[rowsArr, columnsArr2];
            if ((columnsArr) == (rowsArr2))
            {
                for (int i = 0; i < rowsArr; i++)
                {
                    for (int j = 0; j < columnsArr2; j++)
                    {
                        arr3[i, j] = 0;
                        for (int k = 0; k < columnsArr; k++)
                        {
                            arr3[i, j] += arr[i, k] * arr2[k, j];
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Число строк первой матрицы должно быть равно числу столбцов второй матрицы");
            }
            return arr3;
        }
        static float[] AverageTemp(int[,] arr) // Упр 6.3
        {
            float[] monthTemp = new float[12];
            for (int m = 0; m < 12; m++)
            {
                float daySum = 0;
                for (int d = 0; d < 30; d++)
                {
                    daySum += arr[m, d];
                }
                monthTemp[m] = daySum / 30;
            }
            return monthTemp;
        }
        static void PrintMatrix(LinkedList<LinkedList<int>> matrix) // Дз 6.2
        {
            foreach (var row in matrix)
            {
                foreach (var value in row)
                {
                    Console.Write(value + "\t");
                }
                Console.WriteLine();
            }
        }
        static LinkedList<LinkedList<int>> MultiplyMatrices(LinkedList<LinkedList<int>> matrixA, LinkedList<LinkedList<int>> matrixB) // Дз 6.2
        {
            int rowsA = matrixA.Count;
            int colsA = matrixA.First.Value.Count;
            int rowsB = matrixB.Count();
            int colsB = matrixB.First.Value.Count;

            if (colsA != rowsB)
            {
                throw new InvalidOperationException("Число строк первой матрицы должно быть равно числу столбцов второй матрицы");
            }
            LinkedList<LinkedList<int>> resultMatrix = new LinkedList<LinkedList<int>>();
            for (int i = 0; i < rowsA; i++)
            {
                LinkedList<int> resultRow = new LinkedList<int>();
                for (int j = 0; j < colsB; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        int valueA = GetElement(matrixA, i, k);
                        int valueB = GetElement(matrixB, k, j);
                        sum += valueA * valueB;
                    }
                    resultRow.AddLast(sum);
                }
                resultMatrix.AddLast(resultRow);
            }

            return resultMatrix;
        }
        static int GetElement(LinkedList<LinkedList<int>> matrix, int row, int col) // Дз 6.2
        {
            var rowNode = matrix.First;
            for (int i = 0; i < row; i++)
            {
                rowNode = rowNode.Next;
            }

            var colNode = rowNode.Value.First;
            for (int i = 0; i < col; i++)
            {
                colNode = colNode.Next;
            }

            return colNode.Value;
        }
        static void Upr_6_1(string[] path)
        {
            Console.WriteLine(@"Упражнение 6.1 Написать программу, которая вычисляет число гласных и согласных букв в
            файле. Имя файла передавать как аргумент в функцию Main. Содержимое текстового файла
            заносится в массив символов. Количество гласных и согласных букв определяется проходом
            по массиву. Предусмотреть метод, входным параметром которого является массив символов.
            Метод вычисляет количество гласных и согласных букв.");
            string fl = File.ReadAllText($"StuffTumak//{path[0]}");
            char[] arr = fl.ToCharArray();
            int vowelsCount = 0;
            int consonants_count = 0;
            string vowels = "aeyuioAEYUIO";
            foreach (char c in arr)
            {
                if (vowels.Contains(c))
                { 
                    vowelsCount ++;
                }
                else if (char.IsLetter(c))
                {
                    consonants_count ++;
                }
            }
            Console.WriteLine($"\nКоличество гласных букв в файле: {vowelsCount}\nКоличество согласных букв в файле: {consonants_count}");
        }
        static void Upr_6_2()
        {
            Console.WriteLine(@"Упражнение 6.2 Написать программу, реализующую умножению двух матриц, заданных в
            виде двумерного массива. В программе предусмотреть два метода: метод печати матрицы,
            метод умножения матриц (на вход две матрицы, возвращаемое значение – матрица).");
            int[,] a = { { 23,12,56 }, { 45,7,15},{17,34,26} };
            Console.WriteLine($"\nМатрица a:");
            PrintMatrix( a );
            int[,] b = { { 89,25,42}, { 28,57,31 }, {12,8,76 } };
            Console.WriteLine($"\nМатрица b:");
            PrintMatrix(b);
            Console.WriteLine($"\nМатрица a*b:");
            PrintMatrix(MultiplyMatrix(a,b));
        }
        static void Upr_6_3()
        {
            Console.WriteLine(@"Упражнение 6.3 Написать программу, вычисляющую среднюю температуру за год. Создать
            двумерный рандомный массив temperature[12,30], в котором будет храниться температура
            для каждого дня месяца (предполагается, что в каждом месяце 30 дней). Сгенерировать
            значения температур случайным образом. Для каждого месяца распечатать среднюю
            температуру. Для этого написать метод, который по массиву temperature [12,30] для каждого
            месяца вычисляет среднюю температуру в нем, и в качестве результата возвращает массив
            средних температур. Полученный массив средних температур отсортировать по
            возрастанию.");
            int[,] temperature = new int[12,30];
            Random r = new Random();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    temperature[i,j] = r.Next(-30,31);
                }
            }
            float [] monthsTemp = AverageTemp(temperature);
            for (int m = 0; m < 12; m++)
            {
                switch (m)
                {
                    case 0: Console.WriteLine($"\nСредняя температура в Январе: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 1: Console.WriteLine($"Средняя температура в Феврале: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 2: Console.WriteLine($"Средняя температура в Марте: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 3: Console.WriteLine($"Средняя температура в Апреле: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 4: Console.WriteLine($"Средняя температура в Мае: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 5: Console.WriteLine($"Средняя температура в Июне: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 6: Console.WriteLine($"Средняя температура в Июле: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 7: Console.WriteLine($"Средняя температура в Августе: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 8: Console.WriteLine($"Средняя температура в Сентябре: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 9: Console.WriteLine($"Средняя температура в Октябре: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 10: Console.WriteLine($"Средняя температура в Ноябре: {Math.Round(monthsTemp[m], 1)}"); break;
                    case 11: Console.WriteLine($"Средняя температура в Декабре: {Math.Round(monthsTemp[m], 1)}\n"); break;
                }
            }
            Array.Sort(monthsTemp);
            Console.WriteLine("Отсортированное по возрастанию:\n");
            for (int i = 0;i < 12; i++)
            {
                Console.WriteLine(Math.Round(monthsTemp[i],1));
            }
            
        }
        static void Dz_6_1(string [] path)
        {
            Console.WriteLine(@"Домашнее задание 6.1. Упражнение 6.1 выполнить с помощью коллекции List<T>.");
            string fl = File.ReadAllText($"StuffTumak//{path[0]}");
            List<char> lst = fl.ToList();
            int vowelsCount = 0;
            int consonants_count = 0;
            string vowels = "aeyuioAEYUIO";
            foreach (char c in lst)
            {
                if (vowels.Contains(c))
                {
                    vowelsCount++;
                }
                else if (char.IsLetter(c))
                {
                    consonants_count++;
                }
            }
            Console.WriteLine($"\nКоличество гласных букв в файле: {vowelsCount}\nКоличество согласных букв в файле: {consonants_count}");
        }
        static void Dz_6_2()
        {
            Console.WriteLine(@"Домашнее задание 6.2. Упражнение 6.2 выполнить с помощью коллекций LinkedList<LinkedList<T>>.");
            var matrixA = new LinkedList<LinkedList<int>>();
            matrixA.AddLast(new LinkedList<int>(new[] { 13, 23, 43 }));
            matrixA.AddLast(new LinkedList<int>(new[] { 42, 14, 65 }));
            matrixA.AddLast(new LinkedList<int>(new[] { 87, 45, 34 }));
            var matrixB = new LinkedList<LinkedList<int>>();
            matrixB.AddLast(new LinkedList<int>(new[] { 12, 32, 13 }));
            matrixB.AddLast(new LinkedList<int>(new[] { 62, 53, 44 }));
            matrixB.AddLast(new LinkedList<int>(new[] { 3, 22, 1 }));
            Console.WriteLine("\nМатрица A:");
            PrintMatrix(matrixA);
            Console.WriteLine("\nМатрица B:");
            PrintMatrix(matrixB);
            var result = MultiplyMatrices(matrixA, matrixB);

            Console.WriteLine("\nРезультат умножения матриц:");
            PrintMatrix(result);
        }
        static void Dz_6_3()
        {
            Console.WriteLine(@"Домашнее задание 6.3. Написать программу для упражнения 6.3, использовав класс
            Dictionary<TKey, TValue>. В качестве ключей выбрать строки – названия месяцев, а в
            качестве значений – массив значений температур по дням.");
            string[] months = new string[] {"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь","Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"};
            Dictionary<string, int[]> temperature = new Dictionary<string, int[]>();
            Random r = new Random();
            foreach (var m in months)
            {
                int[] temperatureDays = new int[30];
                for (int d = 0; d < 30; d++)
                {
                    temperatureDays[d] = r.Next(-30, 31);
                }
                temperature[m] = temperatureDays;
            }
            Dictionary<string, double> monthTemp = new Dictionary<string, double>();
            foreach (var kvp in temperature)
            {
                string month = kvp.Key;
                int[] dailyTemperatures = kvp.Value;

                double average = dailyTemperatures.Average();
                monthTemp[month] = average;

            }
            var sortedAverages = monthTemp.OrderBy(kvp => kvp.Value);
            foreach (var kvp in sortedAverages)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value:F2}");
            }
        }
        static void Main(string[] args)
        {

            Upr_6_1(args);
            Upr_6_2();
            Upr_6_3();
            Dz_6_1(args);
            Dz_6_2();
            Dz_6_3();
        }
    }
}