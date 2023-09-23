using System.Runtime.InteropServices;

namespace ZeroInputs.Windows;

internal sealed class WindowsInputStateProvider : IInputStateProvider
{
    internal const int VirtualKeyCount = 256;
    private const string User32 = "user32.dll";

    private readonly byte[] _previousStates = new byte[VirtualKeyCount];
    private readonly byte[] _currentStates = new byte[VirtualKeyCount];

    // @alihakankurt TODO: Extract library imports to a seperate class and make it static to avoid multiple imports.
    #region LibraryImports
    [DllImport(User32)]
    private static extern bool GetKeyboardState(byte[] keys);

    [DllImport(User32)]
    private static extern short GetKeyState(int keyCode);
    #endregion

    #region EssentialMethods
    public void Update()
    {
        // This is somehow required to get the correct state of the keys.
        GetKeyState(0); 
        _currentStates.CopyTo(_previousStates, 0);
        GetKeyboardState(_currentStates);
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
