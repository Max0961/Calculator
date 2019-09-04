using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OperationLibrary;

namespace Calculator
{
    class Calculator
    {
        public List<Operation> Operations { get; }

        public Calculator()
        {
            Operations = CreateOperations();
        }

        private List<Operation> CreateOperations()
        {
            var operationTypes = Assembly.GetAssembly(typeof(Operation))
                .GetTypes().Where(type => type.IsSubclassOf(typeof(Operation)));
            List<Operation> operations = new List<Operation>();
            foreach (var type in operationTypes)
            {
                operations.Add((Operation)Activator.CreateInstance(type));
            }
            operations.Sort();
            return operations;
        }

        public decimal Calculate(string[] input) 
        {
            Stack<string> stack = new Stack<string>();
            foreach (var symbol in input)
            {
                if (Char.IsDigit(symbol[0]))
                {
                    stack.Push(symbol);
                }
                else
                {
                    var operation = Operations.Where(y => y.Symbol.Equals(symbol)).First();
                    if (stack.Count < operation.Arity)
                    {
                        throw new InvalidOperationException
                            (String.Format("Недостаточно аргументов для операции \"{0}\"", operation.Symbol));
                    }
                    List<decimal> operands = new List<decimal>();
                    int counter = operation.Arity;
                    while (counter > 0)
                    {                       
                        operands.Insert(0, Decimal.Parse(stack.Pop()));
                        --counter;
                    }                   
                    stack.Push(operation.Execute(operands.ToArray()).ToString());
                }
            }
            if (stack.Count > 1)
            {
                throw new InvalidOperationException
                    (String.Format("Остались неиспользованные аргументы"));
            }
            if (stack.Count == 0)
            {
                throw new InvalidOperationException
                    (String.Format("Введено пустое выражение"));
            }
            return Decimal.Parse(stack.Pop());
        }
    }
}
