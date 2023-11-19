namespace QuickShipParser
{
    public class JsonNumber : IPattern
    {
        readonly IPattern number;

        public JsonNumber()
        {
            IPattern digit = new Range('0', '9');
            IPattern digits = new OneOrMore(digit);

            IPattern integer =
                new Sequence(
                    new OptionalPattern(new Character('-')),
                    new Choice(new Character('0'), digits));

            IPattern fraction =
                new Sequence(
                    new Character('.'),
                    new OneOrMore(digit));

            IPattern exponent =
                new Sequence(
                    new Any("eE"),
                    new OptionalPattern(new Any("+-")),
                    new OneOrMore(digit));

            this.number = new Sequence(
                integer,
                new OptionalPattern(fraction),
                new OptionalPattern(exponent));
        }

        public IMatch Match(string text)
        {
            return number.Match(text);
        }
    }
}
