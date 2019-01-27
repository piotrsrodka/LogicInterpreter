namespace LogicalInterpreter
{
    public struct Token
    {
        public TokenType Type;
        public string Value;
        public int Index;

        public override string ToString()
        {
            return $"{Type.ToString()}: {Value}";
        }
    }
}
