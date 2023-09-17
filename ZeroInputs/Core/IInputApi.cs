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
    public bool IsKeyJustBecameUp(char key);
    public bool IsKeyJustBecameUp(KeyCode key);
    public bool IsKeyJustBecameDown(char key);
    public bool IsKeyJustBecameDown(KeyCode key);
    public bool IsAnyKeyDown();
    public bool IsAnyKeyDown(out KeyCode[] keys);
    public bool IsAnyKeyJustBecameUp();
    public bool IsAnyKeyJustBecameUp(out KeyCode[] keys);
    public bool IsAnyKeyJustBecameDown();
    public bool IsAnyKeyJustBecameDown(out KeyCode[] keys);
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
