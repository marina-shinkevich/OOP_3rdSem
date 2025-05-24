using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNamespace
{
    public class Set
    {
        private List<int> elements;
        public int Id;
        private static readonly int maxSize;
        public static int TotalSetsCreated;

        public Set()
        {
            elements = new List<int>();
            Id = ComputeHash();
            IncrementSetCount();
        }

        public Set(List<int> initialElements)
        {
            elements = new List<int>(initialElements);
            Id = ComputeHash();
            IncrementSetCount();
        }

        static Set()
        {
            maxSize = 100;
            TotalSetsCreated = 0;
        }

        public List<int> Elements => elements; // Свойство для доступа к элементам

        public int Size => elements.Count; // Свойство для получения размера множества

        public bool IsEmpty => elements.Count == 0; // Свойство для проверки, является ли множество пустым

        public override string ToString()
        {
            return $"Множество: {string.Join(", ", elements)}, Размер: {Size}";
        }

        public bool IsOnlyEvenElements()
        {
            return elements.All(x => x % 2 == 0) && elements.Count > 0;
        }

        public bool IsOnlyOddElements()
        {
            return elements.All(x => x % 2 != 0) && elements.Count > 0;
        }

        public bool HasNegativeElements()
        {
            return elements.Any(x => x < 0);
        }

        private void IncrementSetCount()
        {
            TotalSetsCreated++;
        }

        private int ComputeHash()
        {
            return elements.GetHashCode();
        }

        public static void DisplayClassInfo()
        {
            Console.WriteLine($"Класс Set. Всего создано множеств: {TotalSetsCreated}. Максимальный размер множества: {maxSize}");
        }
    }
    public class Tag
    {
        public int Id { get; set; } // Уникальный идентификатор тега
        public string Name { get; set; } // Название тега
    }
}