using ZeroInputs;
using ZeroInputs.Windows;

namespace Tests;

internal class Program
{
    private static void Main(string[] args)
    {
        WindowsInputDevice device = new();

        while (true)
        {
            device.Update();

            if (device.IsKeyDown(Key.LeftMouseButton))
                Console.WriteLine("Left Mouse " + device.GetMousePosition());

            if (device.IsKeyPressed(Key.RightMouseButton))
                Console.WriteLine("Right Mouse Pressed");

            if (device.IsKeyReleased(Key.MiddleMouseButton))
                Console.WriteLine("Middle Mouse Released");

            if (device.IsKeyPressed('Ş'))
            {
                device.KeyDown(Key.Control);
                device.KeyPress('S');
                device.KeyUp(Key.Control);
            }
        }
    }
}