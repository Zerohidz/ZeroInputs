namespace ZeroInputs.Windows.Exceptions;

public sealed class CouldNotGetMousePositionException : Exception
{
    public CouldNotGetMousePositionException() : base("Couldn't get the mouse position from Windows API!")
    {
        
    }
}
