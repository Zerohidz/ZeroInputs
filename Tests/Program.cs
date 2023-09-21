using ZeroInputs;
using ZeroInputs.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

internal class Program
{
    private static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        IServiceProvider provider = services
            .AddTransient<KeyStateReader, KeyStateReader>()
            .AddSingleton<IKeyboard, WindowsKeyboard>()
            .AddSingleton<IMouse, WindowsMouse>()
            .BuildServiceProvider();

        IKeyboard keyboard = provider.GetRequiredService<IKeyboard>();
        IMouse mouse = provider.GetRequiredService<IMouse>();

        while (true)
        {
            keyboard.Update();
            mouse.Update();

            if (keyboard.IsKeyReleased(Key.I))
            {
                keyboard.Write("Test");
                keyboard.KeyPress(Key.Enter);
            }
        }
    }
}