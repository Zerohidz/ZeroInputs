using ZeroInputs.Windows;
using ZeroInputs.Windows.Enums;

namespace Tests;

internal class Program
{
    private static void Main(string[] args)
    {
        InputApi api = new();

        while (true)
        {
            api.Update();

            if (api.IsKeyPressed('ş'))
            {
                Console.WriteLine('ş');
            }
        }
    }
}