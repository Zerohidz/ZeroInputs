using ZeroInputs.Windows;

namespace Tests;

internal class Program
{
    static void Main(string[] args)
    {
        InputApi api = new();
        api.Copy("asdf");
    }
}
