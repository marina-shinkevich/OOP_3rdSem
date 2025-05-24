using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace laba14
{
    public static class ProcessManager
    {
        public static void ListProcesses()
        {
            Process[] processes = Process.GetProcesses();

            using (StreamWriter file = new StreamWriter("C:\\Users\\User\\Documents\\2курс1сем\\лабыООП\\lab14\\lab14\\processes.txt"))

            {
                foreach (Process process in processes)
                {
                    try
                    {
                        file.WriteLine($"ID: {process.Id}, Имя: {process.ProcessName}, Приоритет: {process.BasePriority}, Время начала: {process.StartTime}, Сколько времени использовался: {process.TotalProcessorTime}");
                        Console.WriteLine($"ID: {process.Id}, Имя: {process.ProcessName}, Приоритет: {process.BasePriority}, Время начала: {process.StartTime} Сколько времени использовался: {process.TotalProcessorTime}");
                    }
                    catch (Exception ex)
                    {
                        file.WriteLine($"Процесс {process.Id} ({process.ProcessName}) вызвал ошибку: {ex.Message}");
                        Console.WriteLine($"Не удалось получить подробную информацию о процессе {process.ProcessName}: {ex.Message}");
                    }
                }
            }
        }
    }

    public static class AppDomainManager
    {
        public static void ManipulateAppDomain()
        {
            Console.WriteLine($"Имя текущего домена: {AppDomain.CurrentDomain.FriendlyName}"); 
            Console.WriteLine("Загруженные сборки в текущий домен:");
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) 
            {
                Console.WriteLine($"- {asm.FullName}");
            }

            
            Console.WriteLine("\nСоздаём новый контекст загрузки сборок...");
            var context = new AssemblyLoadContext("NewAssemblyLoadContext", isCollectible: true);

            try
            {
                
                Console.WriteLine("\nЗагружаем сборку System.Text.Json...");
                
                var assembly = context.LoadFromAssemblyName(new AssemblyName("System.Text.Json"));
                Console.WriteLine($"Сборка {assembly.FullName} успешно загружена в новый контекст.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке сборки: {ex.Message}");
            }
            finally
            {
                
                Console.WriteLine("\nВыгружаем контекст загрузки...");
                context.Unload();
                Console.WriteLine("Контекст загрузки успешно выгружен.");
            }
        }
    }


    public static class PrimeCalculator
    {
        private static Thread primeThread; 
        private static bool isPaused = false;

        public static void CalculateInThread()
        {
            Console.Write("Введите n: ");
            int n = int.Parse(Console.ReadLine());

            primeThread = new Thread(() => CalculatePrimes(n));
            primeThread.Start(); 
            
            Thread.Sleep(3000);
            PauseThread();
            Thread.Sleep(2000);
            ResumeThread();

            
        }

        private static void CalculatePrimes(int n) 
        {
            
            using (StreamWriter writer = new StreamWriter("C:\\Users\\User\\Documents\\2курс1сем\\лабыООП\\lab14\\lab14\\primes.txt"))
            {
                for (int i = 2; i <= n; i++)
                {
                    if (IsPrime(i))
                    {
                       
                        writer.WriteLine(i);
                        Console.WriteLine(i);
                    }
                }
            }
        }

        private static bool IsPrime(int number)
        {
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        private static void PauseThread()
        {
            isPaused = true;
            Console.WriteLine("Поток приостановлен.");
        }

        private static void ResumeThread()
        {
            isPaused = false;
            Console.WriteLine("Поток возобновлен.");
        }
    }


    public static class EvenOddThreads
    {
        private static object lockObject = new object(); 
        

        public static void RunThreads()
        {
            Console.Write("Введите n: ");
            int n = int.Parse(Console.ReadLine());
         
            Thread evenThread = new Thread(() => PrintEvenNumbers(n));
            Thread oddThread = new Thread(() => PrintOddNumbers(n));

            evenThread.Priority = ThreadPriority.Highest;

            oddThread.Priority = ThreadPriority.Lowest;

            oddThread.Start();
            oddThread.Join();
            evenThread.Start();
            evenThread.Join();


            //evenThread.Start();
            //oddThread.Start();

           
            //evenThread.Join();
            //oddThread.Join();
        }

        private static void PrintEvenNumbers(int n)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\User\\Documents\\2курс1сем\\лабыООП\\lab14\\lab14\\even.txt"))
            {
                for (int i = 2; i <= n; i += 2)
                {
                    lock (lockObject)
                    {
                        writer.WriteLine(i);
                        Console.WriteLine(i);
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        private static void PrintOddNumbers(int n)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\User\\Documents\\2курс1сем\\лабыООП\\lab14\\lab14\\odd.txt"))
            {
                for (int i = 1; i <= n; i += 2)
                {
                    lock (lockObject)
                    {
                        writer.WriteLine(i);
                        Console.WriteLine(i);
                    }
                    Thread.Sleep(100);
                }
            }
        }
    }



    public static class RepeatingTask
    {
        private static Timer timer;
        private static int countdownValue;
        public static void StartCountdown(int seconds)
        {
            countdownValue = seconds; 
            timer = new Timer(Countdown, null, 0, 1000);
           
            Console.WriteLine($"Обратный отсчет начался с {countdownValue} секунд. Нажмите Enter, чтобы остановить.");
            Console.ReadLine();
            timer.Dispose(); 
        }

        private static void Countdown(object state)
        {
            if (countdownValue > 0)
            {
                Console.WriteLine(countdownValue);
                countdownValue--;
            }
            else
            {
                Console.WriteLine("Обратный отсчет завершен!");
                timer.Dispose();
            }
        }
    }




    class Programm
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите вариант:");
            Console.WriteLine("1. Список всех запущенных процессов.");
            Console.WriteLine("2. Исследование доменов приложений и манипулирование ими.");
            Console.WriteLine("3. Вычисление простых чисел в отдельном потоке.");
            Console.WriteLine("4. Два потока для четных и нечетных чисел.");
            Console.WriteLine("5. Повторение задачи с использованием таймера.");
            Console.WriteLine("0. Выход.");

            while (true)
            {
                Console.WriteLine("Ваш выбор: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ProcessManager.ListProcesses();
                        break;
                    case "2":
                        AppDomainManager.ManipulateAppDomain();
                        break;
                    case "3":
                        PrimeCalculator.CalculateInThread();
                        break;
                    case "4":
                        EvenOddThreads.RunThreads();
                        break;
                    case "5":
                        {
                            Console.Write("Введите значение n: ");
                            string input = Console.ReadLine();

                            if (int.TryParse(input, out int num))
                            {
                                RepeatingTask.StartCountdown(num);
                            }
                            else
                            {
                                Console.WriteLine("Введите число!");
                            }
                            break;
                        }
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неправильный выбор!");
                        break;
                }
            }
        }
    }



}