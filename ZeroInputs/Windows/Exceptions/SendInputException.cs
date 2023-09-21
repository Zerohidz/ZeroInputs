namespace ZeroInputs.Windows.Exceptions;

public sealed class SendInputException : Exception
{
    public SendInputException() : base("Couldn't send input!")
    {
    }
}
