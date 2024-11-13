using System;

namespace FurnitureStore
{
    
    public interface IAssemble
    {
        void Assemble();
        void ShowInfo(); 
    }

    public abstract class Product
    {
        public string Name;
        public decimal Price;

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public abstract void DisplayInfo();
        public abstract void ShowInfo(); 

        public override string ToString()
        {
            return Name + ", Цена: " + Price + " руб.";
        }
    }

    // Базовый класс Мебель
    public abstract class Furniture : Product, IAssemble
    {
        public string Material;
        public string Color;

        public Furniture(string name, decimal price, string material, string color)
            : base(name, price)
        {
            Material = material;
            Color = color;
        }

        public virtual void Assemble()
        {
            Console.WriteLine($"Сборка {Name} из {Material} цвета {Color}.");
        }

        
        void IAssemble.ShowInfo()
        {
            Console.WriteLine($"Сборочная информация: {Name} из материала {Material} цвета {Color}.");
        }

       
        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Мебель: {Name}, Материал: {Material}, Цвет: {Color}, Цена: {Price} руб.");
        }
    }

    // Класс Диван
    public class Sofa : Furniture , IAssemble
    {
        public int SeatCount;

        public Sofa(decimal price, string material, string color, int seatCount)
            : base("Диван", price, material, color)
        {
            SeatCount = seatCount;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name}: {Material}, {Color}, Кол-во мест: {SeatCount}, Цена: {Price} руб.");
        }

       
        void IAssemble.ShowInfo()
        {
            Console.WriteLine($"Сборочная информация - Диван: Кол-во мест: {SeatCount}, Материал: {Material}, Цвет: {Color}");
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Диван: Кол-во мест: {SeatCount}, Материал: {Material}, Цвет: {Color}, Цена: {Price} руб.");
        }
    }

    // Класс Кровать
    public sealed class Bed : Furniture , IAssemble
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

        public override void Assemble()
        {
            Console.WriteLine($"Сборка кровати размера {Size} цвета {Color}.");
        }

        
        void IAssemble.ShowInfo()
        {
            Console.WriteLine($"Сборочная информация - Кровать: Размер {Size}, Материал: {Material}, Цвет: {Color}");
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Кровать: Размер: {Size}, Материал: {Material}, Цвет: {Color}, Цена: {Price} руб.");
        }
    }

    // Класс Вешалка
    public class Hanger : Product, IAssemble
    {
        public string Type;

        public Hanger(decimal price, string type) : base("Вешалка", price)
        {
            Type = type;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name}: {Type}, Цена: {Price} руб.");
        }

        public void Assemble()
        {
            Console.WriteLine($"Установка вешалки типа {Type}.");
        }

       
        void IAssemble.ShowInfo()
        {
            Console.WriteLine($"Сборочная информация - Вешалка: Тип {Type}");
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Вешалка: Тип: {Type}, Цена: {Price} руб.");
        }
    }

    // Класс Printer с полиморфным методом
    public class Printer
    {
        public void IAmPrinting(Product someObj)
        {
            Console.WriteLine(someObj.ToString());
            someObj.ShowInfo();
        }
    }

    class Program
    {
        static void Main()
        {
            var sofa = new Sofa(499, "Кожа", "Чёрный", 3);
            var bed = new Bed(799, "Дерево", "Белый", "King-size");
            var hanger = new Hanger(15, "Настенная");
            Product[] products = { sofa, bed, hanger };
            Printer printer = new Printer();
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("Демонстрация работы с объектами через интерфейсы и абстрактные классы:");
            Console.WriteLine("-------------------------------------------------------------------------------------");

            foreach (var product in products)
            {
                var assemble = product as IAssemble;
                if (assemble != null)
                {
                    assemble.ShowInfo();
                    assemble.Assemble();
                }


                product.ShowInfo();  
                Console.WriteLine();
            }
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("Использование класса Printer для вывода информации о товаре:");
            Console.WriteLine("-------------------------------------------------------------------------------------");


            foreach (var product in products)
            {
                printer.IAmPrinting(product);
                Console.WriteLine();
            }
        }
    }
}

