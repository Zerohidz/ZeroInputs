using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using ZeroInputs.Core;
using ZeroInputs.Core.DataContainers;
using ZeroInputs.Windows.Enums;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace ZeroInputs.Windows;

// TODO: onMouseWheelScroll
// https://github.com/michaelnoonan/inputsimulator/tree/master

public partial class InputApi : IInputApi
{
    private const string User32 = "user32.dll";
    private const int KBInputSize = 40;
    private const int VkCount = 256;
    private readonly byte[] _previousKeyStates = new byte[VkCount];
    private readonly byte[] _currentKeyStates = new byte[VkCount];

    #region LibraryImports
    [DllImport(User32)]
    public static extern short GetAsyncKeyState(int vk);

    [DllImport(User32)]
    private static extern bool GetKeyboardState(byte[] keys);

    [DllImport(User32, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    public static extern short GetKeyState(int keyCode);

    [DllImport(User32)]
    public static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

    [DllImport(User32)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport(User32, SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport(User32, SetLastError = true)]
    public static extern uint GetKeyboardLayout(uint idThread);

    [DllImport(User32)]
    private static extern uint SendInput(uint nInputs, KeyboardInput[] pInputs, int cbSize);

    [DllImport(User32, EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport(User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out Point lpMousePoint);

    [DllImport(User32)]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    [DllImport(User32)]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
    #endregion

    #region EssentialFunctions
    /// <summary>
    /// Updates the key states information, use this method inside a loop
    /// </summary>
    public void Update()
    {
        GetKeyState(0); // This is needed to activate GetKeyboardState()

        _currentKeyStates.CopyTo(_previousKeyStates, 0);
        GetKeyboardState(_currentKeyStates);
    }
    #endregion

    #region MouseInformation
    public Point GetMousePosition()
    {
        if (!GetCursorPos(out var currentMousePoint))
            throw new Exception("Couldn't get mouse point!");

        return currentMousePoint;
    }
    #endregion

    #region MouseSimulation
    public void SetMousePosition(int x, int y)
        => SetCursorPos(x, y);

    public void SetMousePosition(Point point)
        => SetCursorPos(point.X, point.Y);

    public void LeftUp()
    {
        DoMouseEvent(MouseEventFlags.LeftUp);
    }

    public void LeftDown()
    {
        DoMouseEvent(MouseEventFlags.LeftDown);
    }

    public void LeftClick()
    {
        LeftDown();
        LeftUp();
    }

    public void LeftClick(Point position)
    {
        SetMousePosition(position);
        LeftClick();
    }

    public void MiddleUp()
    {
        DoMouseEvent(MouseEventFlags.MiddleUp);
    }

    public void MiddleDown()
    {
        DoMouseEvent(MouseEventFlags.MiddleDown);
    }

    public void MiddleClick()
    {
        MiddleDown();
        MiddleUp();
    }

    public void MiddleClick(Point position)
    {
        SetMousePosition(position);
        MiddleClick();
    }

    public void RightUp()
    {
        DoMouseEvent(MouseEventFlags.RightUp);
    }

    public void RightDown()
    {
        DoMouseEvent(MouseEventFlags.RightDown);
    }

    public void RightClick()
    {
        RightDown();
        RightUp();
    }

    public void RightClick(Point position)
    {
        SetMousePosition(position);
        RightClick();
    }

    private void DoMouseEvent(MouseEventFlags flag)
    {
        Point position = GetMousePosition();

        mouse_event((int)flag, position.X, position.Y, 0, 0);
    }

    private void DoMouseEvent(MouseEventFlags flag, Point position)
    {
        SetMousePosition(position);
        mouse_event((int)flag, position.X, position.Y, 0, 0);
    }
    #endregion

    #region KeyInformation
    public bool IsKeyDown(char key)
    {
        short vKeyCode = CharToVirtualKeyCode(key);

        return (_currentKeyStates[vKeyCode] & 0x80) != 0;
    }

    public bool IsKeyDown(KeyCode keyCode)
    {
        return (_currentKeyStates[(short)keyCode] & 0x80) != 0;
    }

    public bool IsKeyUp(char key)
        => !IsKeyDown(key);

    public bool IsKeyUp(KeyCode keyCode)
        => !IsKeyDown(keyCode);

    public bool IsKeyPressed(char key)
    {
        short vKeyCode = CharToVirtualKeyCode(key);

        return (_currentKeyStates[vKeyCode] & 0x80) != 0 && (_previousKeyStates[vKeyCode] & 0x80) == 0;
    }

    public bool IsKeyPressed(KeyCode keyCode)
    {
        return (_currentKeyStates[(short)keyCode] & 0x80) != 0 && (_previousKeyStates[(short)keyCode] & 0x80) == 0;
    }

    public bool IsKeyReleased(char key)
    {
        short vKeyCode = CharToVirtualKeyCode(key);

        return (_currentKeyStates[vKeyCode] & 0x80) == 0 && (_previousKeyStates[vKeyCode] & 0x80) != 0;
    }
    public bool IsKeyReleased(KeyCode keyCode)
    {
        return (_currentKeyStates[(short)keyCode] & 0x80) == 0 && (_previousKeyStates[(short)keyCode] & 0x80) != 0;
    }

    public bool IsAnyKeyDown()
    {
        return IsAnyKeyFiltered(IsKeyDown);
    }

    public bool IsAnyKeyDown(out KeyCode[] keys)
    {
        return IsAnyKeyFiltered(IsKeyDown, out keys);
    }

    public bool IsAnyKeyPressed()
    {
        return IsAnyKeyFiltered(IsKeyPressed);
    }

    public bool IsAnyKeyPressed(out KeyCode[] keys)
    {
        return IsAnyKeyFiltered(IsKeyPressed, out keys);
    }

    public bool IsAnyKeyReleased()
    {
        return IsAnyKeyFiltered(IsKeyReleased);
    }

    public bool IsAnyKeyReleased(out KeyCode[] keys)
    {
        return IsAnyKeyFiltered(IsKeyReleased, out keys);
    }

    public bool IsCapsLockOn()
    {
        return (GetKeyState((int)KeyCode.CapsLock) & 0x0001) != 0;
    }

    public bool IsNumLockOn()
    {
        return (GetKeyState((int)KeyCode.NumLock) & 0x0001) != 0;
    }

    public bool IsScrollLockOn()
    {
        return (GetKeyState((int)KeyCode.ScrollLock) & 0x0001) != 0;
    }

    public bool IsControlDown()
    {
        return IsKeyDown(KeyCode.Control);
    }

    public bool IsShiftDown()
    {
        return IsKeyDown(KeyCode.Shift);
    }

    public bool IsAltDown()
    {
        return IsKeyDown(KeyCode.Alt);
    }

    private bool IsAnyKeyFiltered(Func<KeyCode, bool> filter)
    {
        for (short i = 0; i < VkCount; i++)
            if (filter.Invoke((KeyCode)i))
                return true;
        return false;
    }

    private bool IsAnyKeyFiltered(Func<KeyCode, bool> filter, out KeyCode[] keys)
    {
        bool result = false;
        List<KeyCode> keysList = new();
        for (short i = 0; i < VkCount; i++)
        {
            KeyCode keyCode = (KeyCode)i;
            if (filter.Invoke(keyCode))
            {
                keysList.Add(keyCode);
                result = true;
            }
        }
        keys = keysList.ToArray();
        return result;
    }

    private uint GetKeyboardLocaleId()
    {
        uint dwProcessId;
        IntPtr hWindowHandle = GetForegroundWindow();
        uint dwThreadId = GetWindowThreadProcessId(hWindowHandle, out dwProcessId);
        return GetKeyboardLayout(dwThreadId);
    }

    private short CharToVirtualKeyCode(char key)
    {
        uint keyboardLocaleId = GetKeyboardLocaleId();
        short vKeyCode = VkKeyScanEx(key, (nint)keyboardLocaleId);
        if ((vKeyCode < VkCount && -1 < vKeyCode) == false)
            throw new KeyCodeOfCharNotFoundException();

        return vKeyCode;
    }
    #endregion

    #region KeySimulation
    public void Type(string text)
    {
        KeyboardInput[] inputs = new KeyboardInput[text.Length * 2];
        for (int i = 0; i < text.Length * 2; i++)
            ConfigureInput(ref inputs[i], text[i / 2], keyUp: i % 2 != 0);

        SendInputs(inputs);
    }

    public void KeyPress(char key)
    {
        KeyboardInput[] inputs = new KeyboardInput[2];
        ConfigureInput(ref inputs[0], key);
        ConfigureInput(ref inputs[1], key, keyUp: true);

        SendInputs(inputs);
    }

    public void KeyPress(KeyCode key)
        => KeyPress((char)key);

    public void KeyUp(char key)
    {
        KeyboardInput[] inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key, keyUp: true);

        SendInputs(inputs);
    }

    public void KeyUp(KeyCode key)
        => KeyUp((char)key);

    public void KeyDown(char key)
    {
        KeyboardInput[] inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key);

        SendInputs(inputs);
    }

    public void KeyDown(KeyCode key)
        => KeyDown((char)key);

    public void Copy()
    {
        throw new NotImplementedException();
    }

    public void Copy(string text)
    {
        // TODO: çalışmıyor
        const uint CF_UNICODETEXT = 0xD;

        string nullTerminatedText = text + "\0";
        byte[] textBytes = Encoding.Unicode.GetBytes(nullTerminatedText);
        IntPtr hglobal = Marshal.AllocHGlobal(textBytes.Length);
        Marshal.Copy(textBytes, 0, hglobal, textBytes.Length);
        SetClipboardData(CF_UNICODETEXT, hglobal);
        Marshal.FreeHGlobal(hglobal);

        throw new NotImplementedException();

        //[DllImport(User32)]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);

        //[DllImport(User32, CharSet = CharSet.Auto)]
        //static public extern IntPtr GetForegroundWindow();

        //[DllImport(User32)]
        //static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        //.....

        //private void SendCtrlC(IntPtr hWnd)
        //    {
        //        uint KEYEVENTF_KEYUP = 2;
        //        byte VK_CONTROL = 0x11;
        //        SetForegroundWindow(hWnd);
        //        keybd_event(VK_CONTROL, 0, 0, 0);
        //        keybd_event(0x43, 0, 0, 0); //Send the C key (43 is "C")
        //        keybd_event(0x43, 0, KEYEVENTF_KEYUP, 0);
        //        keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);// 'Left Control Up

        //    }
    }

    public void Paste()
    {
        throw new NotImplementedException();
    }

    public void Paste(string text)
    {
        throw new NotImplementedException();
    }

    private static void SendInputs(KeyboardInput[] inputs)
    {
        if (SendInput((uint)inputs.Length, inputs, KBInputSize) == 0)
            throw new SendInputException();
    }

    private void ConfigureInput(ref KeyboardInput input, char key, bool keyUp = false)
    {
        if (Enum.IsDefined((KeyCode)key))
            ConfigureForVirtualKey(ref input, key, keyUp);
        else
            ConfigureForUnicode(ref input, key, keyUp);
    }

    private void ConfigureForUnicode(ref KeyboardInput input, char key, bool keyUp)
    {
        input = new KeyboardInput()
        {
            Type = 1,
            Vk = 0,
            Scan = key,
            Flags = (uint)KeyEventFlags.Unicode,
            Time = 0,
            ExtraInfo = IntPtr.Zero,
        };

        if (keyUp)
            input.Flags |= (uint)KeyEventFlags.KeyUp;

        if ((key & 0xFF00) == 0xE000) // Check the high byte
            input.Flags |= (uint)KeyEventFlags.ExtendedKey;
    }

    private void ConfigureForVirtualKey(ref KeyboardInput input, char key, bool keyUp)
    {
        input = new KeyboardInput()
        {
            Type = 1,
            Vk = key,
            Scan = 0,
            Flags = 0,
            Time = 0,
            ExtraInfo = IntPtr.Zero,
        };

        if (keyUp)
            input.Flags |= (uint)KeyEventFlags.KeyUp;

        if ((key & 0xFF00) == 0xE000) // Check the high byte
            input.Flags |= (uint)KeyEventFlags.ExtendedKey;
    }
    #endregion

    #region MouseTypes
    [Flags]
    public enum MouseEventFlags
    {
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        MiddleDown = 0x00000020,
        MiddleUp = 0x00000040,
        Move = 0x00000001,
        Absolute = 0x00008000,
        RightDown = 0x00000008,
        RightUp = 0x00000010
    }
    #endregion

    #region KeyboardTypes
    [StructLayout(LayoutKind.Explicit, Size = KBInputSize, Pack = 1)]
    private struct KeyboardInput
    {
        [FieldOffset(0)]
        public int Type;
        [FieldOffset(8)]
        public ushort Vk;
        [FieldOffset(10)]
        public ushort Scan;
        [FieldOffset(12)]
        public uint Flags;
        [FieldOffset(16)]
        public uint Time;
        [FieldOffset(20)]
        public IntPtr ExtraInfo;
    }
    #endregion

    #region Exceptions
    public class SendInputException : Exception { }
    public class KeyCodeOfCharNotFoundException : Exception { }
    #endregion
}