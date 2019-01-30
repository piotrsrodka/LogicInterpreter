using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicalInterpreter
{
    /*                Grammar:
     * 
     * logicalExpression : logic (( OR | AND) logic)* 
     *             logic : term ((operator) term) | (logic) | !logic
     *              term : integer | string | symbol
     *           integer : addend (( + | - ) addend) | (integer)
     */
    public class Interpreter
    {
        private Token currentToken;
        private int currentTokenIndex;
        private readonly Scanner scanner;
        private readonly Dictionary<string, string> symbolTable;
        private readonly HashSet<TokenType> comparisons = new HashSet<TokenType>
        {
            TokenType.Equal_equal,
            TokenType.Less,
            TokenType.Less_equal,
            TokenType.NotEqual,
            TokenType.Greater,
            TokenType.Greater_equal
        };

        public Interpreter(Scanner scanner, Dictionary<string, string>  symbolTable)
        {
            this.scanner = scanner;
            this.currentTokenIndex = 0;
            this.currentToken = scanner.Tokens[currentTokenIndex];
            this.symbolTable = symbolTable ?? new Dictionary<string, string>();
        }

        public bool LogicalExpression()
        {
            var result = Logic();

            var operators = new TokenType[] { TokenType.Or, TokenType.And };

            while (operators.Contains(currentToken.Type))
            {
                var token = currentToken;

                if (token.Type == TokenType.Or)
                {
                    Eat(TokenType.Or);
                    result = result | Logic(); // single | evaluates both sides always
                }

                if (token.Type == TokenType.And)
                {
                    Eat(TokenType.And);
                    result = result & Logic(); // single & evaluates both sides always
                }
            }

            return result;
        }

        private bool Logic()
        {
            var token = currentToken;

            if (token.Type == TokenType.OpenBracket)
            {
                Eat(TokenType.OpenBracket);
                var exp = LogicalExpression();
                Eat(TokenType.CloseBracket);
                return exp;
            }

            if (token.Type == TokenType.Not)
            {
                Eat(TokenType.Not);
                var exp = LogicalExpression();
                return !exp;
            }

            if (token.Type == TokenType.True)
            {
                Eat(TokenType.True);
                return true;
            }

            if (token.Type == TokenType.False)
            {
                Eat(TokenType.False);
                return false;
            }

            var leftTerm = Term();

            while (comparisons.Contains(currentToken.Type))
            {
                token = currentToken;
                Eat(currentToken.Type);

                switch (token.Type)
                {
                    case TokenType.Equal_equal:                        
                        return leftTerm == Term(); // string operator
                    case TokenType.Less:
                        return int.Parse(leftTerm) < int.Parse(Term());
                    case TokenType.Less_equal:
                        return int.Parse(leftTerm) <= int.Parse(Term());
                    case TokenType.Greater:
                        return int.Parse(leftTerm) > int.Parse(Term());
                    case TokenType.Greater_equal:
                        return int.Parse(leftTerm) >= int.Parse(Term());
                    case TokenType.NotEqual:
                        int left;
                        bool isLeftANumber = int.TryParse(leftTerm, out left);
                        var rightTerm = Term();
                        int right;
                        bool isRightANumber = int.TryParse(rightTerm, out right);

                        if (isLeftANumber && isRightANumber)
                        {
                            return left != right;
                        }
                        else
                        {
                            return leftTerm != rightTerm;
                        }
                    default:
                        throw new Exception("Syntax error");
                }
            }

            return bool.Parse(leftTerm);
        }

        private string Term()
        {
            var token = currentToken;

            if (token.Type == TokenType.Integer)
            {
                Eat(TokenType.Integer);
                return token.Value;
            }
            else if (token.Type == TokenType.String)
            {
                Eat(TokenType.String);
                return token.Value;
            }
            else if (token.Type == TokenType.Symbol)
            {
                Eat(TokenType.Symbol);
                string value;
                var isInSymbolTable = symbolTable.TryGetValue(token.Value, out value);

                if (isInSymbolTable)
                {
                    return value;
                }
                else
                {
                    throw new Exception($"There is no value for symbol: {token.Value}");
                }
            }
            else
            {
                throw new Exception("Syntax error");
            }
        }

        private void Eat(TokenType tokenName)
        {
            if (currentToken.Type == tokenName)
            {
                currentToken = scanner.Tokens[++currentTokenIndex];
            }
            else
            {
                throw new Exception("Invalid syntax");
            }
        }
    }
}