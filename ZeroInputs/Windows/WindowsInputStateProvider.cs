namespace ZeroInputs.Windows;

internal sealed class WindowsInputStateProvider : IInputStateProvider
{
    private readonly byte[] _previousStates = new byte[WinApi.VirtualKeyCount];
    private readonly byte[] _currentStates = new byte[WinApi.VirtualKeyCount];

    #region EssentialMethods
    public void Update()
    {
        _currentStates.CopyTo(_previousStates, 0);
        WinApi.GetKeyboardState(_currentStates);
    }
    #endregion

    #region KeyInformation
    public bool IsKeyDown(ushort keyCode)
       => (_currentStates[keyCode] & 0x80) != 0;

    public bool IsKeyPressed(ushort keyCode)
        => (_currentStates[keyCode] & 0x80) != 0 && (_previousStates[keyCode] & 0x80) == 0;

    public bool IsKeyReleased(ushort keyCode)
        => (_currentStates[keyCode] & 0x80) == 0 && (_previousStates[keyCode] & 0x80) != 0;

    public bool IsTogglableKeyOn(ushort keyCode)
        => (_currentStates[keyCode] & 0x0001) != 0;
    #endregion
}
