using System.Linq;

namespace QuickShipParser
{
    public class ModelParser : IPattern
    {
        private readonly ModelStructure _modelStructure;

        public ModelParser(ModelStructure modelStructure)
        {
            _modelStructure = modelStructure;
        }

        public IMatch Match(string? text)
        {
            int currentIndex = 0;

            if (_modelStructure.Elements != null)
                foreach (var element in _modelStructure.Elements)
                {
                    if (currentIndex + element.Length > text.Length)
                        return new MatchResult(false, text); // Text is too short for this element

                    string currentCode = text.Substring(currentIndex, element.Length);
                    currentIndex += element.Length;

                    if (element.Codes.All(c => c.Code != currentCode))
                        return new MatchResult(false, text.Substring(currentIndex)); // Invalid code found
                }

            return new MatchResult(currentIndex == text.Length, text.Substring(currentIndex));
        }

        private class MatchResult : IMatch
        {
            private readonly bool _success;
            private readonly string? _remainingText;

            public MatchResult(bool success, string? remainingText)
            {
                _success = success;
                _remainingText = remainingText;
            }

            public bool Success()
            {
                return _success;
            }

            public string? RemainingText()
            {
                return _remainingText;
            }
        }
    }
}