using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lab11
{
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

        public abstract void DisplayInfo(string additionalDetails);
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
        public Bed() : base("Кровать", 220, "Дерево", "Черный")
        {
            Size = "220x600";
        }

        
        public override void DisplayInfo(string additionalDetails)
        {
            Console.WriteLine($"{Name}: {Material}, {Color}, Размер: {Size}, Цена: {Price} руб. Дополнительно: {additionalDetails}");
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Кровать: Размер: {Size}, Материал: {Material}, Цвет: {Color}, Цена: {Price} руб.");
        }

        public void Assemble()
        {
            Console.WriteLine($"Сборка кровати размера {Size} цвета {Color}.");
        }

        public override string ToStringRepresentation()
        {
            return base.ToStringRepresentation() + $",{Size}";
        }
    }

    public static class Reflector
    {
        public static string AssemblyName(string className)
        {
            Type type = Type.GetType(className);
            return type?.Assembly.FullName ?? "Класс не найден.";
        }

        public static bool FindPublicConstructors(string className)
        {
            Type type = Type.GetType(className);
            return type?.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any() ?? false;
        }

        public static IEnumerable<string> FindMethods(string className)
        {
            Type type = Type.GetType(className);
            return type?.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Select(method => method.Name) ?? Enumerable.Empty<string>();
        }

        public static IEnumerable<string> FindInterfaces(string className)
        {
            Type type = Type.GetType(className);
            return type?.GetInterfaces().Select(i => i.Name) ?? Enumerable.Empty<string>();
        }

        public static IEnumerable<string> AllFieldsAndProperties(string className)
        {
            Type type = Type.GetType(className);
            if (type == null) return Enumerable.Empty<string>();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                             .Select(field => $"Field: {field.Name}");
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                 .Select(property => $"Property: {property.Name}");

            return fields.Concat(properties);
        }

        public static void Invoke(string className, string methodName, string parametersFilePath)
        {
            Type type = Type.GetType(className);
            if (type == null)
            {
                Console.WriteLine("Класс не найден.");
                return;
            }

            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                Console.WriteLine("Метод не найден.");
                return;
            }

            object instance = Activator.CreateInstance(type);
            var parameters = ReadParametersFromFile(parametersFilePath, method);

            method.Invoke(instance, parameters);
        }

        private static object[] ReadParametersFromFile(string parametersFilePath, MethodInfo method)
        {
            if (!File.Exists(parametersFilePath))
                throw new FileNotFoundException($"Файл '{parametersFilePath}' не найден.");

            var lines = File.ReadAllLines(parametersFilePath);
            var parameters = new List<object>();
            var methodParams = method.GetParameters();

            for (int i = 0; i < methodParams.Length; i++)
            {
                var paramType = methodParams[i].ParameterType;
                var value = Convert.ChangeType(lines[i], paramType);
                parameters.Add(value);
            }

            return parameters.ToArray();
        }
        public static T Create<T>(params object[] parameters) where T : class
        {
            Type type = typeof(T);

            // Найти подходящий конструктор по количеству и типам параметров
            ConstructorInfo constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(c =>  c.GetParameters().Length == parameters.Length &&
            c.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters.Select(p => p?.GetType())));

            if (constructor == null)
                throw new Exception("Подходящий публичный конструктор не найден.");

            
            return (T)constructor.Invoke(parameters);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            Bed bed = new Bed(25000, "Дерево", "Белый", "200x160");

            // Работа с рефлексией
            var typeOfClass = bed.GetType().FullName;
            Console.WriteLine(typeOfClass);
            Console.WriteLine("Assembly Name: " + Reflector.AssemblyName(typeOfClass));
            Console.WriteLine("Has Public Constructors: " + Reflector.FindPublicConstructors(typeOfClass));
            Console.WriteLine("Public Methods: " + string.Join(", ", Reflector.FindMethods(typeOfClass)));
            Console.WriteLine("Fields and Properties: " + string.Join(", ", Reflector.AllFieldsAndProperties(typeOfClass)));
            Console.WriteLine("Implemented Interfaces: " + string.Join(", ", Reflector.FindInterfaces(typeOfClass)));

            // Сохранение параметров в файл и вызов метода через рефлексию
            File.WriteAllLines("params.txt", new string[] { "Эта кровать имеет мягкое изголовье." });
            Reflector.Invoke(typeOfClass, "DisplayInfo", "params.txt");

            // Демонстрация полиморфизма
            bed.DisplayInfo("Дополнительная информация о кровати.");
            bed.ShowInfo();

            Bed bedTw = Reflector.Create<Bed>();
            Console.WriteLine("Объект Bed успешно создан:");
            bedTw.ShowInfo();

            // Пример создания с дополнительной информацией
            bedTw.DisplayInfo("Этот объект был создан с помощью метода Reflector.Create<T>.");
        }
    }
}
