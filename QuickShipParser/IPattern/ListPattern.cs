namespace QuickShipParser
{
    public class ListPattern : IPattern
    {
        readonly IPattern pattern;

        public ListPattern(IPattern element, IPattern separator)
        {
            this.pattern = new OptionalPattern(new Sequence(element, new Many(new Sequence(separator, element))));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
