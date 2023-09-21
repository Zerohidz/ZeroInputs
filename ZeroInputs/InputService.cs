namespace ZeroInputs;

internal class InputService : IInputService
{
    private readonly IInputStateProvider _inputStateProvider;

    public IMouse Mouse { get; }
    public IKeyboard Keyboard { get; }

    public InputService(IMouse mouse, IKeyboard keyboard, IInputStateProvider inputStateProvider)
    {
        Mouse = mouse;
        Keyboard = keyboard;
        _inputStateProvider = inputStateProvider;
    }

    public void Update()
    {
        _inputStateProvider.Update();
    }
}
