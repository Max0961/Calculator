using System;
using System.Threading;

namespace Calculator
{
    class Program
    {
        static void SolveExpression(Calculator calculator)
        {
            Console.Clear();
            Console.WriteLine("Введите выражение (можно без пробелов)");

            string[] symbols;
            try
            {
                symbols = Convertation.ToPostfixNotation(calculator, Console.ReadLine());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                AwaitEscaping();
                return;
            }           

            try
            {
                Console.WriteLine("Ответ: {0}", calculator.Calculate(symbols));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            AwaitEscaping();
        }

        static void ShowOperations(Calculator calculator)
        {
            Console.Clear();
            Console.WriteLine("Список операций");
            foreach (var operation in calculator.Operations)
            {
                Console.WriteLine(operation);
            }
            AwaitEscaping();
        }

        static void AwaitEscaping()
        {
            Console.WriteLine("Вернуться - любая клавиша");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Выберите действие клавишей");
                Console.WriteLine("1 - решить выражение");
                Console.WriteLine("2 - показать операции");
                Console.WriteLine("Любая другая - выход");
           
                var key = Console.ReadKey();               

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        SolveExpression(calculator);
                        break;
                    case ConsoleKey.D2:
                        ShowOperations(calculator);
                        break;
                    default:
                        exit = true;
                        break;
                }
            }
            Console.Clear();
            Console.WriteLine("Выход");
            Thread.Sleep(1000);
        }
    }
}
