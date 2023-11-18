namespace QuickShipParser
{
    public class FailedMatch : IMatch
    {
        private readonly string? _remainingText;

        public FailedMatch(string? text)
        {
            this._remainingText = text;
        }

        public bool Success()
        {
            return false;
        }

        public string? RemainingText()
        {
            return _remainingText;
        }
    }
}
