namespace ZeroInputs;

public interface IKeyboard
{
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
    public void PressKey(char key);
    public void PressKey(Key Key);
    public void ReleaseKey(char key);
    public void ReleaseKey(Key Key);
    public void SendKey(char key);
    public void SendKey(Key Key);
    public void Write(string text);
}
