using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogicalInterpreter
{
    public class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("symbolTable.json");
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

            Console.WriteLine(input + "\n");
            var scanner = new Scanner(input);

            if (scanner.IsError)
            {
                scanner.ErrorList.ForEach(e => Console.WriteLine(e));
                return;
            }

            scanner.Tokens.ForEach(t => Console.WriteLine(t.ToString()));
            var interpreter = new Interpreter(scanner, symbolTable);

            try
            {
                var logicalExpressionResult = interpreter.LogicalExpression();
                Console.WriteLine($"The result is: {logicalExpressionResult}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Syntax error: {exception.Message}");
            }
        }
    }
}
