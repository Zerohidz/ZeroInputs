using System.Runtime.InteropServices;
using ZeroInputs.Windows.Exceptions;

namespace ZeroInputs.Windows;

#pragma warning disable SYSLIB1054
internal static class WinApi
{
    internal const int VirtualKeyCount = 256;
    private const string User32 = "user32.dll";

    private static readonly nint _keyboardLayout;

    static WinApi()
    {
        GetKeyState(0);
        _keyboardLayout = GetKeyboardLayout();
    }

    #region KeyboardRelatedFunctions
    [DllImport(User32, EntryPoint = "GetKeyboardState")]
    internal static extern bool GetKeyboardState(byte[] keys);

    [DllImport(User32, EntryPoint = "GetKeyState")]
    internal static extern short GetKeyState(int keyCode);

    [DllImport(User32, EntryPoint = "SendInput")]
    private static extern uint SendInput(uint inputCount, KeyboardInput[] inputs, int inputSize);
    internal static bool SendInput(KeyboardInput[] inputs)
    {
        return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<KeyboardInput>()) == inputs.Length;
    }


    [DllImport(User32, EntryPoint = "SetClipboardData")]
    internal static extern IntPtr SetClipboardData(uint format, IntPtr hMem);

    [DllImport(User32, EntryPoint = "VkKeyScanEx")]
    private static extern short VkKeyScanEx(char ch, IntPtr hkl);
    internal static ushort ScanVirtualKey(char ch)
    {
        short virtualKeyCode = VkKeyScanEx(ch, _keyboardLayout);
        if ((virtualKeyCode & 0xFF) == -1)
            throw new KeyCodeOfCharNotFoundException();

        return (ushort)virtualKeyCode;
    }

    [DllImport(User32, EntryPoint = "GetKeyboardLayout")]
    private static extern nint GetKeyboardLayout(uint threadId);
    internal static nint GetKeyboardLayout()
    {
        return GetKeyboardLayout(0);
    }
    #endregion

    #region MouseRelatedFunctions
    [DllImport(User32, EntryPoint = "SetCursorPos")]
    private static extern int SetCursorPos(int x, int y);
    internal static void SetCursorPosition(int x, int y)
    {
        if (SetCursorPos(x, y) == 0)
            throw new CouldNotSetMousePositionException();
    }

    [DllImport(User32, EntryPoint = "GetCursorPos")]
    private static extern int GetCursorPos(out Point position);
    internal static Point GetCursorPosition()
    {
        if (GetCursorPos(out var position) == 0)
            throw new CouldNotGetMousePositionException();

        return position;
    }

    [DllImport(User32, EntryPoint = "mouse_event")]
    internal static extern void MouseEvent(int flags, int x, int y, int data, int extraInfo);
    #endregion
}
#pragma warning restore SYSLIB1054
