namespace ZeroInputs.Windows;

[Flags]
internal enum MouseEventFlags : uint
{
    LeftDown = 0x00000002,
    LeftUp = 0x00000004,
    MiddleDown = 0x00000020,
    MiddleUp = 0x00000040,
    Move = 0x00000001,
    Absolute = 0x00008000,
    RightDown = 0x00000008,
    RightUp = 0x00000010,
    Wheel = 0x00000800,
    XDown = 0x00000100,
    XUp = 0x00000200,
    HWheel = 0x00001000
}
