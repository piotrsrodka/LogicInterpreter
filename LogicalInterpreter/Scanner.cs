using System.Collections.Generic;

namespace LogicalInterpreter
{
    public partial class Scanner
    {
        public List<Token> Tokens = new List<Token>();

        public bool IsError = false;
        public List<string> ErrorList = new List<string>();

        public HashSet<string> Keywords = new HashSet<string>
        {
            "if"
        };

        private int start = 0;
        private int current = 0;
        private string input;

        public Scanner(string source)
        {
            input = source;

            while (current < input.Length)
            {
                start = current;
                ScanToken(input);
            }

            AddToken(TokenType.End);
        }

        private void ScanToken(string input)
        {
            char c = input[current];
            current++;

            switch (c)
            {
                case '(':
                    AddToken(TokenType.OpenBracket);
                    break;
                case ')':
                    AddToken(TokenType.CloseBracket);
                    break;
                case '+':
                    AddToken(TokenType.Plus);
                    break;
                case '-':
                    AddToken(TokenType.Minus);
                    break;
                case '*':
                    AddToken(TokenType.Times);
                    break;
                case '/':
                    AddToken(TokenType.Division);
                    break;
                case '=':
                    AddToken(IsNext('=') ? TokenType.Equal_equal : TokenType.Undefined);
                    break;
                case '<':
                    AddToken(IsNext('=') ? TokenType.Less_equal : TokenType.Less);
                    break;
                case '>':
                    AddToken(IsNext('=') ? TokenType.Greater_equal: TokenType.Greater);
                    break;
                case '!':
                    AddToken(IsNext('=') ? TokenType.NotEqual : TokenType.Undefined);
                    break;
                case '|':
                    AddToken(IsNext('|') ? TokenType.Or : TokenType.Or);
                    break;
                case '&':
                    AddToken(IsNext('&') ? TokenType.And : TokenType.And);
                    break;
                case '"':
                    HandleStringLiteral();
                    break;
                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    // Ignore whitespace.  
                    break;
                default:
                    if (char.IsDigit(c))
                    {
                        HandleNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        HandleSymbol();
                    }
                    else
                    {
                        Error($"Unknown character at position {current - 1}");
                    }
                    break;
            }
        }

        private void HandleSymbol()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            var text = input.Substring(start, current - start);

            if (Keywords.Contains(text))
            {
                AddToken(TokenType.If);
            }
            else
            {
                AddToken(TokenType.Symbol, text);
            }
        }

        private void HandleNumber()
        {
            while (char.IsDigit(Peek())) Advance();

            AddToken(TokenType.Integer, input.Substring(start, current - start));
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || char.IsDigit(c);
        }

        private void HandleStringLiteral()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                Advance();
            }

            if (IsAtEnd())
            {
                Error($"Unterminated string.");
                return;
            }

            // The closing '"'                                      
            Advance();

            // Trim the surrounding quotes.                         
            var value = input.Substring(start + 1, current - start - 2);
            AddToken(TokenType.String, value);
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return input[current];
        }

        private char Advance()
        {
            current++;
            return input[current - 1];
        }

        private bool IsNext(char expected)
        {
            if (IsAtEnd()) return false;
            if (input[current] != expected) return false;
            current++;
            return true;
        }

        private bool IsAtEnd()
        {
            return current >= input.Length;
        }

        private void AddToken(TokenType type)
        {
            Tokens.Add(new Token { Type = type, Value = null });
        }

        private void AddToken(TokenType type, string value)
        {
            Tokens.Add(new Token { Type = type, Value = value });
        }

        private void Error(string message)
        {
            IsError = true;
            ErrorList.Add(message);
        }
    }
}
