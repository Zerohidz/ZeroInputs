using System.Runtime.InteropServices;
using System.Text;
using ZeroInputs.Core;
using ZeroInputs.Core.DataContainers;
using ZeroInputs.Windows.Enums;

namespace ZeroInputs.Windows;


public partial class InputApi : IInputApi
{
    private const string User32 = "user32.dll";
    private const int KBInputSize = 40; // This is fixed, don't change it
    private const int VkCount = 256;
    private readonly Dictionary<short, bool> _previousKeyStates = new();
    private readonly Dictionary<short, bool> _currentKeyStates = new();

    #region LibraryImports
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    public static extern short GetKeyState(int keyCode);

    [DllImport("user32.dll")]
    public static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true)]
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

    [DllImport("user32.dll")]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
    #endregion

    #region EssentialFunctions
    public InputApi()
    {
        for (short vKey = 0; vKey < VkCount; vKey++)
        {
            _previousKeyStates[vKey] = false;
            _currentKeyStates[vKey] = false;
        }
    }

    /// <summary>
    /// Updates the key states information, use this method inside a loop
    /// </summary>
    public void Update()
    {
        for (short vk = 0; vk < VkCount; vk++)
        {
            _previousKeyStates[vk] = _currentKeyStates[vk];
            _currentKeyStates[vk] = (GetAsyncKeyState(vk) & 0x8000) != 0;
        }
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

        return _currentKeyStates[vKeyCode];
    }

    public bool IsKeyDown(KeyCode keyCode)
    {
        return _currentKeyStates[(short)keyCode];
    }

    public bool IsKeyUp(char key)
        => !IsKeyDown(key);

    public bool IsKeyUp(KeyCode keyCode)
        => !IsKeyDown(keyCode);

    public bool IsKeyJustBecameDown(char key)
    {
        short vKeyCode = CharToVirtualKeyCode(key);

        return _currentKeyStates[vKeyCode] && !_previousKeyStates[vKeyCode];
    }
    public bool IsKeyJustBecameDown(KeyCode keyCode)
    {
        return _currentKeyStates[(short)keyCode] && !_previousKeyStates[(short)keyCode];
    }

    public bool IsKeyJustBecameUp(char key)
    {
        short vKeyCode = CharToVirtualKeyCode(key);

        return !_currentKeyStates[vKeyCode] && _previousKeyStates[vKeyCode];
    }
    public bool IsKeyJustBecameUp(KeyCode keyCode)
    {
        return !_currentKeyStates[(short)keyCode] && _previousKeyStates[(short)keyCode];
    }

    public bool IsAnyKeyDown()
    {
        return AnyKeyCheck(IsKeyDown);
    }
    
    public bool IsAnyKeyDown(out KeyCode[] keys)
    {
        return AnyKeyCheck(IsKeyDown, out keys);
    }

    public bool IsAnyKeyJustBecameDown()
    {
        return AnyKeyCheck(IsKeyJustBecameDown);
    }

    public bool IsAnyKeyJustBecameDown(out KeyCode[] keys)
    {
        return AnyKeyCheck(IsKeyJustBecameDown, out keys);
    }

    public bool IsAnyKeyJustBecameUp()
    {
        return AnyKeyCheck(IsKeyJustBecameUp);
    }

    public bool IsAnyKeyJustBecameUp(out KeyCode[] keys)
    {
        return AnyKeyCheck(IsKeyJustBecameUp, out keys);
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

    public bool IsShiftDown()
    {
        return IsKeyDown(KeyCode.Shift);
    }

    public bool IsCtrlDown()
    {
        return IsKeyDown(KeyCode.Control);
    }

    private bool AnyKeyCheck(Func<KeyCode, bool> checker)
    {
        for (short i = 0; i < VkCount; i++)
            if (checker.Invoke((KeyCode)i))
                return true;
        return false;
    }

    private bool AnyKeyCheck(Func<KeyCode, bool> checker, out KeyCode[] keys)
    {
        bool result = false;
        List<KeyCode> keysList = new();
        for (short i = 0; i < VkCount; i++)
        {
            KeyCode keyCode = (KeyCode)i;
            if (checker.Invoke(keyCode))
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
        if ((vKeyCode < 256 && -1 < vKeyCode) == false)
            throw new KeyCodeOfCharNotFoundException();

        return vKeyCode;
    }
    #endregion

    #region KeySimulation
    public void Type(string text)
    {
        KeyboardInput[] inputs = new KeyboardInput[text.Length * 2];
        for (int i = 0; i < text.Length * 2; i++)
            ConfigureInput(ref inputs[i], text[i / 2], keyUp: i % 2 == 1);

        if (SendInput((uint)inputs.Length, inputs, KBInputSize) == 0)
            throw new SendKeysException();
    }

    public void KeyPress(char key)
    {
        // TODO: enter'ları basmıyor
        // TODO: keybd_event() diye bi şey de varmış
        KeyboardInput[] inputs = new KeyboardInput[2];
        ConfigureInput(ref inputs[0], key);
        ConfigureInput(ref inputs[1], key, keyUp: true);

        if (SendInput((uint)inputs.Length, inputs, KBInputSize) == 0)
            throw new SendKeysException();
    }

    public void KeyDown(char key)
    {
        KeyboardInput[] inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key);

        if (SendInput((uint)inputs.Length, inputs, KBInputSize) == 0)
            throw new SendKeysException();
    }

    public void KeyUp(char key)
    {
        KeyboardInput[] inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key, keyUp: true);

        if (SendInput((uint)inputs.Length, inputs, KBInputSize) == 0)
            throw new SendKeysException();
    }

    public void KeyPress(KeyCode key)
        => KeyPress((char)key);

    public void KeyDown(KeyCode key)
        => KeyDown((char)key);

    public void KeyUp(KeyCode key)
        => KeyUp((char)key);

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

        //[DllImport("User32.dll")]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //static public extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll")]
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

    private void ConfigureInput(ref KeyboardInput input, char key, bool keyUp = false)
    {
        if (key is '\n' or '\t')
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
            Flags = Convert.ToUInt32(keyUp ? 0x0004 | 0x0002 : 0x0004),
            Time = 0,
            ExtraInfo = IntPtr.Zero,
        };
    }

    private void ConfigureForVirtualKey(ref KeyboardInput input, char key, bool keyUp)
    {
        input = new KeyboardInput()
        {
            Type = 1,
            Vk = key,
            Scan = 0,
            Flags = Convert.ToUInt32(keyUp ? 0x0000 | 0x0002 : 0x0000),
            Time = 0,
            ExtraInfo = IntPtr.Zero,
        };
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
    internal struct KeyboardInput
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
    public class SendKeysException : Exception { }
    public class KeyCodeOfCharNotFoundException : Exception { }
    #endregion
}