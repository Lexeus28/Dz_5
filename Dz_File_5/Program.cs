using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
namespace Dz
{
    class Graph
    {
        private Dictionary<int, List<int>> adjacencyList;
        public Graph()
        {
            adjacencyList = new Dictionary<int, List<int>>();
        }
        // Добавление ребра в граф
        public void AddEdge(int source, int destination)
        {
            if (!adjacencyList.ContainsKey(source))
            {
                adjacencyList[source] = new List<int>();
            }
            if (!adjacencyList.ContainsKey(destination))
            {
                adjacencyList[destination] = new List<int>();
            }

            adjacencyList[source].Add(destination);
            adjacencyList[destination].Add(source); // Для неориентированного графа
        }
        // Поиск кратчайшего пути с использованием BFS
        public List<int> FindShortestPath(int start, int end)
        {
            var visited = new HashSet<int>();
            var queue = new Queue<List<int>>();
            // Начинаем с пути, содержащего только начальную вершину
            queue.Enqueue(new List<int> { start });
            visited.Add(start);
            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                int currentNode = path[^1]; // Последний узел в пути
                // Если достигли конечной вершины, возвращаем путь
                if (currentNode == end)
                {
                    return path;
                }
                // Проходим по всем соседям текущего узла
                foreach (var neighbor in adjacencyList[currentNode])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        var newPath = new List<int>(path) { neighbor };
                        queue.Enqueue(newPath);
                    }
                }
            }
            // Если путь не найден, возвращаем пустой список
            return new List<int>();
        }
    }
    class Program
    {
        static void ValidateString(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    throw new ArgumentException("Строка не должна содержать цифры.");
                }
            }
        }
        static void AddStudent(Dictionary<int, Student> group)
        {
            try
            {
                Console.Write("Введите фамилию: ");
                string lastName = (Console.ReadLine());
                ValidateString(lastName);

                Console.Write("Введите имя: ");
                string firstName = Console.ReadLine();
                ValidateString(firstName);

                Console.Write("Введите год рождения: ");
                int birthYear = int.Parse(Console.ReadLine());

                Console.Write("Введите экзамен: ");
                string exam = Console.ReadLine();
                ValidateString(exam);

                Console.Write("Введите баллы: ");
                int score = int.Parse(Console.ReadLine());
                var lastEntry = group.Last();
                group.Add(lastEntry.Key + 1, new Student
                {
                    surname = lastName,
                    name = firstName,
                    yearOfBirth = birthYear,
                    exam = exam,
                    exPoints = score
                });
                Console.WriteLine("\nСтудент успешно добавлен.");
                DisplayStudents(group);
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат данных. Здесь нужно вводить число");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Неверный формат данных. Здесь нужно вводить буквы");
            }
        }
        static void RemoveStudent(Dictionary<int, Student> group)
        {
            Console.Write("Введите фамилию студента для удаления: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите имя студента для удаления: ");
            string firstName = Console.ReadLine();
            int keyToRemove = -1;
            foreach (var entry in group)
            {
                Student student = entry.Value;
                if (student.name == firstName && student.surname == lastName)
                {
                    keyToRemove = entry.Key;
                    break;
                }
            }
            if (keyToRemove != -1)
            {
                group.Remove(keyToRemove);
                Console.WriteLine("\nСтудент успешно удален.");
            }
            else
            {
                Console.WriteLine("\nСтудент с указанными данными не найден.");
            }
            DisplayStudents(group);
        }
        static void DisplayStudents(Dictionary<int, Student> group)
        {
            foreach (var entry in group)
            {
                Student student = entry.Value;
                Console.WriteLine(student.ToString());
            }
        }
        static void Sort(Dictionary<int, Student> group)
        {
            Dictionary<int, Student> sorted = group.OrderBy(entry => entry.Value.exPoints).ToDictionary(entry => entry.Key, entry => entry.Value);
            DisplayStudents(sorted);
        }
        static void AddGranny(Queue<Babka> grannies)
        {
            try
            {
                Console.Write("\nВведите имя бабули: ");
                string name = Console.ReadLine();
                ValidateString(name);
                Console.Write("\nВведите возраст бабули: ");
                byte age = byte.Parse(Console.ReadLine());
                Console.Write("\nВведите болезни бабули (через запятую, если есть): ");
                List<string> diseases = Console.ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries).Select(d => d.Trim()).ToList();
                Console.Write("\nВведите лекарства бабули: ");
                List<string> medicines = Console.ReadLine()?.Split(",").Select(m => m.Trim()).Where(m => !string.IsNullOrEmpty(m)).ToList() ?? new List<string>();
                grannies.Enqueue(new Babka { name = name, age = age, deseases = diseases, medication = medicines });
                Console.WriteLine("Бабуля добавлена.");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nВозраст должен быть числом");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("\nИмя должно состоять из букв");
            }
        }
        static void DistributeGrannies(Queue<Babka> grannies, Stack<Hospital> hospitals)
        {
            if (!grannies.Any())
            {
                Console.WriteLine("Нет бабуль в очереди.");
                return;
            }

            if (!hospitals.Any())
            {
                Console.WriteLine("Нет больниц для распределения.");
                return;
            }

            Babka granny = grannies.Dequeue();
            foreach (Hospital hospital in hospitals)
            {
                if (hospital.Patients.Count >= hospital.capacity)
                    continue;

                int treatableDiseasesCount = granny.deseases.Count(d => hospital.hospDesease.Contains(d));
                if (granny.deseases.Count == 0 || (double)treatableDiseasesCount / granny.deseases.Count >= 0.5)
                {
                    hospital.Patients.Add(granny);
                    Console.WriteLine($"{granny.name} направлена в {hospital.name}.");
                    return;
                }
            }
        }
        static void ShowHospitals(Stack<Hospital> hospitals)
        {
            if (!hospitals.Any())
            {
                Console.WriteLine("Нет больниц для отображения.");
                return;
            }

            foreach (Hospital hospital in hospitals)
            {
                Console.WriteLine(hospital);
            }
        }
        static void ShowGrannies(Queue<Babka> grannies)
        {
            if (!grannies.Any())
            {
                Console.WriteLine("Очередь бабуль пуста.");
                return;
            }

            foreach (Babka granny in grannies)
            {
                Console.WriteLine(granny);
            }
        }

        static void Task1()
        {
            Console.WriteLine(@"Задание 1. Создать List на 64 элемента, скачать из интернета 32 пары картинок (любых). В List
            должно содержаться по 2 одинаковых картинки. Необходимо перемешать List с
            картинками. Вывести в консоль перемешанные номера (изначальный List и полученный
            List). Перемешать любым способом.");
            List<Image> images = new List<Image>();
            for (int i = 1; i < 33; i++)
            {
                images.Add(Image.FromFile($"Stuff//photo_{i}.jpg"));
                images.Add(Image.FromFile($"Stuff//photo_{i}.jpg"));
            }
            List<int> originalIndices = Enumerable.Range(0, images.Count).ToList();
            Random rand = new Random();
            List<Image> shuffledList = images.OrderBy(x => rand.Next()).ToList();
            Console.WriteLine("\nИсходный список индексов:");
            Console.WriteLine(string.Join(" ", originalIndices));

            Console.WriteLine("\nПеремешанный список индексов:");
            for (int i = 0; i < shuffledList.Count; i++)
            {
                int shuffledIndex = images.IndexOf(shuffledList[i]);
                Console.Write($"{shuffledIndex} ");
            }
            Console.WriteLine();
        }
        static void Task2()
        {
            Console.WriteLine(@"Задание 2. Создать студента из вашей группы (фамилия, имя, год рождения, с каким экзаменом
            поступил, баллы). Создать словарь для студентов из вашей группы (10 человек).
            Необходимо прочитать информацию о студентах с файла. В консоли необходимо создать
            меню:
            a. Если пользователь вводит: Новый студент, то необходимо ввести
            информацию о новом студенте и добавить его в List
            b. Если пользователь вводит: Удалить, то по фамилии и имени удаляется
            студент
            c. Если пользователь вводит: Сортировать, то происходит сортировка по баллам
            (по возрастанию)");
            Dictionary<int, Student> group = new Dictionary<int, Student>();
            string[] lines = File.ReadAllLines("Stuff//Groupmates.txt");
            for (int i = 0; i < File.ReadAllLines("Stuff//Groupmates.txt").Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length == 5)
                {
                    group.Add(i, new Student
                    {
                        surname = parts[0].Trim(),
                        name = parts[1].Trim(),
                        yearOfBirth = int.Parse(parts[2].Trim()),
                        exam = parts[3].Trim(),
                        exPoints = int.Parse(parts[4].Trim())
                    });
                }
            }
            Console.WriteLine("\nСписок студентов:");
            DisplayStudents(group);
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("\nДля того, чтобы добавить студента, введите 'Новый студент' (без кавычек)");
                Console.WriteLine("Для того, чтобы удалить студента, введите 'Удалить' (без кавычек)");
                Console.WriteLine("Для того, чтобы отсортировать список по баллам, введите 'Сортировать' (без кавычек)");
                Console.WriteLine("Для того, чтобы перейти к следующему заданию, введите 'Продолжить' (без кавычек)");
                switch (Console.ReadLine().ToLower())
                {
                    case "новый студент": AddStudent(group); break;
                    case "удалить": RemoveStudent(group); break;
                    case "сортировать": Sort(group); break;
                    case "продолжить": flag = false; break;
                }
            }
            Console.WriteLine();
        }
        static void Task3()
        {
            Console.WriteLine(@"Задание 3. Создать бабулю. У бабули есть Имя, возраст, болезнь и лекарство от этой болезни,
            которое она принимает (болезней может быть у бабули несколько). Реализуйте список
            бабуль. Также есть больница (у больницы есть название, список болезней, которые они
            лечат и вместимость). Больниц также, как и бабуль несколько. Бабули по очереди
            поступают (реализовать ввод с клавиатуры) и дальше идут в больницу, в зависимости от
            заполненности больницы и списка болезней, которые лечатся в данной больнице,
            реализовать функционал, который будет распределять бабулю в нужную больницу. Если
            бабуля не имеет болезней, то она хочет только спросить - она может попасть в первую
            свободную клинику. Если бабулю ни одна из больниц не лечит, то бабуля остаётся на
            улице плакать. На экран выводить список всех бабуль, список всех больниц, болезни,
            которые там лечат и сколько бабуль в данный момент находится в больнице, также
            вывести процент заполненности больницы. P.S. Бабуля попадает в больницу, если там
            лечат более 50% ее болезней. Больницы реализовать в виде стека, бабуль в виде
            очереди.");
            Queue<Babka> grannies = new Queue<Babka>();
            Stack<Hospital> hospitals = new Stack<Hospital>();
            hospitals.Push(new Hospital
            {
                name = "Центральная больница",
                hospDesease = new List<string> { "Грипп", "Диабет", "Гипертония" },
                capacity = 3,
                Patients = new List<Babka>()
            });

            hospitals.Push(new Hospital
            {
                name = "Городская клиника",
                hospDesease = new List<string> { "Простуда", "Артрит", "Мигрень" },
                capacity = 2,
                Patients = new List<Babka>()
            });
            grannies.Enqueue(new Babka
            {
                name = "Мария Ивановна",
                age = 80,
                deseases = new List<string> { "Грипп" },
                medication = new List<string> { "Терафлю" }
            });

            grannies.Enqueue(new Babka
            {
                name = "Анна Петровна",
                age = 76,
                deseases = new List<string> { "Диабет" },
                medication = new List<string> { "Инсулин" }
            });

            grannies.Enqueue(new Babka
            {
                name = "Екатерина Сергеевна",
                age = 85,
                deseases = new List<string> { "Гипертония", "Грипп" },
                medication = new List<string> { "Каптоприл" }
            });

            grannies.Enqueue(new Babka
            {
                name = "Татьяна Алексеевна",
                age = 79,
                deseases = new List<string> { "Простуда" },
                medication = new List<string> { "Парацетамол" }
            });

            grannies.Enqueue(new Babka
            {
                name = "Ольга Николаевна",
                age = 82,
                deseases = new List<string> { "Артрит" },
                medication = new List<string> { "Диклофенак" }
            });

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("\n1. Добавить бабулю");
                Console.WriteLine("2. Распределить бабуль по больницам");
                Console.WriteLine("3. Показать все больницы");
                Console.WriteLine("4. Показать всех бабуль");
                Console.WriteLine("5. Перейти к следующему заданию");
                Console.Write("Введите номер действия: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddGranny(grannies);
                        break;

                    case "2":
                        DistributeGrannies(grannies, hospitals);
                        break;

                    case "3":
                        ShowHospitals(hospitals);
                        break;

                    case "4":
                        ShowGrannies(grannies);
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }
        static void Task4()
        {
            Console.WriteLine(@"Задание 4. Написать метод для обхода графа в глубину или ширину - вывести на экран кратчайший путь.");

            var graph = new Graph();
            graph.AddEdge(1, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(2, 4);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            int start = 1;
            int end = 5;
            var path = graph.FindShortestPath(start, end);
            if (path.Count > 0)
            {
                Console.WriteLine("Кратчайший путь: " + string.Join(" -> ", path));
            }
            else
            {
                Console.WriteLine("Путь не найден.");
            }
        }
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Task4();
        }
    }
    struct Student
    {
        public string surname;
        public string name;
        public int yearOfBirth;
        public string exam;
        public int exPoints;
        public override string ToString()
        {
            return $"Студент: {surname} {name}, Год рождения: {yearOfBirth}, Экзамен: {exam}, Баллы: {exPoints}";
        }
    }
    struct Babka
    {
        public string name;
        public byte age;
        public List<string> deseases;
        public List<string> medication;
        public override string ToString()
        {
            return $"{name}, {age} лет, Болезни: {string.Join(", ", deseases)}, Лекарства: {string.Join(", ", medication)}";
        }

    }
    struct Hospital
    {
        public string name;
        public List<string> hospDesease;
        public int capacity;
        public List<Babka> Patients;
        public double FillPercentage => (double)Patients.Count / capacity * 100;
        public override string ToString()
        {
            return $"Больница: {name}, Вместимость: {capacity}, Лечимые болезни: {string.Join(", ", hospDesease)}, Заполненность: {Patients.Count}/{capacity} ({FillPercentage:F1}%)";
        }
    }
}