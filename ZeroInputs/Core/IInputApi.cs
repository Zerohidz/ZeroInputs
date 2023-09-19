using ZeroInputs.Core.DataContainers;
using ZeroInputs.Windows.Enums;

namespace ZeroInputs.Core;

public interface IInputApi
{
    public Point GetMousePosition();
    public void SetMousePosition(int x, int y);
    public void SetMousePosition(Point position);
    public void LeftUp();
    public void LeftDown();
    public void LeftClick();
    public void LeftClick(Point position);
    public void MiddleUp();
    public void MiddleDown();
    public void MiddleClick();
    public void MiddleClick(Point position);
    public void RightUp();
    public void RightDown();
    public void RightClick();
    public void RightClick(Point position);
    public bool IsKeyUp(char key);
    public bool IsKeyUp(KeyCode key);
    public bool IsKeyDown(char key);
    public bool IsKeyDown(KeyCode key);
    public bool IsKeyReleased(char key);
    public bool IsKeyReleased(KeyCode key);
    public bool IsKeyPressed(char key);
    public bool IsKeyPressed(KeyCode key);
    public bool IsAnyKeyDown();
    public bool IsAnyKeyDown(out KeyCode[] keys);
    public bool IsAnyKeyReleased();
    public bool IsAnyKeyReleased(out KeyCode[] keys);
    public bool IsAnyKeyPressed();
    public bool IsAnyKeyPressed(out KeyCode[] keys);
    public bool IsCapsLockOn();
    public bool IsNumLockOn();
    public bool IsScrollLockOn();
    public bool IsCtrlDown();
    public bool IsShiftDown();
    public bool IsAltDown();

    public void Copy();
    public void Copy(string text);

    public void Paste();
    public void Paste(string text);
}
