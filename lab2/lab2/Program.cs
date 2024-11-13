using System;
using System.Collections.Generic;
using System.Linq;

public class Set
{
    private List<int> elements;
    private readonly int id;
    private static readonly int maxSize;
    public static int TotalSetsCreated;

    public Set()
    {
        elements = new List<int>();
        id = ComputeHash();
        IncrementSetCount();
    }

    public Set(List<int> initialElements)
    {
        elements = new List<int>(initialElements);
        id = ComputeHash();
        IncrementSetCount();
    }

    public Set(int[]? initialElements = null)
    {
        elements = initialElements != null ? new List<int>(initialElements) : new List<int>();
        id = ComputeHash();
        IncrementSetCount();
    }
    //Закрытый,частные
    private Set(bool isPrivate, List<int> initialElements) 
    {
        elements = new List<int>(initialElements); 
        id = ComputeHash();
        IncrementSetCount();

    }

    static Set() 
    {
        maxSize = 100;
        TotalSetsCreated = 0;
    }

    private void IncrementSetCount() 
    {
        TotalSetsCreated++;
    }

    public List<int> Elements 
    {
        get { return elements; }
        set
        {
            if (value.Count <= maxSize)
            {
                elements = value;
            }
            else
            {
                throw new InvalidOperationException($"Превышен максимальный размер множества ({maxSize} элементов).");
            }
        }
    }

   

    private int ComputeHash()
    {
        return elements.GetHashCode();
    }

    public static Set CreatePrivateSet(List<int> initialElements)
    {
        return new Set(true, initialElements); 

    } 
    public void ModifySet(ref int element, out bool added)
    {

        if (elements.Count < maxSize && !elements.Contains(element))
        {
            elements.Add(element); 
            added = true;
        }
        else
        {
            added = false;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Set otherSet)
        {
            return elements.SequenceEqual(otherSet.elements);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (int element in elements)
        {
            hash = hash * 31 + element.GetHashCode();
        }
        return hash;
    }

    public override string ToString()
    {
        return $"Множество: {string.Join(", ", elements)}, ID: {id}, Количество элементов: {elements.Count}";
       
    }

    public static void DisplayClassInfo()
    {
        Console.WriteLine($"Класс Set. Всего создано множеств: {TotalSetsCreated}. Максимальный размер множества: {maxSize}");
    }

    // Методы работы с множествами
    public void Add(int element)
    {
        if (elements.Count < maxSize && !elements.Contains(element))
        {
            elements.Add(element);
        }
        else
        {
            Console.WriteLine("Множество переполнено или элемент уже существует.");
        }
    }

    public void Remove(int element)//удаление элемента
    {
        elements.Remove(element);
    }

    public Set Intersection(Set otherSet)// пересечение множеств
    {
        Set result = new Set();
        foreach (int element in elements)
        {
           
            if (otherSet.elements.Contains(element))
            {
                result.Add(element);
            }
        }
        return result;
    }

    public Set Difference(Set otherSet)// разность
    {
        Set result = new Set();
        foreach (int element in elements)
        {
            if (!otherSet.elements.Contains(element))
            {
                result.Add(element);
            }
        }
        return result;
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
}


public partial class MyPartialClass
{
    public void DisplayMessagePart1()
    {
        Console.WriteLine("Это первая часть partial класса.");
    }
}

public partial class MyPartialClass
{
    public void DisplayMessagePart2()
    {
        Console.WriteLine("Это вторая часть partial класса.");
    }
}


class Program
{
    static void Main()
    {
        // Создание множеств
        Set set1 = new Set(new List<int> { 2, 4, 6 });
        Set set2 = new Set(new List<int> { -2, -3, 5 });
        Set set3 = new Set(new List<int> { 1, 3, 5 });

        // Вызов свойств
        Console.WriteLine("Созданные множества:");
        Console.WriteLine(set1);
        Console.WriteLine(set2);
        Console.WriteLine(set3);
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        // Вызов статического метода для создания множества с использованием закрытого конструктора

        Set privateSet = Set.CreatePrivateSet([7, 8, 9]);
        Console.WriteLine("Созданное частное множество:");
        Console.WriteLine(privateSet);
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        // Вывод количества созданных объектов

        Console.WriteLine("\nВся информация о классе:");
        Set.DisplayClassInfo();
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        // Сравнение объектов
        Console.WriteLine("\nСравнение множеств:");
        Console.WriteLine($"Множество 1 и 2 равны: {set1.Equals(set2)}");
        Console.WriteLine($"Множество 2 и 3 равны: {set2.Equals(set3)}");
        Console.WriteLine($"Множество 1 и 3 равны: {set1.Equals(set3)}");
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        // Проверка типов созданных объектов
        Console.WriteLine($"\nТип множества 1: {set1.GetType()}");
        Console.WriteLine($"Тип множества 2: {set2.GetType()}");
        Console.WriteLine($"Тип множества 3: {set3.GetType()}");
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        // Проверка условий и вывод результатов
        Console.WriteLine("\nМножества, содержащие только четные элементы:");
        if (set1.IsOnlyEvenElements())
            Console.WriteLine(set1);

        Console.WriteLine("\nМножества, содержащие только нечетные элементы:");
        if (set2.IsOnlyOddElements())
            Console.WriteLine(set2);
        if (set3.IsOnlyOddElements())
            Console.WriteLine(set3);

        Console.WriteLine("\nМножества, содержащие отрицательные элементы:");
        if (set2.HasNegativeElements())
            Console.WriteLine(set2);
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        // Использование метода ModifySet с ref и out параметрами
        int newElement = 7;
        bool wasAdded;
        set3.ModifySet(ref newElement, out wasAdded);
        Console.WriteLine($"\nЭлемент {newElement} был добавлен в множество 3: {wasAdded}");
        Console.WriteLine("Обновленное состояние множества:");
        Console.WriteLine(set3.ToString());
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        int elementToRemove = 2;
        set1.Remove(elementToRemove);
        Console.WriteLine($"Элемент {elementToRemove} был удален.");
        Console.WriteLine(set1.ToString());
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        Set intersectionSet = set2.Intersection(set3);
        Console.WriteLine("Пересечение множеств:");
        Console.WriteLine(intersectionSet.ToString());

        Console.WriteLine("\n-------------------------------------------------------------------------------");

        Set differenceSet = set1.Difference(set2);
        Console.WriteLine("Разность первого множества относительно второго:");
        Console.WriteLine(differenceSet.ToString());
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        
        Console.WriteLine("\nИнформация о множестве 1: " + set1.ToString());

        // Создание анонимного типа
        var anonymousSet = new
        {
            Elements = new List<int> { 3, 4, 6 },
            ID = new Random().Next(1000, 9999) 
        };


        Console.WriteLine("\n-------------------------------------------------------------------------------");


        // Вывод анонимного типа
        Console.WriteLine("\nАнонимный тип:");
        Console.WriteLine($"Элементы: {string.Join(", ", anonymousSet.Elements)}, ID: {anonymousSet.ID}");

        MyPartialClass myPartial = new MyPartialClass();
        Console.WriteLine("\n-------------------------------------------------------------------------------");

        // Вызываем методы из обеих частей класса
        myPartial.DisplayMessagePart1();  // Выведет: Это первая часть partial класса.
        myPartial.DisplayMessagePart2();

    }
}
