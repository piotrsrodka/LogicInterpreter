using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PScript
{
    public class Lexer
    {
        public List<Token> Tokens = new List<Token>();

        public const string Or = "or";
        public const string And = "and";
        public const string Comparison = "comparison";
        public const string Digit = "digit";
        public const string String = "string";
        public const string If = "if";
        public const string QuestionValue = "QuestionValue";
        public const string OpenBracket = "openBracket";
        public const string CloseBracket = "closeBracket";
        public const string True = "true";
        public const string False = "false";
        public const string End = "End";

        public Lexer(string input)
        {
            // Remove all whitespace
            input = Regex.Replace(input, @"\s+", "");
                        
            var tokenizer = new Dictionary<string, Regex>
            {
                { Or, new Regex(@"\|\|") },
                { And, new Regex(@"&&") },
                { Comparison,  new Regex(@"==|>=|<=|>|<") },
                { String,  new Regex("\"" + @"\w+" + "\"") },
                { Digit, new Regex(@"(?<!["+ "\"" + @"Q])\d+(?![" + "\"" + @"\]])\b") },
                { If,  new Regex("if") },
                { QuestionValue,  new Regex(@"\[Q\d+\]") },
                { OpenBracket,  new Regex(@"\(") },
                { CloseBracket,  new Regex(@"\)") },
                { True,  new Regex(@"true") },
                { False,  new Regex(@"false") },
            };

            foreach (var regex in tokenizer)
            {
                var matches = regex.Value.Matches(input);

                foreach (var m in matches)
                {
                    var match = m as Match;

                    Tokens.Add(new Token
                    {
                        Name = regex.Key,
                        Value = input.Substring(match.Index, match.Length),
                        Index = match.Index
                    });
                }
            }

            Tokens = Tokens.OrderBy(t => t.Index).ToList();
            Tokens.Add(new Token { Name = End, Value = null });
        }
    }
}
