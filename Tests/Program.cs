using ZeroInputs.Core.Enums;
using ZeroInputs.Windows;

namespace Tests;

internal class Program
{
    static void Main(string[] args)
    {
        InputApi api = new();
        while (true)
        {
            api.Update();

            if (api.IsShiftDown() && api.IsKeyJustBecameDown(KeyCode.S))
            {
                api.KeyPress(KeyCode.Backspace);
                api.Type("Ali Hakan Merabalar");
            }
        }
    }
}