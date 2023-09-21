using ZeroInputs;
using ZeroInputs.Windows;

namespace Tests;

internal class Program
{
    private static void Main(string[] args)
    {
        WindowsInputDevice device = new();
        WindowsMouse mouse = new();

        while (true)
        {
            device.Update();

            if (device.IsKeyReleased(Key.LeftMouseButton))
            {
                Console.WriteLine("asdf");
                mouse.MouseDown(MouseButton.Left, (1200, 600));
            }
        }
    }
}