namespace QuickShipParser
{
    public class Character : IPattern
    {
        readonly char pattern;

        public Character(char pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            return !string.IsNullOrEmpty(text) && text[0] == pattern ? new SuccessMatch(text[1..]) : new FailedMatch(text);
        }
    }
}
