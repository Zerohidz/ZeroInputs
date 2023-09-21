using System.Runtime.InteropServices;
using ZeroInputs.Windows.Exceptions;

namespace ZeroInputs.Windows;

internal sealed class WindowsKeyboard : IKeyboard
{
    private const string User32 = "user32.dll";
    private const string IgnoredChars = "\r";

    private static readonly Dictionary<Key, ushort> _keyCodes = new()
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
    private readonly static Dictionary<ushort, Key> _keysOfKeyCodes = _keyCodes.ToDictionary(kv => kv.Value, kv => kv.Key);

    private readonly WindowsInputStateProvider _stateProvider;

    #region LibraryImports
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
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
    #endregion

    #region EssentialFunctions
    public WindowsKeyboard(WindowsInputStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
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
        => (_stateProvider.CurrentStates[_keyCodes[Key.CapsLock]] & 0x0001) != 0;

    public bool IsNumLockOn()
        => (_stateProvider.CurrentStates[_keyCodes[Key.NumLock]] & 0x0001) != 0;

    public bool IsScrollLockOn()
        => (_stateProvider.CurrentStates[_keyCodes[Key.ScrollLock]] & 0x0001) != 0;

    private bool IsKeyDown(ushort keyCode)
       => (_stateProvider.CurrentStates[keyCode] & 0x80) != 0;

    private bool IsKeyPressed(ushort keyCode)
        => (_stateProvider.CurrentStates[keyCode] & 0x80) != 0 && (_stateProvider.PreviousStates[keyCode] & 0x80) == 0;

    private bool IsKeyReleased(ushort keyCode)
        => (_stateProvider.CurrentStates[keyCode] & 0x80) == 0 && (_stateProvider.PreviousStates[keyCode] & 0x80) != 0;

    private bool IsAnyKeyFiltered(Func<ushort, bool> filter)
        => _keyCodes.Values.Any(filter);

    private bool IsAnyKeyFiltered(Func<ushort, bool> filter, out Key[] keys)
    {
        bool result = false;
        List<Key> keysList = new();
        for (ushort keyCode = 0; keyCode < 256; keyCode++)
        {
            if (filter.Invoke(keyCode) && _keysOfKeyCodes.TryGetValue(keyCode, out Key key))
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
        IntPtr hWindowHandle = GetForegroundWindow();
        uint dwThreadId = GetWindowThreadProcessId(hWindowHandle, out _);
        return GetKeyboardLayout(dwThreadId);
    }

    private ushort CharToVirtualKeyCode(char key)
    {
        if (char.IsUpper(key)) key = char.ToLower(key);

        uint keyboardLocaleId = GetKeyboardLocaleId();
        short vKeyCode = VkKeyScanEx(key, (nint)keyboardLocaleId);
        if ((vKeyCode < 256 && -1 < vKeyCode) == false)
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

    public void PressKey(char key)
    {
        if (IsIgnoredChar(key))
            return;

        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key);

        SendInputs(inputs);
    }

    public void PressKey(Key key)
    {
        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key);

        SendInputs(inputs);
    }

    public void ReleaseKey(char key)
    {
        if (IsIgnoredChar(key))
            return;

        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key, keyUp: true);

        SendInputs(inputs);
    }

    public void ReleaseKey(Key key)
    {
        var inputs = new KeyboardInput[1];
        ConfigureInput(ref inputs[0], key, keyUp: true);

        SendInputs(inputs);
    }

    public void SendKey(char key)
    {
        if (IsIgnoredChar(key))
            return;

        var inputs = new KeyboardInput[2];
        ConfigureInput(ref inputs[0], key);
        ConfigureInput(ref inputs[1], key, keyUp: true);

        SendInputs(inputs);
    }

    public void SendKey(Key key)
    {
        var inputs = new KeyboardInput[2];
        ConfigureInput(ref inputs[0], key);
        ConfigureInput(ref inputs[1], key, keyUp: true);

        SendInputs(inputs);
    }

    private static bool IsIgnoredChar(char key)
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
        if (key is '\n') key = (char)_keyCodes[Key.Enter];

        // If the given key is a lowercase character and its upper version is an ascii character
        // we can send it as a virtual key. This provides us the ability to trigger shortcuts (ex: Ctrl+S)
        bool isLowerAscii = char.IsLower(key) && char.IsAscii(char.ToUpper(key));
        if ((!char.IsLetter(key) || isLowerAscii) && _keysOfKeyCodes.ContainsKey(key))
        {
            if (isLowerAscii) key = char.ToUpper(key);
            ConfigureForVirtualKey(ref input, key, keyUp);
        }
        else
        {
            ConfigureForUnicode(ref input, key, keyUp);
        }
    }

    private static void ConfigureForUnicode(ref KeyboardInput input, char key, bool keyUp)
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

    private static void ConfigureForVirtualKey(ref KeyboardInput input, ushort key, bool keyUp)
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