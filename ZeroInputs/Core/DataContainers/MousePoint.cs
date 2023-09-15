using System.Runtime.InteropServices;

namespace ZeroInputs.Core.DataContainers;

[StructLayout(LayoutKind.Sequential)]
public struct MousePoint
{
    public int X;
    public int Y;

    public MousePoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"[X:{X}, Y:{Y}]";
    }
}