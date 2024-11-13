using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
public abstract class Product
{
    public string Name;
    public decimal Price;
    public string Material;
    public string Color;

    public Product(string name, decimal price, string material, string color)
    {
        Name = name;
        Price = price;
        Material = material;
        Color = color;
    }

    public abstract void DisplayInfo();
    public abstract void ShowInfo();

   
    public virtual string ToStringRepresentation()
    {
        return $"{Name},{Price},{Material},{Color}";
    }

}


public interface IAssemble
{
    void Assemble();
    void ShowInfo();
}


public class Bed : Product, IAssemble
{
    public string Size;

    public Bed(decimal price, string material, string color, string size)
        : base("Кровать", price, material, color)
    {
        Size = size;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"{Name}: {Material}, {Color}, Размер: {Size}, Цена: {Price} руб.");
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Общая информация - Кровать: Размер: {Size}, Материал: {Material}, Цвет: {Color}, Цена: {Price} руб.");
    }

    public void Assemble()
    {
        Console.WriteLine($"Сборка кровати размера {Size} цвета {Color}.");
    }

    void IAssemble.ShowInfo()
    {
        Console.WriteLine($"Сборочная информация - Кровать: Размер {Size}, Материал: {Material}, Цвет: {Color}");
    }

    public override string ToStringRepresentation()
    {
        return base.ToStringRepresentation() + $",{Size}";
    }

   
}

public interface Interf<T>
{
    void Add(T item);
    void Remove(T item);
    T View(int predicate);
}


public class CollectionType<T> : Interf<T> where T : Product
{
    private List<T> _items = new List<T>();

    public void Add(T item)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item cannot be null");
            _items.Add(item);
            Console.WriteLine($"{item.Name} добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Операция добавления завершена.");
        }
    }

    public void Remove(T item)
    {
        try
        {
            if (!_items.Contains(item))
                throw new ArgumentException("Элемент не найден в коллекции");
            _items.Remove(item);
            Console.WriteLine($"{item.Name} удален.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Операция удаления завершена.");
        }
    }

    public T View(int predicate)
    {
        try
        {
            if (predicate < 0 || predicate >= _items.Count)
                throw new ArgumentOutOfRangeException("Такого элемента нет!");
           
            T item = _items.ElementAt(predicate);

            
            if (item is Product product)
            {
                product.DisplayInfo();  
            }
            else
            {
                Console.WriteLine($"Элемент: {item}");
            }

            return item;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return default;
        }
        finally
        {
            Console.WriteLine("Метод View завершён.");
        }
    }

  
    public void SaveToFile(string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in _items)
                {
                    writer.WriteLine(item.ToStringRepresentation());
                }
            }
            Console.WriteLine("Коллекция успешно сохранена в файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
        }
    }


    public void LoadFromFile(string filePath)
    {
        
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"Прочитали из файла: {line}");

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении из файла: {ex.Message}");
        }
        
    }
}

public class GenericCollectionType<T> : Interf<T>where T : struct
{
    private List<T> _items = new List<T>();

    public void Add(T item)
    {
        try
        {
            _items.Add(item);
            Console.WriteLine($"Элемент {item} добавлен в коллекцию.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Операция добавления завершена.");
        }
    }

    public void Remove(T item)
    {
        try
        {
            if (!_items.Contains(item))
                throw new ArgumentException("Элемент не найден в коллекции");
            _items.Remove(item);
            Console.WriteLine($"Элемент {item} удален из коллекции.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Операция удаления завершена.");
        }
    }

    public T View(int predicate)
    {
        try
        {
            
            if (predicate < 0 || predicate >= _items.Count)
                throw new ArgumentOutOfRangeException("index", "Такого элемента нет!");

            
            T item = _items.ElementAt(predicate);

            
            Console.WriteLine($"Элемент с индексом {predicate}: {item}");

            return item;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return default; 
        }
        finally
        {
            Console.WriteLine("Метод View завершён.");
        }
    }


    
    public void SaveToFile(string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in _items)
                {
                    writer.WriteLine(item.ToString());
                }
            }
            Console.WriteLine("Коллекция успешно сохранена в файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
        }
    }


    public void LoadFromFile(string filePath)
    {
       
      
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {


                    Console.WriteLine($"{line}");

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении из файла: {ex.Message}");
        }
      
    }
}

public class Program
{
    public static void Main()
    {
        CollectionType<Bed> bedCollection = new CollectionType<Bed>();

        // Создание 
        Bed bed1 = new Bed(15000m, "Дерево", "Белый", "Queen");
        Bed bed3 = new Bed(17000m, "Дерево", "Белый", "Simple");
        Bed bed2 = new Bed(20000m, "МДФ", "Черный", "King");

        // Добавление 
        bedCollection.Add(bed1);
        bedCollection.Add(bed2);
        bedCollection.Add(bed3);
        Console.WriteLine("-----------------------------------------------------------------------------------------");

        // Удаление 
        bedCollection.Remove(bed1);
        Console.WriteLine("-----------------------------------------------------------------------------------------");

        // Поиск
        Bed item = bedCollection.View(0);
        

        // Сохранение 
        string filePath = "beds.txt";
        bedCollection.SaveToFile(filePath);
        Console.WriteLine("-----------------------------------------------------------------------------------------");

        bedCollection.LoadFromFile(filePath);

        Console.WriteLine("-----------------------------------------------------------------------------------------");


        // для стандартных типов данных
        GenericCollectionType<int> intCollection = new GenericCollectionType<int>();
       
        intCollection.Add(12);
        intCollection.Add(1);
        intCollection.Add(25);
        intCollection.Add(20);
        intCollection.Add(22);
        Console.WriteLine("-----------------------------------------------------------------------------------------");

        // Сохранение 
        string intFilePath = "integers.txt";
        intCollection.SaveToFile(intFilePath);
        Console.WriteLine("-----------------------------------------------------------------------------------------");
        intCollection.Remove(1);
       
        Console.WriteLine("-----------------------------------------------------------------------------------------");
       int foundInts = intCollection.View(0);

        Console.WriteLine("Прочитанные элементы из файла:");

        intCollection.LoadFromFile(intFilePath);
        Console.WriteLine("-----------------------------------------------------------------------------------------");
    }
}