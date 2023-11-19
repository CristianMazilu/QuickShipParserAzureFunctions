namespace QuickShipParser
{
    public class Sequence : IPattern
    {
        readonly IPattern[] patterns;

        public Sequence(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            string updatedText = text;
            foreach (var pattern in patterns)
            {
                IMatch match = pattern.Match(updatedText);
                updatedText = match.RemainingText();
                if (!match.Success())
                {
                    return new FailedMatch(text);
                }
            }

            return new SuccessMatch(updatedText);
        }
    }
}
