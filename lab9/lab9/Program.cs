using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ConcertApp
{
    public interface IConcertManager
    {
        void AddConcert(string id, Concert concert);      // Добавить концерт
        void RemoveConcert(string id);                   // Удалить концерт
        Concert GetConcertById(string id);               // Получить концерт по ID
        void DisplayAllConcerts();                       // Показать все концерты
    }

    // Класс "Концерт"
    public class Concert
    {
        public string Title;    // Название концерта
        public DateTime Date;   // Дата концерта
        public string Location; // Место проведения

        public Concert(string title, DateTime date, string location)
        {
            Title = title;
            Date = date;
            Location = location;
        }

        public override string ToString()
        {
            return $"Title: {Title}, Date: {Date.ToShortDateString()}, Location: {Location}";
        }
    }

    // Класс для управления коллекцией концертов
    public class ConcertManager : IConcertManager
    {
        private Dictionary<string, Concert> concerts = new Dictionary<string, Concert>();

        public void AddConcert(string id, Concert concert)
        {
            if (!concerts.ContainsKey(id))
            {
                concerts.Add(id, concert);
                Console.WriteLine($"Концерт с идентификатором {id} добавлен.");
            }
            else
            {
                Console.WriteLine($"Концерт с идентификатором {id} уже существует.");
            }
        }

        public void RemoveConcert(string id)
        {
            if (concerts.Remove(id))
            {
                Console.WriteLine($"Концерт с идентификатором {id} удалён.");
            }
            else
            {
                Console.WriteLine($"Концерт с идентификатором {id} не найден.");
            }
        }

        public Concert GetConcertById(string id)
        {
            foreach (var pair in concerts)
            {
                if (pair.Key == id)
                {
                    return pair.Value;
                }
            }

            Console.WriteLine($"Концерт с идентификатором {id} не найден.");
            return null;
        }


        public void DisplayAllConcerts()
        {
            if (concerts.Count > 0)
            {
                Console.WriteLine("\nAll concerts:");
                foreach (var pair in concerts)
                {
                    Console.WriteLine($"ID: {pair.Key}, {pair.Value}");
                }
            }
            else
            {
                Console.WriteLine("No concerts available.");
            }
        }
    }

    // Тестирование
    class Program
    {
        private static void ConcertCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Console.WriteLine("Добавлен новый элемент в коллекцию.");
                    foreach (Concert concert in e.NewItems)
                    {
                        Console.WriteLine($"Добавлен: {concert}");
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Console.WriteLine("Удален элемент из коллекции.");
                    foreach (Concert concert in e.OldItems)
                    {
                        Console.WriteLine($"Удален: {concert}");
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Console.WriteLine("Коллекция была сброшена.");
                    break;
            }
        }

            static void Main(string[] args)
        {
            // Создаем объект ConcertManager
            ConcertManager manager = new ConcertManager();

            // Добавляем концерты
            manager.AddConcert("1", new Concert("Rock Night", new DateTime(2024, 11, 30), "New York"));
            manager.AddConcert("2", new Concert("Jazz Evening", new DateTime(2024, 12, 15), "Chicago"));
            manager.AddConcert("3", new Concert("Classical Music", new DateTime(2025, 1, 10), "Los Angeles"));

            // Вывод всех концертов
            manager.DisplayAllConcerts();

            // Поиск концерта по ID
            Console.WriteLine("\nSearching for concert with ID 2:");
            Concert concert = manager.GetConcertById("2");
            if (concert != null)
            {
                Console.WriteLine(concert);
            }

            // Удаление концерта
            Console.WriteLine("\nRemoving concert with ID 2:");
            manager.RemoveConcert("2");

            // Повторный вывод всех концертов
            manager.DisplayAllConcerts();

            // Добавление концерта с существующим ID
            Console.WriteLine("\nAdding concert with existing ID 1:");
            manager.AddConcert("1", new Concert("Pop Night", new DateTime(2025, 2, 5), "Miami"));

            // Добавление нового концерта
            Console.WriteLine("\nAdding new concert with ID 4:");
            manager.AddConcert("4", new Concert("Folk Festival", new DateTime(2025, 3, 20), "Austin"));

            // Окончательный вывод всех концертов
            manager.DisplayAllConcerts();


            Dictionary<int, char> myDictionary = new Dictionary<int, char> { };
            myDictionary.Add(1, 'f');
            myDictionary.Add(2, 'a');
            myDictionary.Add(3, 'b');
            myDictionary.Add(4, 'v');
          

            Console.WriteLine("Initial Dictionary:");
            foreach (var item in myDictionary)
            {
                Console.WriteLine($"Ключ: {item.Key}, Значение: {item.Value}");
            }

            int n = 2; // Количество удаляемых элементов
            Console.WriteLine($"Removing {n} elements from the dictionary");
            var keysToRemove = new List<int>();
            foreach (var key in myDictionary.Keys)
            {
                if (keysToRemove.Count < n)
                {
                    keysToRemove.Add(key);
                }
                else
                {
                    break;
                }
            }

            foreach (var key in keysToRemove)
            {
                myDictionary.Remove(key);
            }

            Console.WriteLine("\nDictionary after removal:");
            foreach (var item in myDictionary)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            myDictionary[5] ='c';
            bool added = myDictionary.TryAdd(6, 'k');
            Console.WriteLine("\nКоллекция после добавления:");

            foreach (var item in myDictionary)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            List<char> valueOnlyList = new List<char>();
            foreach (var item in myDictionary)
            {
                valueOnlyList.Add(item.Value);
            }
            Console.WriteLine("\nLinkedList with only Values from Dictionary:");
            foreach (var item in valueOnlyList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("\nSearching for a specific char value in LinkedList (values only):");
            char searchChar = 'c'; // Значение для поиска

            if (valueOnlyList.Contains(searchChar))
            {
                Console.WriteLine($"The value '{searchChar}' is found in the LinkedList.");
            }
            else
            {
                Console.WriteLine($"The value '{searchChar}' is not found in the LinkedList.");
            }




            ObservableCollection<Concert> concertCollection = new ObservableCollection<Concert>();

            // Регистрация метода на событие CollectionChanged
            concertCollection.CollectionChanged += ConcertCollectionChanged;

            concertCollection.Add(new Concert("Rock Night", new DateTime(2024, 11, 30), "New York"));
            concertCollection.RemoveAt(0);

        }
    }
}

