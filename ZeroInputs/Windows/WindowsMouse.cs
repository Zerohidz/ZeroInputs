namespace ZeroInputs.Windows;

internal sealed class WindowsMouse : IMouse
{
    private const int WheelDelta = 120;

    private readonly WindowsInputStateProvider _stateProvider;
    private static readonly Dictionary<MouseButton, ushort> _buttonKeyCodes = new()
    {
        {MouseButton.Left, 0x01},
        {MouseButton.Middle, 0x04},
        {MouseButton.Right, 0x02},
    };
    private static readonly Dictionary<MouseButton, MouseEventFlags> _buttonDownFlags = new()
    {
        {MouseButton.Left, MouseEventFlags.LeftDown},
        {MouseButton.Middle, MouseEventFlags.MiddleDown},
        {MouseButton.Right, MouseEventFlags.RightDown},
    };
    private static readonly Dictionary<MouseButton, MouseEventFlags> _buttonUpFlags = new()
    {
        {MouseButton.Left, MouseEventFlags.LeftUp},
        {MouseButton.Middle, MouseEventFlags.MiddleUp},
        {MouseButton.Right, MouseEventFlags.RightUp},
    };

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
    public Point Position => WinApi.GetCursorPosition();

    public bool IsButtonDown(MouseButton button)
           => _stateProvider.IsKeyDown(_buttonKeyCodes[button]);

    public bool IsButtonUp(MouseButton button)
        => !IsButtonDown(button);

    public bool IsButtonPressed(MouseButton button)
           => _stateProvider.IsKeyPressed(_buttonKeyCodes[button]);

    public bool IsButtonReleased(MouseButton button)
           => _stateProvider.IsKeyReleased(_buttonKeyCodes[button]);
    #endregion

    #region MouseSimulation
    public void MoveTo(int x, int y)
        => WinApi.SetCursorPosition(x, y);

    public void MoveTo(Point point)
        => WinApi.SetCursorPosition(point.X, point.Y);

    public void PressButton(MouseButton button)
        => DoMouseEvent(_buttonDownFlags[button]);

    public void PressButton(MouseButton button, Point position)
        => DoMouseEvent(_buttonDownFlags[button], position);

    public void ReleaseButton(MouseButton button)
        => DoMouseEvent(_buttonUpFlags[button]);

    public void ReleaseButton(MouseButton button, Point position)
        => DoMouseEvent(_buttonUpFlags[button], position);

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
        int multipliedDistance = distance * WheelDelta;
        WinApi.MouseEvent((int)MouseEventFlags.Wheel, 0, 0, multipliedDistance, 0);
    }

    private void DoMouseEvent(MouseEventFlags flag)
    {
        WinApi.MouseEvent((int)flag, 0, 0, 0, 0);
    }

    private void DoMouseEvent(MouseEventFlags flag, Point position)
    {
        MoveTo(position);
        DoMouseEvent(flag);
    }
    #endregion
}
