using System.Runtime.InteropServices;

namespace ZeroInputs.Windows;
public class KeyStateReader
{
    public readonly byte[] PreviousKeyStates = new byte[256];
    public readonly byte[] CurrentKeyStates = new byte[256];

    #region LibraryImports
    [DllImport("user32.dll")]
    private static extern bool GetKeyboardState(byte[] keys);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    public static extern short GetKeyState(int keyCode);
    #endregion

    #region EssentialMethods
    /// <summary>
    /// Updates the key states information, use this method inside a loop
    /// </summary>
    public void Read()
    {
        GetKeyState(0); // This is needed to activate GetKeyboardState()

        CurrentKeyStates.CopyTo(PreviousKeyStates, 0);
        GetKeyboardState(CurrentKeyStates);
    }
    #endregion
}
