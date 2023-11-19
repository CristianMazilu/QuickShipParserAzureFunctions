namespace QuickShipParser
{
    public class JsonText : IPattern
    {
        readonly string prefix;

        public JsonText(string prefix)
        {
            this.prefix = prefix;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < prefix.Length)
            {
                return new FailedMatch(text);
            }

            return text.StartsWith(prefix) ? new SuccessMatch(text.Substring(prefix.Length)) : new FailedMatch(text);
        }
    }
}
