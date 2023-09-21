namespace ZeroInputs;

internal interface IKeyStateReader
{
    public byte[] PreviousKeyStates { get; }
    public byte[] CurrentKeyStates { get; }

    public void Read();
}
