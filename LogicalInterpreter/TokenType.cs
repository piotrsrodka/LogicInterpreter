namespace LogicalInterpreter
{
    public enum TokenType
    {
        Undefined = 0,

        // Logical operations
        Or, And,

        // Comparisons
        Equal_equal, NotEqual,
        Less, Greater,
        Less_equal, Greater_equal,

        // Values
        Integer, String,

        // Keywords
        If,

        // And the rest
        Symbol,
        OpenBracket,
        CloseBracket,
        True,
        False,
        End,
    }
}
