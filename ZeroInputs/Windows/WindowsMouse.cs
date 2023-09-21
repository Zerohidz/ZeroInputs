using System.Runtime.InteropServices;
using ZeroInputs.Windows.Exceptions;

namespace ZeroInputs.Windows;

internal sealed class WindowsMouse : IMouse
{
    private const int WheelDelta = 120;
    private const string User32 = "user32.dll";

    private readonly WindowsInputStateProvider _stateProvider;

    #region LibraryImports
    [DllImport(User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport(User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out Point lpMousePoint);

    [DllImport(User32)]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    #endregion

    #region EssentialMethods
    public WindowsMouse(WindowsInputStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    public void Update()
    {
        _stateProvider.Update();
    }
    #endregion

    #region MouseInformation
    public Point Position => GetCursorPos(out var currentMousePoint)
        ? currentMousePoint
        : throw new Exception("Couldn't get mouse point!");

    public bool IsButtonDown(MouseButton button)
    {
        throw new NotImplementedException();
    }

    public bool IsButtonUp(MouseButton button)
        => !IsButtonDown(button);

    public bool IsButtonPressed(MouseButton button)
    {
        throw new NotImplementedException();
    }

    public bool IsButtonReleased(MouseButton button)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region MouseSimulation
    public void MoveTo(int x, int y)
        => SetCursorPos(x, y);

    public void MoveTo(Point point)
        => SetCursorPos(point.X, point.Y);

    public void PressButton(MouseButton button)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftDown,
            MouseButton.Middle => MouseEventFlags.MiddleDown,
            MouseButton.Right => MouseEventFlags.RightDown,
            _ => throw new InvalidMouseButtonException(),
        });

    public void PressButton(MouseButton button, Point position)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftDown,
            MouseButton.Middle => MouseEventFlags.MiddleDown,
            MouseButton.Right => MouseEventFlags.RightDown,
            _ => throw new InvalidMouseButtonException(),
        }, position);

    public void ReleaseButton(MouseButton button)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftUp,
            MouseButton.Middle => MouseEventFlags.MiddleUp,
            MouseButton.Right => MouseEventFlags.RightUp,
            _ => throw new InvalidMouseButtonException(),
        });

    public void ReleaseButton(MouseButton button, Point position)
        => DoMouseEvent(button switch
        {
            MouseButton.Left => MouseEventFlags.LeftUp,
            MouseButton.Middle => MouseEventFlags.MiddleUp,
            MouseButton.Right => MouseEventFlags.RightUp,
            _ => throw new InvalidMouseButtonException(),
        }, position);

    public void ClickButton(MouseButton button)
    {
        PressButton(button);
        ReleaseButton(button);
    }

    public void ClickButton(MouseButton button, Point position)
    {
        PressButton(button, position);
        ReleaseButton(button, position);
    }

    public void Scroll(int distance)
    {
        int scrollAmount = distance * WheelDelta;
        throw new NotImplementedException();
    }

    private void DoMouseEvent(MouseEventFlags flag)
    {
        mouse_event((int)flag, 0, 0, 0, 0);
    }

    private void DoMouseEvent(MouseEventFlags flag, Point position)
    {
        MoveTo(position);
        DoMouseEvent(flag);
    }
    #endregion
}
