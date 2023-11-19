namespace QuickShipParser
{
    public class Any : IPattern
    {
        readonly string accepted;

        public Any(string accepted)
        {
            this.accepted = accepted;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new FailedMatch(text);
            }

            return this.accepted.Contains(text[0]) ? new SuccessMatch(text[1..]) : new FailedMatch(text);
        }
    }
}
