using System;
using System.Collections.Generic;
using System.Linq;
using OperationLibrary;

namespace Calculator
{
    static partial class Convertation
    {
        public static IEnumerable<Symbol> SeparateSymbols(Calculator calculator, string input)
        {
            var operationNames = calculator.Operations.Select(x => x.Symbol).ToArray();

            input = input.Replace(" ", string.Empty);
            int position = 0;
            string symbol = String.Empty;
            while (position < input.Length)
            {
                if (input[position] == '(')
                {
                    CheckUnknownOperation(symbol);
                    symbol += input[position++];
                    yield return new Symbol(symbol, Type.OpeningParenthese);
                    symbol = String.Empty;
                }
                if (input[position] == ')')
                {
                    CheckUnknownOperation(symbol);
                    symbol += input[position++];
                    yield return new Symbol(symbol, Type.ClosingParenthese);
                    symbol = String.Empty;
                }
                else if (Char.IsDigit(input[position]))
                {
                    CheckUnknownOperation(symbol);
                    while (Char.IsDigit(input[position]) || input[position] == ',')
                    {
                        symbol += input[position++];
                        if (position == input.Length) break;
                    }
                    ValidateNumber(symbol);
                    yield return new Symbol(symbol, Type.Number);
                    symbol = String.Empty;
                }
                else
                {
                    symbol += input[position++];
                    if (operationNames.Contains(symbol))
                    {
                        yield return new Symbol(symbol, Type.Operation);
                        symbol = symbol.Replace(symbol, String.Empty);                       
                    }
                }
            }
            CheckUnknownOperation(symbol);
        }

        static void ValidateNumber(string symbol)
        {
            if (!Double.TryParse(symbol, out double tmp))
            {
                throw new InvalidOperationException
                   (String.Format("Невозможно прочитать число \"{0}\"", symbol)); ;
            }
        }

        static void CheckUnknownOperation(string symbol)
        {
            if (symbol.Length > 0)
            {
                throw new InvalidOperationException
                    (String.Format("Встречена неизвестная операция \"{0}\"", symbol));
            }
        }

        static public string[] ToPostfixNotation(Calculator calculator, string input)
        {
            List<Symbol> symbols = new List<Symbol>();
            Stack<Symbol> stack = new Stack<Symbol>();
            foreach (var symbol in SeparateSymbols(calculator, input))
            {
                if (symbol.Type == Type.Number) symbols.Add(symbol);
                else if (symbol.Type == Type.Operation && 
                    symbol.GetOperation(calculator).Type == OperationLibrary.Type.Postfix) symbols.Add(symbol);
                else if (symbol.Type == Type.Operation &&
                    symbol.GetOperation(calculator).Type == OperationLibrary.Type.Prefix) stack.Push(symbol);
                else if (symbol.Type == Type.OpeningParenthese) stack.Push(symbol);
                else if (symbol.Type == Type.ClosingParenthese)
                {
                    while (stack.Peek().Type != Type.OpeningParenthese)
                    {
                        if (stack.Count > 1) symbols.Add(stack.Pop());
                        else throw new InvalidOperationException("В выражении не согласованы скобки");
                    }
                    stack.Pop();
                }
                else if (symbol.Type == Type.Operation && symbol.GetOperation(calculator).Arity == 2)
                {
                    if (stack.Count > 0)
                    {
                        while (stack.Count > 0 && (stack.Peek().Type == Type.Operation
                            && (
                            (stack.Peek().GetOperation(calculator).Type == OperationLibrary.Type.Prefix) ||
                            (stack.Peek().GetOperation(calculator).Priority > symbol.GetOperation(calculator).Priority) ||
                            (stack.Peek().GetOperation(calculator).Priority == symbol.GetOperation(calculator).Priority
                            && stack.Peek().GetOperation(calculator).LeftAssociativity)))
                            )
                        {
                            symbols.Add(stack.Pop());
                        }
                    }
                    stack.Push(symbol);
                }
            }
            while (stack.Count > 0)
            {
                if (stack.Peek().Type != Type.OpeningParenthese
                    && stack.Peek().Type != Type.ClosingParenthese) symbols.Add(stack.Pop());
                else throw new InvalidOperationException("В выражении не согласованы скобки");
            }
            return symbols.Select(x => x.Name).ToArray();
        }

        internal partial class Symbol
        {
            public string Name { get; set; }

            public Type Type { get; set; }

            public Operation GetOperation(Calculator calculator)
            {
                return calculator.Operations.Find(x => x.Symbol == this.Name);
            }

            public Symbol(string name, Type type)
            {
                Name = name;
                Type = type;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
