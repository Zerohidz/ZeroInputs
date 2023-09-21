﻿namespace ZeroInputs;
public interface IMouse
{
    public void Update();
    public Point GetMousePosition();
    public void SetMousePosition(int x, int y);
    public void SetMousePosition(Point position);
    public bool IsMouseDown(MouseButton button);
    public bool IsMouseUp(MouseButton button);
    public bool IsMousePressed(MouseButton button);
    public bool IsMouseReleased(MouseButton button);
    public void MouseDown(MouseButton button);
    public void MouseDown(MouseButton button, Point position);
    public void MouseUp(MouseButton button);
    public void MouseUp(MouseButton button, Point position);
    public void ScrollMouseWheel(int wheelClickCount);
    public void Click(MouseButton button);
    public void Click(MouseButton button, Point position);
}
