using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FurnitureStore
{
    
    public interface IAssemble
    {
        void Assemble();
        void ShowInfo();
    }

   
    public interface ISerializer
    {
        void Serialize<T>(T obj, string filePath); 
        T Deserialize<T>(string filePath); 
    }

    public class BinarySerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var formatter = new BinaryFormatter(); 
            using (var stream = new FileStream(filePath, FileMode.Create)) 

            {
               
                formatter.Serialize(stream, obj); 
            }
        }

        public T Deserialize<T>(string filePath)
        {
            var formatter = new BinaryFormatter(); 
            using (var stream = new FileStream(filePath, FileMode.Open)) 
            {
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public class SoapSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var formatter = new SoapFormatter();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(stream, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            var formatter = new SoapFormatter();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public class JsonSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)

        {
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            
                        var json = System.Text.Json.JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filePath, json); 
        }

        public T Deserialize<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }

    public class XmlSerializerWrapper : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T)); 
            

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream); 
            }
        }
    }


    [Serializable]
    public abstract class Product
    {
       
        public string Name { get; set; }
        [JsonIgnore]
        public decimal Price { get; set; }

        
        [NonSerialized]

        [XmlIgnore]

        public string Material ;

        protected Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public abstract void DisplayInfo();
        public abstract void ShowInfo();
    }

    [Serializable]
    public abstract class Furniture : Product, IAssemble
    {
        public string Color { get; set; }

        protected Furniture(string name, decimal price, string material, string color)
            : base(name, price)
        {
            Material = material;  
            Color = color;
        }

        public virtual void Assemble()
        {
            Console.WriteLine($"Сборка {Name} из {Material} цвета {Color}.");
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Мебель: {Name}, Материал: {Material}, Цвет: {Color}, Цена: {Price} руб.");
        }
    }

    [Serializable]
    public class Sofa : Furniture
    {
        
        public int SeatCount { get; set; }

        public Sofa() : base("Диван", 0, "Дерево", "Цвет") { }

        public Sofa(decimal price, string material, string color, int seatCount)
            : base("Диван", price, material, color)
        {
            SeatCount = seatCount;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name}: {Material}, {Color}, Кол-во мест: {SeatCount}, Цена: {Price} руб.");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // Коллекция объектов
            var sofas = new List<Sofa>
        {
            new Sofa(19999, "Дерево", "Синий", 3),
            new Sofa(15999, "Металл", "Красный", 2),
            new Sofa(24999, "Ткань", "Зеленый", 4)
        };


            
            var sofaArray = sofas.ToArray();


            
            ISerializer binarySerializer = new BinarySerializer();
            ISerializer soapSerializer = new SoapSerializer();
            ISerializer jsonSerializer = new JsonSerializer();
            ISerializer xmlSerializer = new XmlSerializerWrapper(); 

            
            binarySerializer.Serialize(sofas, "sofas_binary.dat");
            Console.WriteLine("Коллекция объектов сериализована в Binary формате.");

            
            var binaryDeserializedSofas = binarySerializer.Deserialize<List<Sofa>>("sofas_binary.dat");
            Console.WriteLine("Коллекция объектов десериализована из Binary формата:");
            foreach (var sofa in binaryDeserializedSofas)
            {
                sofa.ShowInfo();
            }



            
            jsonSerializer.Serialize(sofas, "sofas.json");
            Console.WriteLine("Коллекция объектов сериализована в JSON формате.");

            
            var jsonDeserializedSofas = jsonSerializer.Deserialize<List<Sofa>>("sofas.json");
            Console.WriteLine("Коллекция объектов десериализована из JSON формата:");
            foreach (var sofa in jsonDeserializedSofas)
            {
                sofa.ShowInfo();
            }

            
            xmlSerializer.Serialize(sofas, "sofas.xml");
            Console.WriteLine("Коллекция объектов сериализована в XML формате.");

            
            var xmlDeserializedSofas = xmlSerializer.Deserialize<List<Sofa>>("sofas.xml");
            Console.WriteLine("Коллекция объектов десериализована из XML формата:");
            foreach (var sofa in xmlDeserializedSofas)
            {
                sofa.ShowInfo();
            }

            
            soapSerializer.Serialize(sofaArray, "sofas_soap.xml");
            Console.WriteLine("Коллекция объектов сериализована в SOAP формате.");

            
            var soapDeserializedSofas = soapSerializer.Deserialize<Sofa[]>("sofas_soap.xml");
            Console.WriteLine("Коллекция объектов десериализована из SOAP формата:");
            foreach (var sofa in soapDeserializedSofas)
            {
                sofa.ShowInfo();
            }


            
            var xDoc = new XDocument( 
                new XElement("Sofas",
                    from sofa in sofas 
                    select new XElement("Sofa",
                    
                        new XElement("Name", sofa.Name), 
                        new XElement("Price", sofa.Price),
                        new XElement("Material", sofa.Material),
                        new XElement("Color", sofa.Color),
                        new XElement("SeatCount", sofa.SeatCount)
                    )
                )
            );

           

            string newXmlPath = "new.xml";
            xDoc.Save(newXmlPath);
            Console.WriteLine($"новый XML сохранен в {newXmlPath}");

           
            var wooSofas = xDoc.Descendants("Sofa")
    .Where(s => s.Element("Material")?.Value == "Дерево")
    .ToList();

            
            foreach (var sofa in wooSofas)
            {
                Console.WriteLine($"Название: {sofa.Element("Name")?.Value}, Материал: {sofa.Element("Material")?.Value}, Цена: {sofa.Element("Price")?.Value}");
            }

            var xmlDoc = new XmlDocument(); 
            xmlDoc.Load("new.xml"); 

            
            XmlNodeList sofaNodes = xmlDoc.SelectNodes("//Sofa");
            Console.WriteLine("\nВсе диваны в XML:");
            foreach (XmlNode node in sofaNodes)
            {
                Console.WriteLine($"Название: {node["Name"].InnerText}, Материал: {node["Material"].InnerText}");
            }

            
            XmlNodeList woodSofas = xmlDoc.SelectNodes("//Sofa[Material='Дерево']"); 
            Console.WriteLine("\nДиваны с материалом 'Дерево':");
            foreach (XmlNode node in woodSofas)
            {
                Console.WriteLine($"Название: {node["Name"].InnerText}, Материал: {node["Material"].InnerText}");
            }

        }
    }

}

