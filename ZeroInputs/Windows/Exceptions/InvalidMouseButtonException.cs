namespace ZeroInputs.Windows.Exceptions;

public sealed class InvalidMouseButtonException : Exception
{
    public InvalidMouseButtonException() : base("Unrecognized mouse button!")
    {
    }
}
