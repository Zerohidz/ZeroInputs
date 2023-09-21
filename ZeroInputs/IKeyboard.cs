namespace ZeroInputs;
public interface IKeyboard
{
    public void Update();
    public bool IsKeyDown(char key);
    public bool IsKeyDown(Key key);
    public bool IsKeyUp(char key);
    public bool IsKeyUp(Key key);
    public bool IsKeyPressed(char key);
    public bool IsKeyPressed(Key key);
    public bool IsKeyReleased(char key);
    public bool IsKeyReleased(Key key);
    public bool IsAnyKeyDown();
    public bool IsAnyKeyDown(out Key[] keys);
    public bool IsAnyKeyPressed();
    public bool IsAnyKeyPressed(out Key[] keys);
    public bool IsAnyKeyReleased();
    public bool IsAnyKeyReleased(out Key[] keys);
    public bool IsCapsLockOn();
    public bool IsNumLockOn();
    public bool IsScrollLockOn();
    public void KeyDown(char key);
    public void KeyDown(Key Key);
    public void KeyUp(char key);
    public void KeyUp(Key Key);
    public void KeyPress(char key);
    public void KeyPress(Key Key);
    public void Write(string text);

    public void Copy();
    public void Copy(string text);

    public void Paste();
    public void Paste(string text);
}
