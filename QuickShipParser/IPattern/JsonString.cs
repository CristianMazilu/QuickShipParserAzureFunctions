namespace QuickShipParser
{
    public class JsonString : IPattern
    {
        readonly IPattern str;

        public JsonString()
        {
            IPattern hex = new Choice(
                    new Range('0', '9'),
                    new Range('a', 'f'),
                    new Range('A', 'F'));

            IPattern hexSeq = new Sequence(
                new Character('u'),
                new Sequence(
                    hex,
                    hex,
                    hex,
                    hex));

            IPattern escape = new Choice(new Any("\\\"/bfnrt"), hexSeq);

            IPattern character = new Choice(
                    new Range(' ', '!'),
                    new Range('#', '['),
                    new Range(']', (char)0xFFFF),
                    new Sequence(new Character('\\'), escape));

            this.str = new Sequence(
                new Character('\"'),
                new Many(character),
                new Character('\"'));
        }

        public IMatch Match(string text)
        {
            return str.Match(text);
        }
    }
}