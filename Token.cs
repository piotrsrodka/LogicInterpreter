namespace PScript
{
    public struct Token
    {
        public string Name;
        public string Value;
        public int Index;

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}
