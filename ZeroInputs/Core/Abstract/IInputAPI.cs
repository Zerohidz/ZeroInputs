using ZeroInputs.Core.DataContainers;
using ZeroInputs.Core.Enums;

namespace ZeroInputs.Core.Abstract;

public interface IInputAPI
{
    public MousePoint GetMousePosition();
    public MousePoint SetMousePosition(int x, int y);
    public MousePoint SetMousePosition(MousePoint position);
    public void LeftClick();
    public void RightClick();
    public void MiddleClick();
    public void LeftClick(MousePoint position);
    public void RightClick(MousePoint position);
    public void MiddleClick(MousePoint position);
    public bool IsKeyUp(char key);
    public bool IsKeyDown(char key);
    public bool IsKeyJustBecameUp(char key);
    public bool IsKeyJustBecameDown(char key);
    public bool IsKeyUp(KeyCode key);
    public bool IsKeyDown(KeyCode key);
    public bool IsKeyJustUp(KeyCode key);
    public bool IsKeyJustDown(KeyCode key);
    public bool IsCapsOn();
    public bool IsCtrlDown();
    public bool IsShiftDown();

    public void Copy();
    public void Copy(string text);

    public void Paste();
    public void Paste(string text);
}
