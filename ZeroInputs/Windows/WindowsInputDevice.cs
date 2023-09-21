using System.Runtime.InteropServices;
using System.Text;

namespace ZeroInputs.Windows;

// TODO: onMouseWheelScroll
// https://github.com/michaelnoonan/inputsimulator/tree/master

// TODO: Point record
// TODO: IInputDevice tamamla

public partial class WindowsInputDevice : IInputDevice
{
    private const string User32 = "user32.dll";
    private const string IgnoredChars = "\r";
    private const int VkCount = 256;
    private readonly byte[] _previousKeyStates = new byte[VkCount];
    private readonly byte[] _currentKeyStates = new byte[VkCount];
    private readonly Dictionary<Key, ushort> _keyCodes = new()
    {
        {Key.LeftMouseButton, 0x01},
        {Key.RightMouseButton, 0x02},
        {Key.ControlBreak, 0x03},
        {Key.MiddleMouseButton, 0x04},
        {Key.X1MouseButton, 0x05},
        {Key.X2MouseButton, 0x06},
        {Key.Backspace, 0x08},
        {Key.Tab, 0x09},
        {Key.Clear, 0x0C},
        {Key.Enter, 0x0D},
        {Key.Shift, 0x10},
        {Key.Control, 0x11},
        {Key.Alt, 0x12},
        {Key.Pause, 0x13},
        {Key.CapsLock, 0x14},
        {Key.IMEKanaOrHangul, 0x15},
        {Key.IMEOn, 0x16},
        {Key.IMEJunja, 0x17},
        {Key.IMEFinal, 0x18},
        {Key.IMEHanjaOrKanji, 0x19},
        {Key.IMEOff, 0x1A},
        {Key.Escape, 0x1B},
        {Key.Space, 0x20},
        {Key.PageUp, 0x21},
        {Key.PageDown, 0x22},
        {Key.End, 0x23},
        {Key.Home, 0x24},
        {Key.LeftArrow, 0x25},
        {Key.UpArrow, 0x26},
        {Key.RightArrow, 0x27},
        {Key.DownArrow, 0x28},
        {Key.Select, 0x29},
        {Key.Print, 0x2A},
        {Key.Execute, 0x2B},
        {Key.PrintScreen, 0x2C},
        {Key.Insert, 0x2D},
        {Key.Delete, 0x2E},
        {Key.Help, 0x2F},
        {Key.Key0, 0x30},
        {Key.Key1, 0x31},
        {Key.Key2, 0x32},
        {Key.Key3, 0x33},
        {Key.Key4, 0x34},
        {Key.Key5, 0x35},
        {Key.Key6, 0x36},
        {Key.Key7, 0x37},
        {Key.Key8, 0x38},
        {Key.Key9, 0x39},
        {Key.A, 0x41},
        {Key.B, 0x42},
        {Key.C, 0x43},
        {Key.D, 0x44},
        {Key.E, 0x45},
        {Key.F, 0x46},
        {Key.G, 0x47},
        {Key.H, 0x48},
        {Key.I, 0x49},
        {Key.J, 0x4A},
        {Key.K, 0x4B},
        {Key.L, 0x4C},
        {Key.M, 0x4D},
        {Key.N, 0x4E},
        {Key.O, 0x4F},
        {Key.P, 0x50},
        {Key.Q, 0x51},
        {Key.R, 0x52},
        {Key.S, 0x53},
        {Key.T, 0x54},
        {Key.U, 0x55},
        {Key.V, 0x56},
        {Key.W, 0x57},
        {Key.X, 0x58},
        {Key.Y, 0x59},
        {Key.Z, 0x5A},
        {Key.LeftWindows, 0x5B},
        {Key.RightWindows, 0x5C},
        {Key.Apps, 0x5D},
        {Key.Sleep, 0x5F},
        {Key.Numpad0, 0x60},
        {Key.Numpad1, 0x61},
        {Key.Numpad2, 0x62},
        {Key.Numpad3, 0x63},
        {Key.Numpad4, 0x64},
        {Key.Numpad5, 0x65},
        {Key.Numpad6, 0x66},
        {Key.Numpad7, 0x67},
        {Key.Numpad8, 0x68},
        {Key.Numpad9, 0x69},
        {Key.Multiply, 0x6A},
        {Key.Add, 0x6B},
        {Key.Separator, 0x6C},
        {Key.Subtract, 0x6D},
        {Key.Decimal, 0x6E},
        {Key.Divide, 0x6F},
        {Key.F1, 0x70},
        {Key.F2, 0x71},
        {Key.F3, 0x72},
        {Key.F4, 0x73},
        {Key.F5, 0x74},
        {Key.F6, 0x75},
        {Key.F7, 0x76},
        {Key.F8, 0x77},
        {Key.F9, 0x78},
        {Key.F10, 0x79},
        {Key.F11, 0x7A},
        {Key.F12, 0x7B},
        {Key.F13, 0x7C},
        {Key.F14, 0x7D},
        {Key.F15, 0x7E},
        {Key.F16, 0x7F},
        {Key.F17, 0x80},
        {Key.F18, 0x81},
        {Key.F19, 0x82},
        {Key.F20, 0x83},
        {Key.F21, 0x84},
        {Key.F22, 0x85},
        {Key.F23, 0x86},
        {Key.F24, 0x87},
        {Key.NumLock, 0x90},
        {Key.ScrollLock, 0x91},
        {Key.LeftShift, 0xA0},
        {Key.RightShift, 0xA1},
        {Key.LeftControl, 0xA2},
        {Key.RightControl, 0xA3},
        {Key.LeftAlt, 0xA4},
        {Key.RightAlt, 0xA5},
        {Key.BrowserBack, 0xA6},
        {Key.BrowserForward, 0xA7},
        {Key.BrowserRefresh, 0xA8},
        {Key.BrowserStop, 0xA9},
        {Key.BrowserSearch, 0xAA},
        {Key.BrowserFavorites, 0xAB},
        {Key.BrowserHome, 0xAC},
        {Key.VolumeMute, 0xAD},
        {Key.VolumeDown, 0xAE},
        {Key.VolumeUp, 0xAF},
        {Key.MediaNextTrack, 0xB0},
        {Key.MediaPrevTrack, 0xB1},
        {Key.MediaStop, 0xB2},
        {Key.MediaPlayPause, 0xB3},
        {Key.LaunchMail, 0xB4},
        {Key.LaunchMediaSelect, 0xB5},
        {Key.LaunchApp1, 0xB6},
        {Key.LaunchApp2, 0xB7},
        {Key.OEMSemicolon, 0xBA},
        {Key.OEMPlus, 0xBB},
        {Key.OEMComma, 0xBC},
        {Key.OEMMinus, 0xBD},
        {Key.OEMPeriod, 0xBE},
        {Key.OEMQuestion, 0xBF},
        {Key.OEMTilde, 0xC0},
        {Key.OEMOpenBracket, 0xDB},
        {Key.OEMBackslash, 0xDC},
        {Key.OEMCloseBracket, 0xDD},
        {Key.OEMQuotes, 0xDE},
        {Key.OEMMisc, 0xDF},
        {Key.OEMPLUS102, 0xE2},
        {Key.IMEProcess, 0xE5},
        {Key.Packet, 0xE7},
        {Key.PA1, 0xFD},
        {Key.OEMClear, 0xFE},
    };
    private readonly Dictionary<ushort, Key> _keysOfKeyCodes;

    #region LibraryImports
    [DllImport(User32)]
    private static extern bool GetKeyboardState(byte[] keys);

    [DllImport(User32, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    public static extern short GetKeyState(int keyCode);

    [DllImport(User32)]
    public static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

    [DllImport(User32)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport(User32)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport(User32)]
    public static extern uint GetKeyboardLayout(uint idThread);

    [DllImport(User32)]
    private static extern uint SendInput(uint nInputs, KeyboardInput[] pInputs, int cbSize);

    [DllImport(User32)]
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
    public WindowsInputDevice()
    {
        _keysOfKeyCodes = _keyCodes.ToDictionary(kv => kv.Value, kv => kv.Key);
    }

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

    public void MouseUp(MouseButton button)
    {
        MouseEventFlags flag = button switch
        {
            MouseButton.Left => MouseEventFlags.LeftUp,
            MouseButton.Middle => MouseEventFlags.MiddleUp,
            MouseButton.Right => MouseEventFlags.RightUp,
            _ => throw new InvalidMouseButtonException(),
        };
        DoMouseEvent(flag);
    }

    public void MouseDown(MouseButton button)
    {
        MouseEventFlags flag = button switch
        {
            MouseButton.Left => MouseEventFlags.LeftDown,
            MouseButton.Middle => MouseEventFlags.MiddleDown,
            MouseButton.Right => MouseEventFlags.RightDown,
            _ => throw new InvalidMouseButtonException(),
        };
        DoMouseEvent(flag);
    }

    public void Click(MouseButton button)
    {
        MouseDown(button);
        MouseUp(button);
    }

    public void Click(MouseButton button, Point position)
    {
        SetMousePosition(position);
        Click(button);
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
        => IsKeyDown(CharToVirtualKeyCode(key));

    public bool IsKeyDown(Key key)
        => IsKeyDown(_keyCodes[key]);

    public bool IsKeyUp(char key)
        => !IsKeyDown(key);

    public bool IsKeyUp(Key key)
        => !IsKeyDown(key);

    public bool IsKeyPressed(char key)
        => IsKeyPressed(CharToVirtualKeyCode(key));

    public bool IsKeyPressed(Key key)
        => IsKeyPressed(_keyCodes[key]);

    public bool IsKeyReleased(char key)
        => IsKeyReleased(CharToVirtualKeyCode(key));

    public bool IsKeyReleased(Key key)
        => IsKeyReleased(_keyCodes[key]);

    public bool IsAnyKeyDown()
        => IsAnyKeyFiltered(IsKeyDown);

    public bool IsAnyKeyDown(out Key[] keys)
        => IsAnyKeyFiltered(IsKeyDown, out keys);

    public bool IsAnyKeyPressed()
        => IsAnyKeyFiltered(IsKeyPressed);

    public bool IsAnyKeyPressed(out Key[] keys)
        => IsAnyKeyFiltered(IsKeyPressed, out keys);

    public bool IsAnyKeyReleased()
        => IsAnyKeyFiltered(IsKeyReleased);

    public bool IsAnyKeyReleased(out Key[] keys)
        => IsAnyKeyFiltered(IsKeyReleased, out keys);

    public bool IsCapsLockOn()
        => (GetKeyState((int)Key.CapsLock) & 0x0001) != 0;

    public bool IsNumLockOn()
        => (GetKeyState((int)Key.NumLock) & 0x0001) != 0;

    public bool IsScrollLockOn()
        => (GetKeyState((int)Key.ScrollLock) & 0x0001) != 0;

    private bool IsKeyDown(ushort keyCode)
       => (_currentKeyStates[keyCode] & 0x80) != 0;
    private bool IsKeyPressed(ushort keyCode)
        => (_currentKeyStates[keyCode] & 0x80) != 0 && (_previousKeyStates[keyCode] & 0x80) == 0;

    private bool IsKeyReleased(ushort keyCode)
        => (_currentKeyStates[keyCode] & 0x80) == 0 && (_previousKeyStates[keyCode] & 0x80) != 0;

    private bool IsAnyKeyFiltered(Func<ushort, bool> filter)
    {
        for (ushort key = 0; key < VkCount; key++)
            if (filter.Invoke(key))
                return true;
        return false;
    }

    private bool IsAnyKeyFiltered(Func<ushort, bool> filter, out Key[] keys)
    {
        bool result = false;
        List<Key> keysList = new();
        for (ushort keyCode = 0; keyCode < VkCount; keyCode++)
        {
            if (filter.Invoke(keyCode) && _keysOfKeyCodes.TryGetValue(keyCode, out Key key)) // TODO: Eğer Key karşılığı yoksa IsAnyKeyFiltered ignoreluyor o key'i
            {
                keysList.Add(key);
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

    private ushort CharToVirtualKeyCode(char key)
    {
        if (char.IsUpper(key)) key = char.ToLower(key);

        uint keyboardLocaleId = GetKeyboardLocaleId();
        short vKeyCode = VkKeyScanEx(key, (nint)keyboardLocaleId);
        if ((vKeyCode < VkCount && -1 < vKeyCode) == false)
            throw new KeyCodeOfCharNotFoundException();

        return (ushort)vKeyCode;
    }
    #endregion

    #region KeySimulation
    public void Write(string text)
    {
        var inputs = new KeyboardInput[text.Length * 2];
        for (int i = 0; i < text.Length * 2; i++)
            ConfigureInput(ref inputs[i], text[i / 2], keyUp: i % 2 != 0);

        SendInputs(inputs);
    }

    public void KeyPress(char key)
    {
        if (IsIgnoredChar(key)) 
            return;

        var inputs = new KeyboardInput[2];
        ConfigureInput(ref inputs[0], key);
        ConfigureInput(ref inputs[1], key, keyUp: true);

        SendInputs(inputs);
    }

    public void KeyPress(Key key)
    {
        var inputs = new KeyboardInput[2];
        ConfigureInput(ref inputs[0], key);
        ConfigureInput(ref inputs[1], key, keyUp: true);

        SendInputs(inputs);
    }

    public void KeyUp(char key)
    {
        if (IsIgnoredChar(key))
            return;

        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key, keyUp: true);

        SendInputs(inputs);
    }

    public void KeyUp(Key key)
    {
        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key, keyUp: true);

        SendInputs(inputs);
    }

    public void KeyDown(char key)
    {
        if (IsIgnoredChar(key))
            return;

        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key);

        SendInputs(inputs);
    }

    public void KeyDown(Key key)
    {
        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key);

        SendInputs(inputs);
    }

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

    private bool IsIgnoredChar(char key)
        => IgnoredChars.Contains(key);

    private static void SendInputs(KeyboardInput[] inputs)
    {
        if (SendInput((uint)inputs.Length, inputs, 40) == 0)
            throw new SendInputException();
    }

    private void ConfigureInput(ref KeyboardInput input, Key key, bool keyUp = false)
        => ConfigureForVirtualKey(ref input, _keyCodes[key], keyUp);

    private void ConfigureInput(ref KeyboardInput input, char key, bool keyUp = false)
    {
        if (key is '\n') key = (char)Key.Enter;

        // This if statement is necessary to prevent wrong keycoding for uppercase letters or nonletter buttons
        // If we don't send upper character inputs as virtual keycodes, the shortcuts bound to them don't work (ex: Ctrl+S)
        if ((!char.IsLetter(key) || char.IsUpper(key)) && Enum.IsDefined((Key)key))
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

    private void ConfigureForVirtualKey(ref KeyboardInput input, ushort key, bool keyUp)
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
}