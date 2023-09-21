using Microsoft.Extensions.DependencyInjection;
using ZeroInputs;

namespace Tests;

internal class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddZeroInputs()
            .BuildServiceProvider();

        IInputService input = services.GetRequiredService<IInputService>();
        IKeyboard keyboard = services.GetRequiredService<IKeyboard>();

        Console.WriteLine("Ready!");

        while (true)
        {
            input.Update();

            if (keyboard.IsKeyReleased(Key.I))
            {
                keyboard.PressKey(Key.Control);
                keyboard.SendKey(Key.S);
                keyboard.ReleaseKey(Key.Control);
            }
        }
    }
}