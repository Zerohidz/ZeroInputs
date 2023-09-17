using ZeroInputs.Windows;
using ZeroInputs.Windows.Enums;

namespace Tests;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Press Any Key");

        InputApi api = new();

        while (true)
        {
            api.Update();

            if (api.IsKeyDown(KeyCode.A))
                Console.WriteLine(KeyCode.A);

            if (api.IsCtrlDown() && api.IsKeyJustBecameDown(KeyCode.S))
                Console.WriteLine("Kaydedildi");
        }
    }
}