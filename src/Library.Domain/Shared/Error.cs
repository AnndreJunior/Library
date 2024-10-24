namespace Library.Domain.Shared;

public record Error(string Key, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
