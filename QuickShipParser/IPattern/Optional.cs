namespace QuickShipParser
{
    public class OptionalPattern : IPattern
    {
        readonly IPattern pattern;

        public OptionalPattern(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            IMatch match = pattern.Match(text);
            return new SuccessMatch(match.RemainingText());
        }
    }
}
