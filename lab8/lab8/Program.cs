using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProgrammerEventApp
{
    public delegate void RenameDelegate(string newName);
    public delegate void NewPropertyDelegate(string property);
    public class Programmer
    {
        public string Name;

        public event RenameDelegate OnRename;
        public event NewPropertyDelegate OnNewProperty;

        public Programmer(string name)
        {
            Name = name;
        }

        public void Rename(string newName)
        {
            Name = newName;
            OnRename?.Invoke(newName);
        }

        public void AddNewProperty(string property)
        {
            OnNewProperty?.Invoke(property);
        }


        public Func<string, string> CombinedOperations = input =>
        {
            input = Regex.Replace(input, "[^\\w\\s]", "");
            input = $"^^{input}^^";
            input = input.ToUpper();
            input = Regex.Replace(input, "\\s+", " ").Trim();
            input = input.Replace("УЧИСЬ", "НЕ ЛЕНИСЬ");
            return input;
        };
        

        public string ProcessString(string input, Func<string, string> operation)
        {
            return operation(input);
        }
    }

    public class ProgrammingLanguage
    {
        public string Name;
        public string Version;
        public List<string> Technologies;

        public ProgrammingLanguage(string name, string version)
        {
            Name = name;
            Version = version;
            Technologies = new List<string>();
        }

        public void OnProgrammerRenamed(string newName)
        {
            Version = "Обновленная";
            Console.WriteLine($"Версия языка {Name} обновлена, так как изменилось имя программиста: версия '{Version}'");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }

        public void OnNewPropertyAdded(string property)
        {
            Technologies.Add(property);
            Console.WriteLine($"{Name} язык добавил новую технологию: {property}");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Язык: {Name}, Версия: {Version}, Технология: {string.Join(", ", Technologies)}");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Programmer programmer = new Programmer("Катя");

            ProgrammingLanguage csharp = new ProgrammingLanguage("C#", "9.0");
            ProgrammingLanguage python = new ProgrammingLanguage("Python", "3.9");
            ProgrammingLanguage javascript = new ProgrammingLanguage("JavaScript", "ES6");

            programmer.OnRename += csharp.OnProgrammerRenamed;
            programmer.OnRename += python.OnProgrammerRenamed;
            programmer.OnNewProperty += python.OnNewPropertyAdded;
            programmer.OnNewProperty += javascript.OnNewPropertyAdded;

            programmer.Rename("Марина");
            programmer.AddNewProperty("Machine Learning");

            Console.WriteLine("-------------------------------------------------------------------------------------");

            csharp.DisplayInfo();
            python.DisplayInfo();
            javascript.DisplayInfo();

            string input = "        Живи, учись,        смейся.     ";

            string processedString = programmer.ProcessString(input, programmer.CombinedOperations);
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("\nНачальная строка: " + input);
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("Строка после обработки: " + processedString);
        }
    }
}
