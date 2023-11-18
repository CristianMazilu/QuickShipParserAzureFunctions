namespace QuickShipParser
{
    public interface IMatch
    {
        bool Success();

        string? RemainingText();
    }
}
