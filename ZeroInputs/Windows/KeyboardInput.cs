using System.Runtime.InteropServices;

namespace ZeroInputs.Windows;
[StructLayout(LayoutKind.Explicit, Size = 40, Pack = 1)]
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