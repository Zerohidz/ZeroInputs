namespace ZeroInputs;

public record Point(int X, int Y)
{
    public static Point Zero => new (0, 0);

    public static implicit operator Point(System.Drawing.Point sPoint) => new(sPoint.X, sPoint.Y);
    public static implicit operator System.Drawing.Point(Point point) => new(point.X, point.Y);

    public static implicit operator Point((int x, int y) tuple) => new(tuple.x, tuple.y);
    public static implicit operator (int x, int y)(Point point) => new(point.X, point.Y);
}