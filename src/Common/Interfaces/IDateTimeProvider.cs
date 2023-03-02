namespace Common.Interfaces;

public interface IDateTimeProvider
{
    DateTime       Now       { get; }
    DateTimeOffset NowOffset { get; }
}
