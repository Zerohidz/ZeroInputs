namespace ZeroInputs;

public record struct Point(int X, int Y)
{
    public static Point Zero => new (0, 0);

    public static implicit operator Point((int x, int y) tuple) => new(tuple.x, tuple.y);
    public static implicit operator (int x, int y)(Point point) => new(point.X, point.Y);
}