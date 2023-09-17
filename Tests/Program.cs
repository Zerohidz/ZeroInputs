using System.Runtime.InteropServices;
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

            if (api.IsKeyDown(KeyCode.A))
                Console.WriteLine("A");
        }
    }

    //[DllImport("user32.dll")]
    //private static extern bool GetKeyboardState(byte[] keys);

    //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    //public static extern short GetKeyState(int keyCode);

    //private static byte[] _keyboardStates = new byte[256];
    //static void Main(string[] args)
    //{
    //    while (true)
    //    {
    //        GetKeyState(1); // To activate GetKeyboardState()
    //        byte[] keyboardState = new byte[256];
    //        _keyboardStates.CopyTo(keyboardState, 0);
    //        GetKeyboardState(_keyboardStates);

    //        for (int i = 0; i < 256; i++)
    //        {
    //            if (_keyboardStates[i] != keyboardState[i])
    //                Console.WriteLine("Bi şeyler değişti");
    //        }
    //    }
    //}
}