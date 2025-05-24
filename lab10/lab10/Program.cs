namespace MyNamespace;


class Program
    {
        static void Main()
        {
            

        string[] months =
         {
          "January", "February", "March", "April", "May",
          "June", "July", "August", "September", "October",
          "November", "December"
          };

        // 1. Месяцы с длиной строки равной n
        Console.Write("Введите длину строки n для поиска месяцев: ");
        int n = int.Parse(Console.ReadLine());

        var monthsWithLengthN = from month in months
                                where month.Length == n
                                orderby month
                                select month;

        Console.WriteLine("Месяцы с длиной строки " + n + ":");
        foreach (string month in monthsWithLengthN)
        {
            Console.WriteLine(month);
        }
        Console.WriteLine("...........................................................................................");

        // 2. Летние и зимние месяцы
        var summerAndWinterMonths = from month in months
                                    where month == "June" || month == "July" || month == "August" ||
                                          month == "December" || month == "January" || month == "February"
                                    orderby month
                                    select month;

        Console.WriteLine("Летние и зимние месяцы:");
        foreach (string month in summerAndWinterMonths)
        {
            Console.WriteLine(month);
        }
        Console.WriteLine("...........................................................................................");

        // 3. Месяцы в алфавитном порядке
        var monthsInAlphabeticalOrder = from month in months
                                        orderby month
                                        select month;

        Console.WriteLine("Месяцы в алфавитном порядке:");
        foreach (string month in monthsInAlphabeticalOrder)
        {
            Console.WriteLine(month);
        }
        Console.WriteLine("...........................................................................................");

        // 4. Месяцы, содержащие 'u' и длиной не менее 4-х
        var monthsWithUAndLengthAtLeast4 = from month in months
                                           where month.Contains('u') && month.Length >= 4
                                           orderby month
                                           select month;

        Console.WriteLine("Месяцы, содержащие 'u' и длиной не менее 4-х:");
        foreach (string month in monthsWithUAndLengthAtLeast4)
        {
            Console.WriteLine(month);
        }
        Console.WriteLine($"Количество таких месяцев: {monthsWithUAndLengthAtLeast4.Count()}");

        Console.WriteLine("...........................................................................................");

        // Создаем и заполняем коллекцию List<Set>
        List<Set> sets = new List<Set>
        {
            new Set(new List<int> {1, 3, 5, 7}),              // Нечетные элементы
            new Set(new List<int> {2, 4, 6, 8}),              // Четные элементы
            new Set(new List<int> {-1, -2, -3}),              // Отрицательные элементы
            new Set(new List<int> {0, 2, 4, 6}),              // Содержит ноль и четные
            new Set(new List<int> {10, 20, 30, 40, 50}),      // Положительные числа
            new Set(new List<int> {-10, -20, 30}),            // Смешанные элементы
            new Set(new List<int> {3, 9, 15}),                // Нечетные числа
            new Set(new List<int> {100, 200, 300}),           // Крупные четные числа
            new Set(new List<int> {-5, -7, -9}),              // Нечетные отрицательные
            new Set(new List<int> {8, 12, 16, 20})            // Четные положительные
        };


       

        // Отобразим информацию обо всех множествах
        Console.WriteLine("Список всех множеств:");
            foreach (var set in sets)
            {
                Console.WriteLine(set);
            }

        Console.WriteLine("...........................................................................................");

        // 1. Запрос с использованием LINQ: выбор всех множеств, где все элементы четные
        var evenSets = sets.Where(set => set.IsOnlyEvenElements()).ToList();
            Console.WriteLine("\nМножества, содержащие только четные элементы (LINQ):");
            foreach (var set in evenSets)
            {
                Console.WriteLine(set);
            }

        Console.WriteLine("...........................................................................................");


        // 2. Множества только с нечетными элементами
        var oddSets = sets.Where(set => set.IsOnlyOddElements());
        Console.WriteLine("\nМножества только с нечетными элементами:");
        foreach (var set in oddSets)
        {
            Console.WriteLine(set);
        }
        Console.WriteLine("...........................................................................................");



        // 2. Запрос с использованием методов расширения LINQ: выбор всех множеств с отрицательными элементами
        var setsWithNegatives = sets.FindAll(set => set.HasNegativeElements());
            Console.WriteLine("\nМножества с отрицательными элементами (методы расширения LINQ):");
            foreach (var set in setsWithNegatives)
            {
                Console.WriteLine(set);
            }

        Console.WriteLine("...........................................................................................");

        // 4. Количество пустых множеств
        int emptySetCount = sets.Count(set => set.IsEmpty);
        Console.WriteLine($"\nКоличество пустых множеств: {emptySetCount}");

        // 5. Список множеств длины, принадлежащих заданному диапазону
        Console.WriteLine("...........................................................................................");

        Console.WriteLine("\nВведите минимальную длину множества:");
        int minRange = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите максимальную длину множества:");
        int maxRange = int.Parse(Console.ReadLine());

        Console.WriteLine("...........................................................................................");

        var setsInRange = sets.Where(set => set.Size >= minRange && set.Size <= maxRange);
        Console.WriteLine($"\nМножества длины в диапазоне [{minRange}, {maxRange}]:");
        foreach (var set in setsInRange)
        {
            Console.WriteLine(set);
        }
        Console.WriteLine("...........................................................................................");

        // Запрос для поиска множества с минимальным количеством элементов
        var minSet = sets.OrderBy(set => set.Size).FirstOrDefault();
        Console.WriteLine("\nМножество с минимальным количеством элементов:");
        Console.WriteLine(minSet != null ? minSet.ToString() : "Множество не найдено.");
        Console.WriteLine("...........................................................................................");






        Console.WriteLine("................................запрос с 5 операторами из разных категорий............................................");

        // Собственный запрос с 5 операторами из разных категорий
        var complexQuery = sets
            .Where(set => set.Elements.Any(x => x > 0) && set.Elements.Any(x => x < 0)) // Условие: множества с положительными и отрицательными элементами
            .Select(set => new { Id = set.Id, Description = $"ID: {set.Id}, Размер: {set.Size}, Элементы: {string.Join(", ", set.Elements)}" }) // Проекция
            .OrderByDescending(set => set.Description.Length) // Упорядочивание по длине строки описания (уменьшение)
            .GroupBy(set => set.Description.Contains("-")) // Группировка: наличие отрицательных чисел
            
        // Вывод результата
        foreach (var group in complexQuery)
        {
            Console.WriteLine($"\nГруппа: {group.Key}, Количество множеств: {group.Count}");
            foreach (var set in group.Sets)
            {
                Console.WriteLine(set.Description);
            }
        }
        Console.WriteLine("...........................................................................................");



    }
}
