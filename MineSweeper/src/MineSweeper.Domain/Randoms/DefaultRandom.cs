namespace MineSweeper.Domain.Randoms;

internal class DefaultRandom : IRandom
{
    private readonly Random _random;

    public DefaultRandom()
    {
        _random = new Random();
    }

    public int GetRandom(int min, int max)
    {
        return _random.Next(min, max);
    }
}
