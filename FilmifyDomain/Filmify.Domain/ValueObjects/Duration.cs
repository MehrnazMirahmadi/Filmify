namespace Filmify.Domain.ValueObjects;

public class Duration
{
    public int Minutes { get; }
    public int Seconds { get; }

    public Duration(int minutes, int seconds)
    {
        if (minutes < 0 || seconds < 0 || seconds >= 60)
            throw new ArgumentException("Invalid duration");

        Minutes = minutes;
        Seconds = seconds;
    }

    public int ToTotalSeconds() => Minutes * 60 + Seconds;
}
