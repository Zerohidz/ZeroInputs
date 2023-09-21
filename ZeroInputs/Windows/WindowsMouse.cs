using System.Runtime.InteropServices;

namespace ZeroInputs.Windows;
internal class WindowsMouse : IMouse
{
    private const int WheelDelta = 120;
    private readonly KeyStateReader _stateReader;

    #region LibraryImports
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out Point lpMousePoint);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    #endregion

    #region EssentialMethods
    public WindowsMouse(KeyStateReader stateReader)
    {
        _stateReader = stateReader;
    }

    public void Update()
    {
        _stateReader.Read();
    }
    #endregion

    #region MouseInformation
    public Point GetMousePosition()
    {
        if (!GetCursorPos(out var currentMousePoint))
            throw new Exception("Couldn't get mouse point!");

        return currentMousePoint;
    }

    public bool IsMouseDown(MouseButton button)
    {
        throw new NotImplementedException();
    }

    public bool IsMouseUp(MouseButton button)
        => !IsMouseDown(button);

    public bool IsMousePressed(MouseButton button)
    {
        throw new NotImplementedException();
    }

    public bool IsMouseReleased(MouseButton button)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region MouseSimulation
    public void SetMousePosition(int x, int y)
        => SetCursorPos(x, y);

    public void SetMousePosition(Point point)
        => SetCursorPos(point.X, point.Y);

    public void MouseDown(MouseButton button)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftDown,
            MouseButton.Middle => MouseEventFlags.MiddleDown,
            MouseButton.Right => MouseEventFlags.RightDown,
            _ => throw new InvalidMouseButtonException(),
        });

    public void MouseDown(MouseButton button, Point position)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftDown,
            MouseButton.Middle => MouseEventFlags.MiddleDown,
            MouseButton.Right => MouseEventFlags.RightDown,
            _ => throw new InvalidMouseButtonException(),
        }, position);

    public void MouseUp(MouseButton button)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftUp,
            MouseButton.Middle => MouseEventFlags.MiddleUp,
            MouseButton.Right => MouseEventFlags.RightUp,
            _ => throw new InvalidMouseButtonException(),
        });

    public void MouseUp(MouseButton button, Point position)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftUp,
            MouseButton.Middle => MouseEventFlags.MiddleUp,
            MouseButton.Right => MouseEventFlags.RightUp,
            _ => throw new InvalidMouseButtonException(),
        }, position);

    public void Click(MouseButton button)
    {
        MouseDown(button);
        MouseUp(button);
    }

    public void Click(MouseButton button, Point position)
    {
        MouseDown(button, position);
        MouseUp(button, position);
    }

    public void ScrollMouseWheel(int wheelClickCount)
    {
        int scrollAmount = wheelClickCount * WheelDelta;
        throw new NotImplementedException();
    }

    private void DoMouseEvent(MouseEventFlags flag)
    {
        mouse_event((int)flag, 0, 0, 0, 0);
    }

    private void DoMouseEvent(MouseEventFlags flag, Point position)
    {
        SetMousePosition(position);
        DoMouseEvent(flag);
    }
    #endregion
}
