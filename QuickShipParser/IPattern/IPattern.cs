namespace QuickShipParser
{
    public interface IPattern
    {
        IMatch Match(string text);
    }
}
