namespace QuickShipParser
{
    public class Value : IPattern
    {
        readonly IPattern pattern;
        readonly IPattern ws = new Many(new Any(" \n\r\t"));

        public Value()
        {
            var valuePattern =
                new Choice(
                        new JsonString(),
                        new JsonNumber(),
                        new JsonText("true"),
                        new JsonText("false"),
                        new JsonText("null"));

            var element = new Sequence(ws, valuePattern, ws);

            var elements = new ListPattern(element, new Character(','));

            IPattern array =
                new Choice(
                    new Sequence(new Character('['), ws, elements, new Character(']')));

            var member = new Sequence(ws, new JsonString(), ws, new Character(':'), element);

            var members = new ListPattern(member, new Character(','));

            IPattern obj =
                new Choice(
                    new Sequence(new Character('{'), ws, members, new Character('}')));

            valuePattern.Add(array);
            valuePattern.Add(obj);

            this.pattern = element;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
