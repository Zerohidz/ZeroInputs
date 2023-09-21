namespace ZeroInputs;
public interface IInputDevice : IMouse, IKeyboard
{
    public new void Update();
}
