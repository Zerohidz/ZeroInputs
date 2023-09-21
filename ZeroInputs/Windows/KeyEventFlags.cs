namespace ZeroInputs.Windows;

[Flags]
internal enum KeyEventFlags : uint
{
    None = 0x0000,
    ExtendedKey = 0x0001,
    KeyUp = 0x0002,
    ScanCode = 0x0008,
    Unicode = 0x0004
}
