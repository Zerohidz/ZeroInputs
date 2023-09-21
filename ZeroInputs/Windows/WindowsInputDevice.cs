namespace ZeroInputs.Windows;
internal class WindowsInputDevice : IInputDevice
{
    private readonly KeyStateReader _stateReader;
    private readonly IKeyboard _keyboard;
    private readonly IMouse _mouse;

    public WindowsInputDevice(KeyStateReader stateReader)
    {
        _stateReader = stateReader;
        _keyboard = new WindowsKeyboard(stateReader);
        _mouse = new WindowsMouse(stateReader);
    }

    public void Update()
    {
        _stateReader.Read();
    }

    #region Keyboard
    public bool IsKeyDown(char key) => _keyboard.IsKeyDown(key);

    public bool IsKeyDown(Key key) => _keyboard.IsKeyDown(key);

    public bool IsKeyUp(char key) => _keyboard.IsKeyUp(key);

    public bool IsKeyUp(Key key) => _keyboard.IsKeyUp(key);

    public bool IsKeyPressed(char key) => _keyboard.IsKeyPressed(key);

    public bool IsKeyPressed(Key key) => _keyboard.IsKeyPressed(key);

    public bool IsKeyReleased(char key) => _keyboard.IsKeyReleased(key);

    public bool IsKeyReleased(Key key) => _keyboard.IsKeyReleased(key);

    public bool IsAnyKeyDown() => _keyboard.IsAnyKeyDown();

    public bool IsAnyKeyDown(out Key[] keys) => _keyboard.IsAnyKeyDown(out keys);

    public bool IsAnyKeyPressed() => _keyboard.IsAnyKeyPressed();

    public bool IsAnyKeyPressed(out Key[] keys) => _keyboard.IsAnyKeyPressed(out keys);

    public bool IsAnyKeyReleased() => _keyboard.IsAnyKeyReleased();

    public bool IsAnyKeyReleased(out Key[] keys) => _keyboard.IsAnyKeyReleased(out keys);

    public bool IsCapsLockOn() => _keyboard.IsCapsLockOn();

    public bool IsNumLockOn() => _keyboard.IsNumLockOn();

    public bool IsScrollLockOn() => _keyboard.IsScrollLockOn();

    public void KeyDown(char key) => _keyboard.KeyDown(key);

    public void KeyDown(Key Key) => _keyboard.KeyDown(Key);

    public void KeyUp(char key) => _keyboard.KeyUp(key);

    public void KeyUp(Key Key) => _keyboard.KeyUp(Key);

    public void KeyPress(char key) => _keyboard.KeyPress(key);

    public void KeyPress(Key Key) => _keyboard.KeyPress(Key);

    public void Write(string text) => _keyboard.Write(text);

    public void Copy() => _keyboard.Copy();

    public void Copy(string text) => _keyboard.Copy(text);

    public void Paste() => _keyboard.Paste();

    public void Paste(string text) => _keyboard.Paste(text);
    #endregion

    #region Mouse
    public Point GetMousePosition() => _mouse.GetMousePosition();

    public void SetMousePosition(int x, int y) => _mouse.SetMousePosition(x, y);

    public void SetMousePosition(Point position) => _mouse.SetMousePosition(position);

    public bool IsMouseDown(MouseButton button) => _mouse.IsMouseDown(button);

    public bool IsMouseUp(MouseButton button) => _mouse.IsMouseUp(button);

    public bool IsMousePressed(MouseButton button) => _mouse.IsMousePressed(button);

    public bool IsMouseReleased(MouseButton button) => _mouse.IsMouseReleased(button);

    public void MouseDown(MouseButton button) => _mouse.MouseDown(button);

    public void MouseDown(MouseButton button, Point position) => _mouse.MouseDown(button, position);

    public void MouseUp(MouseButton button) => _mouse.MouseUp(button);

    public void MouseUp(MouseButton button, Point position) => _mouse.MouseUp(button, position);

    public void ScrollMouseWheel(int wheelClickCount) => _mouse.ScrollMouseWheel(wheelClickCount);

    public void Click(MouseButton button) => _mouse.Click(button);

    public void Click(MouseButton button, Point position) => _mouse.Click(button, position);
    #endregion
}

