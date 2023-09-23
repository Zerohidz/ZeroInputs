namespace ZeroInputs.Windows.Exceptions;

public sealed class CouldNotSetMousePositionException : Exception
{
    public CouldNotSetMousePositionException() : base("Couldn't set the mouse position from Windows API!")
    {
        
    }
}
