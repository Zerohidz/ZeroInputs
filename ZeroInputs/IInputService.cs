namespace ZeroInputs;

public interface IInputService
{
    public IMouse Mouse { get; }
    public IKeyboard Keyboard { get; }
    public void Update();
}
