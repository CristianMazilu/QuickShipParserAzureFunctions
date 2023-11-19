namespace QuickShipParser
{
    internal class SuccessMatch : IMatch
    {
        private readonly string _text;

        public SuccessMatch(string text)
        {
            _text = text;
        }

        public bool Success()
        {
            return true;
        }

        public string RemainingText()
        {
            return _text;
        }
    }
}
