using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Lab_15_OOP
{
    public class Program
    {
       
        public static void Main()
        {
            
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(); 

          
            CancellationToken tok = cancellationTokenSource.Token; 

            bool[] resheto = new bool[100001];
            for (int i = 0; i < 100001; i++) 
            {
                resheto[i] = true;
            }

            List<int> primeNumbers = new List<int>();

            void searchPrimeNum(int n) 
            {
                if (tok.IsCancellationRequested) 

                {
                    Console.WriteLine("Операция была прервана");
                    return;
                }
                for (int i = 2; i <= n; i++)
                {
                    if (resheto[i]) 
                    {
                        primeNumbers.Add(i);
                    }

                    for (int j = i * i; j <= n; j += i) 

                    {
                        resheto[j] = false;
                    }
                }
            }
            int n = int.Parse(Console.ReadLine()); 
            Task searchPrimeNumbers = new Task(() => searchPrimeNum(n)); 
        

            Console.WriteLine($"Идентификатор задачи: {searchPrimeNumbers.Id}");
            Console.WriteLine($"Статус задачи до начала выполнения: {searchPrimeNumbers.Status}");
            Stopwatch stopwatch = new Stopwatch(); 
            stopwatch.Start(); 
            searchPrimeNumbers.Start(); 
            Console.WriteLine($"Статус задачи во время выполнения: {searchPrimeNumbers.Status}"); 
            searchPrimeNumbers.Wait(); 
            stopwatch.Stop(); 
            Console.WriteLine($"Затраченное время: {stopwatch.Elapsed.TotalMilliseconds}"); 
            Console.WriteLine($"Статус задачи по завершению выполнения: {searchPrimeNumbers.Status}"); 

            foreach (int i in primeNumbers) 
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
            primeNumbers.Clear();
            ///////////////////////////////////////////////////////////////////////////////



            Task task2 = new Task(() => searchPrimeNum(n), tok); 
            task2.Start(); 

            cancellationTokenSource.Cancel(); 
            Thread.Sleep(200); 
            Console.WriteLine($"Статус задачи task2: {task2.Status}");
            primeNumbers.Clear(); 

            /////////////////////////////////////////////////////////////////////////////


            int sum(int a, int b)
            {
                return a + b;
            }

            int mul(int a, int b)
            {
                return a * b;
            }

            int sub(int a, int b)
            {
                return a - b;
            }

            
            Task<int> t1 = Task.Run(() => sum(1, 2));
            Task<int> t2 = Task.Run(() => mul(1, 2));
            Task<int> t3 = Task.Run(() => sub(1, 2));
            
            Task t4 = Task.WhenAll(t1, t2, t3).ContinueWith(t =>  
            {
                Console.WriteLine($"Сумма, произведение и разность чисел 1 и 2 соответственно равны {t1.Result}, {t2.Result}, {t3.Result}");
            });
            t4.Wait(); 


            Task<int> t5 = Task.Run(() => sum(1, 2));
            Task<int> t6 = Task.Run(() => mul(1, 2));
            Task<int> t7 = Task.Run(() => sub(1, 2));

            var awaiter1 = t5.GetAwaiter();
            awaiter1.OnCompleted(() => 
            {
                
                Console.WriteLine($"Результат выполнения первой задачи {awaiter1.GetResult()}");
            });

            Task.WaitAll(t5, t6, t7); 

            Console.WriteLine($"Сумма, произведение и разность чисел 1 и 2 соответственно равны {t5.Result}, {t6.Result}, {t7.Result}");


            ///////////////////////////////////////////////////////////////////////////////////
            int[] arr1 = new int[1000000]; 
            int[] arr2 = new int[1000000];
            void createMas(int i)
            {
                Random rand = new Random();
                arr1[i] = rand.Next(0, 1000);
            }
            void createMas2(int i)
            {
                Random rand = new Random();
                arr2[i] = rand.Next(0, 1000);
            }
            Stopwatch timer = new Stopwatch(); 
            timer.Start();
            Parallel.For(0, 1000000, createMas);
            timer.Stop();
            Console.WriteLine($"Время выполнения Parallel.For:  {timer.Elapsed.TotalMilliseconds}");

            Stopwatch timer2 = new Stopwatch();
            timer2.Start();
            for (int i = 0; i < 1000000; i++) 
            {
                createMas2(i);
            }
            timer2.Stop();
            Console.WriteLine($"Время выполнения For:  {timer2.Elapsed.TotalMilliseconds}");

            void createMas3(int i)
            {
                i += 5;
            }

            Stopwatch timer3 = new Stopwatch();
            timer3.Start();
            Parallel.ForEach(arr1, createMas3); 
            timer3.Stop();
            Console.WriteLine($"Время выполнения Parallel.Foreach:  {timer3.Elapsed.TotalMilliseconds}");


            Stopwatch timer4 = new Stopwatch();
            timer4.Start();
            foreach (int i in arr2)
            {
                createMas3(i);
            }
            timer4.Stop();
            Console.WriteLine($"Время выполнения foreach:  {timer4.Elapsed.TotalMilliseconds}");

            /////////////////////////////////////////////////////////////////

            void method()
            {
                decimal sum = 0;
                for (int i = 0; i < 1000000; i++)
                {
                    sum += i; 
                }
            }
            
           
          
            void method2()
            {
                int mul = 1;
                for (int i = 1; i <= 50; i++)
                {
                    mul *= i;
                }
            }

            Stopwatch timer5 = new Stopwatch(); 
            timer5.Start();
            
            Parallel.Invoke(() => method(), () => method2()); 
            timer5.Stop();
            Console.WriteLine($"Время работы Parallel Invoke {timer5.Elapsed.TotalMilliseconds}");

            //////////////////////////////////////////////////////////////////////

             async static void quest_8() 
            {
                using var writer = new StreamWriter("C:\\Users\\User\\Documents\\2курс1сем\\лабыООП\\lab15\\output.txt"); 
                Console.WriteLine("Начало асинхронной записи в файл...");
                
                await writer.WriteLineAsync("Какая-то асинхронаая запись, которая не мешает выполнению основной программы!"); 
                Console.WriteLine("Запись в файл завершена");
            }

            quest_8();

            ////////////////////////////////






            BlockingCollection<string> sklad = new BlockingCollection<string>(5); 

            void postav(string prod, int time) 
            {
                while (true)
                {
                    sklad.Add(prod); 
                    Console.WriteLine($"Поставщик доставил {prod}");
                    skladState();
                    Thread.Sleep(time); 
                
                } 
            }

            void Client(int clientID) 
            {
                while (true)
                {
                    Random random = new Random(); 
                    Thread.Sleep(random.Next(0, 5000)); 
                    if (sklad.TryTake(out string prod)) 
                    {
                        Console.WriteLine($"Клиент с ID = {clientID} купил {prod}"); 
                        skladState();
                    }
                    else
                    {
                        Console.WriteLine($"Клиент ушел с пустыми руками");
                    }

                }
            }

            void skladState()
            {
                Console.WriteLine($"Текущее состояние склада: {string.Join(", ", sklad)}"); 

            }

            var postavschiki = new List<Task>() { 

                Task.Run(() => postav("Телевизор", 2500)), 
                Task.Run(() => postav("Компьютер", 3500)),
                Task.Run(() => postav("Планшет", 5000)),
                Task.Run(() => postav("Телефон", 1000)),
                Task.Run(() => postav("Ноутбук", 4500))
            };

            List<Task> clients = new List<Task>(); 

            for (int i = 0; i < 10; i++) 
            {
                clients.Add(Task.Run(() => Client(i)));
            }

            foreach (Task task in clients)
            {
                task.Wait(); 
            }

            //////////////////////////////////////////////////
            
        }
    }
}