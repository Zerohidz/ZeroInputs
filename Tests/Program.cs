using ZeroInputs.Windows;
using ZeroInputs.Windows.Enums;

namespace Tests;

internal class Program
{
    static void Main(string[] args)
    {
        InputApi api = new();

        while (true)
        {
            api.Update();

            if (api.IsAnyKeyJustBecameDown())
                Console.WriteLine("Herhangi bi tuşa basıldı");
        }
    }
}