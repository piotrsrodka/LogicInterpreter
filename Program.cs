using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;

namespace PScript
{
    public class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("answeredQuestions.json");
            var symbolTable = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var input = string.Empty;

            try
            {
                input = File.ReadAllText("logicalExpression.txt");
            }
            catch
            {
                Console.WriteLine("No 'input.ps' file in current directory");
            }

            if (args.Length > 0)
            {
                input = args[0];
            }
            Console.WriteLine("\n" + input + "\n");
            var lexer = new Lexer(input);
            lexer.Tokens.ForEach(t => Console.WriteLine($"{t.Index}\t{t.Value}\t\t{t.Name}"));
            var interpreter = new Interpreter(lexer, symbolTable);
            try
            {
                var logicalExpressionResult = interpreter.LogicalExpression();
                Console.WriteLine($"The result is: {logicalExpressionResult}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Syntax error: {exception.Message}");
            }
                
            //var expression = Expression.Or(Expression.Constant(true), Expression.Constant(false));
            //var result = Expression.Lambda<Func<bool>>(expression).Compile()();
            //Console.WriteLine($"EXpression: {expression.ToString()}");
            //Console.WriteLine($"Result: {result}");
        }

        public T ConvertTo<T>(object value)
        {
            if (value is T variable) return variable;

            //Handling Nullable types i.e, int?, double?, bool? .. etc
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
