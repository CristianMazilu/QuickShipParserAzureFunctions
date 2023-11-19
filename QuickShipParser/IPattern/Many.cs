namespace QuickShipParser
{
    public class Many : IPattern
    {
        readonly IPattern pattern;

        public Many(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            IMatch match = pattern.Match(text);
            while (match.Success())
            {
                match = pattern.Match(match.RemainingText());
            }

            return new SuccessMatch(match.RemainingText());
        }
    }
}
