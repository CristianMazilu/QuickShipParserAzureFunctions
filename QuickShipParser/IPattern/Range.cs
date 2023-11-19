namespace QuickShipParser
{
    public class Range : IPattern
    {
        readonly char start;
        readonly char end;

        public Range(char start, char end)
        {
            this.start = start;
            this.end = end;
        }

        public IMatch Match(string text)
        {
            return !string.IsNullOrEmpty(text) && text[0] >= this.start && text[0] <= this.end ?
                new SuccessMatch(text[1..]) :
                new FailedMatch(text);
        }
    }
}
