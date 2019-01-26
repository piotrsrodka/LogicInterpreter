using System;
using System.Collections.Generic;
using System.Linq;

namespace PScript
{
    /*                Grammar:
     * 
     * logicalExpression : logic (( OR | AND) logic)* 
     *             logic : term ((operator) term) | (logic)
     *              term : integer | string | QuestionValue
     */
    internal class Interpreter
    {
        private Token currentToken;
        private int currentTokenIndex;
        private readonly Lexer lexer;
        private readonly Dictionary<string, string> symbolTable;

        public Interpreter(Lexer lexer, Dictionary<string, string>  symbolTable)
        {
            this.lexer = lexer;
            this.currentTokenIndex = 0;
            this.currentToken = lexer.Tokens[currentTokenIndex];
            this.symbolTable = symbolTable;
        }

        public bool LogicalExpression()
        {
            var result = Logic();

            var operators = new string[] { Lexer.Or, Lexer.And };

            while (operators.Contains(currentToken.Name))
            {
                var token = currentToken;

                if (token.Name == Lexer.Or)
                {
                    Eat(Lexer.Or);
                    result = result | Logic(); // single | evaluates both sides always
                }

                if (token.Name == Lexer.And)
                {
                    Eat(Lexer.And);
                    result = result & Logic(); // single & evaluates both sides always
                }
            }

            return result;
        }

        private bool Logic()
        {
            var token = currentToken;

            if (token.Name == Lexer.OpenBracket)
            {
                Eat(Lexer.OpenBracket);
                var exp = LogicalExpression();
                Eat(Lexer.CloseBracket);
                return exp;
            }

            var leftTerm = Term();

            while (currentToken.Name == Lexer.Comparison)
            {
                token = currentToken;
                Eat(Lexer.Comparison);

                switch (token.Value)
                {
                    case "==":                        
                        return leftTerm == Term();
                    case "<":
                        return int.Parse(leftTerm) < int.Parse(Term());
                    case "<=":
                        return int.Parse(leftTerm) <= int.Parse(Term());
                    case ">":
                        return int.Parse(leftTerm) > int.Parse(Term());
                    case ">=":
                        return int.Parse(leftTerm) >= int.Parse(Term());
                    default:
                        throw new Exception("Syntax error");
                }
            }

            return bool.Parse(leftTerm);
        }

        private string Term()
        {
            var token = currentToken;

            if (token.Name == Lexer.Digit)
            {
                Eat(Lexer.Digit);
                return token.Value;
            }
            else if (token.Name == Lexer.String)
            {
                Eat(Lexer.String);
                return token.Value;
            }
            else if (token.Name == Lexer.QuestionValue)
            {
                Eat(Lexer.QuestionValue);
                string answer;
                var isInSymbolTable = symbolTable.TryGetValue(token.Value, out answer);

                if (isInSymbolTable)
                {
                    return answer;
                }
                else
                {
                    throw new Exception($"There is no known answer for question: {token.Value}");
                }
            }
            else
            {
                throw new Exception("Syntax error");
            }
        }

        private void Eat(string tokenName)
        {
            if (currentToken.Name == tokenName)
            {
                currentToken = lexer.Tokens[++currentTokenIndex];
            }
            else
            {
                throw new Exception("Invalid syntax");
            }
        }
    }
}