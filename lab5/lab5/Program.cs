using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace FurnitureStore
{
    
    public class FurnitureException : Exception
    {
        public FurnitureException(string message) : base(message) { }
    }

    public class InvalidProductDataException : FurnitureException
    {
        public InvalidProductDataException(string message) : base(message) { }
    }

    public class OutOfStockException : FurnitureException
    {
        public OutOfStockException(string message) : base(message) { }
    }

    public class ProductNotFoundException : FurnitureException
    {
        public ProductNotFoundException(string message) : base(message) { }
    }
    public interface IAssemble
    {
        void Assemble();
        void ShowInfo();

    }

    public enum MaterialType
    {
        Wood,
        Metal,
        Leather,
        Fabric
    }

    public struct Dimensions
    {
        public double Width;
        public double Height;

        public Dimensions(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{Width}x{Height} см";
        }
    }

    public abstract class Product
    {
        public string Name;
        public decimal Price;
        public double Weight;

        public Product(string name, decimal price, double weight)
        {
            Name = name;
            Price = price;
            Weight = weight;
            Debug.Assert(price >= 0, "Цена продукта должна быть положительной"); 
        }

        public abstract void DisplayInfo();
        public abstract void ShowInfo();

        public override string ToString()
        {
            return $"{Name}, Цена: {Price} руб., Вес: {Weight} кг";
        }


    }

    public class Sofa : Furniture
    {
        public int SeatCount;

        public Sofa(decimal price, double weight, MaterialType material, string color, int seatCount, Dimensions size)
            : base("Диван", price, weight, material, color, size)
        {
            SeatCount = seatCount;
        }
        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name}: {Material}, {Color}, Кол-во мест: {SeatCount}, Размеры: {Size}, Цена: {Price} руб.");
        }


    }

    public class Bed : Furniture
    {
        public string BedSize;

        public Bed(decimal price, double weight, MaterialType material, string color, string bedSize, Dimensions size)
            : base("Кровать", price, weight, material, color, size)
        {
            BedSize = bedSize;
        }
        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name}: {Material}, {Color}, Размер: {BedSize}, Габариты: {Size}, Цена: {Price} руб.");
        }


    }

    public class Hanger : Product, IAssemble
    {
        public string Type;

        public Hanger(decimal price, double weight, string type)
            : base("Вешалка", price, weight)
        {
            Type = type;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name}: {Type}, Цена: {Price} руб., Вес: {Weight} кг");
        }

        public void Assemble()
        {
            Console.WriteLine($"Установка вешалки типа {Type}.");
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Вешалка: Тип: {Type}, Цена: {Price} руб., Вес: {Weight} кг");
        }
    }

    public class Warehouse 
    {
       
        private List<Product> products = new List<Product>();

        public List<Product> Products 
        {
            get { return products; }
            set { products = value; }
        }

        public Warehouse()
        {
            Products = new List<Product>();
        }

      
        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product", "Продукт не может быть null");
            if (product.Price < 0)
            {
                throw new InvalidProductDataException("Цена продукта не может быть отрицательной");
  
               
            }
            Products.Add(product);
        }

      
        public void RemoveProduct(Product product)
        {
            if (!Products.Contains(product))
            {
                throw new ProductNotFoundException("Продукт не найден на складе");
                
               
            }
            Products.Remove(product);
        }


        // Метод для вывода всех продуктов на консоль
        public void DisplayProducts()
        {
            if (Products.Count == 0)
            {
                throw new OutOfStockException("Склад пуст.");
            }

            foreach (var product in Products)
            {
                product.ShowInfo();
            }
        }


        public IEnumerable<Product> FindByManufacturer(string manufacturer)
        {
            
            List<Product> foundProducts = new List<Product>();

           
            foreach (Product product in products)
            {
                
                if (product.Name.Contains(manufacturer))
                {
                    
                    foundProducts.Add(product);
                }
            }

           
            return foundProducts;
        }


        public decimal GetTotalPriceByType(Type type)
        {
            decimal totalPrice = 0; 

            
            foreach (Product product in products)
            {
                
                if (product.GetType() == type)
                {

                    totalPrice += product.Price;
                }
            }


            return totalPrice;
        }


        public List<Product> GetProducts()
        {
            return products; 
        }

    }


    public class PriceComparer : IComparer<Product> 
    {
        public int Compare(Product x, Product y)
        {
            if (x == null || y == null) return 0;
            return x.Price.CompareTo(y.Price);
        }
    }

    public class WeightComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x == null || y == null) return 0;
            return x.Weight.CompareTo(y.Weight);
        }
    }
    public class WarehouseController
    {
        private Warehouse warehouse;

        public WarehouseController(Warehouse warehouse) 
        {
            this.warehouse = warehouse;
        }

        public void AddProduct(Product product)
        {
            warehouse.AddProduct(product);
        }

        public void RemoveProduct(Product product)
        {
            warehouse.RemoveProduct(product);
        }

        public void ShowAllProducts()
        {
            foreach (var product in warehouse.GetProducts())
            {
                product.ShowInfo();
            }
        }

        public void ShowProductsByManufacturer(string manufacturer)
        {
            var products = warehouse.FindByManufacturer(manufacturer);
            foreach (var product in products)
            {
                product.ShowInfo();
            }
        }

        public void SortProductsByPrice()
        {
            var sortedProducts = warehouse.GetProducts();
            sortedProducts.Sort(new PriceComparer());
            foreach (var product in sortedProducts)
            {
                product.ShowInfo();
            }
        }

        public void SortProductsByWeight()
        {
            var sortedProducts = warehouse.GetProducts();
            sortedProducts.Sort(new WeightComparer());
            foreach (var product in sortedProducts)
            {
                product.ShowInfo();
            }
        }

        public void ShowTotalPriceByType(Type type)
        {
            decimal totalPrice = warehouse.GetTotalPriceByType(type);
            Console.WriteLine($"Общая стоимость товаров типа {type.Name}: {totalPrice} руб.");
        }
    }
    class Program
    {


        static void Main()
        {
            Dimensions sofaDimensions = new Dimensions(200, 90);

            Warehouse warehouse = new Warehouse();
            WarehouseController controller = new WarehouseController(warehouse);

            try
            {
                // Проверка на добавление недопустимого товара
                var invalidSofa = new Sofa(100, 0, MaterialType.Leather, "Неверный", 0, sofaDimensions); 
                controller.AddProduct(invalidSofa);
                controller.RemoveProduct(invalidSofa);
                controller.RemoveProduct(invalidSofa);
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine($"Ошибка поиска продукта: {ex.Message}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

                Console.WriteLine($"Диагностика: {ex.StackTrace}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

            }

            try {

                Warehouse warehouset = new Warehouse();
                // Новый пустой склад
                warehouset.DisplayProducts();
            }
            catch (OutOfStockException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

                Console.WriteLine($"Диагностика: {ex.StackTrace}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

            }
            try
            {
                var nonexistentProduct = new Sofa(299, 70, MaterialType.Wood, "Синий", 2, sofaDimensions);
                warehouse.RemoveProduct(nonexistentProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

                Console.WriteLine($"Диагностика: {ex.StackTrace}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

            }


            Console.WriteLine("--------------------------------------------Проброс вверх-------------------------------------------------");

            try
            {
                firstMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Обработка исключения в main" + ex.ToString() + "    ");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

                Console.WriteLine("Диагностика" + ex.StackTrace + "    ");
                Console.WriteLine("---------------------------------------------------------------------------------------------");
            }

            void firstMethod()
            {
                try
                {
                    int t = 0;
                    int res = 7 / t;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Обработка в первом методе" + ex.ToString() + "    ");
                    Console.WriteLine("---------------------------------------------------------------------------------------------");

                    throw; 
                }
            }

           

            try
            {
                Product nullProduct = null;
                warehouse.AddProduct(nullProduct);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");
                Console.WriteLine($"Диагностика: {ex.StackTrace}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");

                
            }
          

            
            try
            {
             
                var invalidSofa = new Sofa(-100, 90, MaterialType.Leather, "Неверный", 0, sofaDimensions); 
                warehouse.AddProduct(invalidSofa);

            }
            
            catch (InvalidProductDataException ex)
            {
                Console.WriteLine($"Ошибка инициализации продукта: {ex.Message}");
                Console.WriteLine($"Диагностика: {ex.StackTrace}");
                
            }



            finally
            {
                Console.WriteLine("Завершение обработки исключений.");
            }
        }
    }
}