﻿using ZeroInputs;

namespace Tests;

internal class Program
{
    private static void Main(string[] args)
    {
        IInputDevice device = DeviceProvider.GetInputDevice();
        Console.WriteLine("Ready!");

        while (true)
        {
            device.Update();

            if (device.IsKeyReleased(Key.I))
            {
                device.Write("Test");
                device.KeyPress(Key.Enter);
            }
        }
    }
}