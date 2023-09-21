namespace ZeroInputs;

public interface IMouse
{
    public Point Position { get; }

    public void MoveTo(int x, int y);
    public void MoveTo(Point position);
    public bool IsButtonDown(MouseButton button);
    public bool IsButtonUp(MouseButton button);
    public bool IsButtonPressed(MouseButton button);
    public bool IsButtonReleased(MouseButton button);
    public void PressButton(MouseButton button);
    public void PressButton(MouseButton button, Point position);
    public void ReleaseButton(MouseButton button);
    public void ReleaseButton(MouseButton button, Point position);
    public void ClickButton(MouseButton button);
    public void ClickButton(MouseButton button, Point position);
    public void Scroll(int distance);
}
