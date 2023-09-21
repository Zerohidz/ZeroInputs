namespace ZeroInputs.Windows.Exceptions;

public sealed class KeyCodeOfCharNotFoundException : Exception
{
    public KeyCodeOfCharNotFoundException() : base("Couldn't find the key code of the given char!")
    {
    }
}
