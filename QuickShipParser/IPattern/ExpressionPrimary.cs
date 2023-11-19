namespace QuickShipParser
{
    public class ExpressionPrimary : IPattern
    {
        readonly IPattern pattern;

        public ExpressionPrimary()
        {
            var primaryPattern = new Choice();

            var factor = new Choice();
            factor.Add(new Sequence(new Any("+-"), factor));
            factor.Add(primaryPattern);

            var term = new ListPattern(factor, new Any("*/"));

            var expression = new ListPattern(term, new Any("+-"));

            primaryPattern.Add(
                new Sequence(
                        new Character('('),
                        expression,
                        new Character(')')));

            primaryPattern.Add(
                new JsonNumber());

            pattern = primaryPattern;
        }

        public IMatch Match(string text)
        {
            var result = pattern.Match(text);
            return result.RemainingText().Length == 0 ? result : new FailedMatch(result.RemainingText());
        }
    }
}
