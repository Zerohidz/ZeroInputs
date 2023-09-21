using Microsoft.Extensions.DependencyInjection;
using ZeroInputs.Windows;

namespace ZeroInputs;
public static class DeviceProvider
{
    private static readonly IServiceProvider _serviceProvider;

    static DeviceProvider()
    {
        if (OperatingSystem.IsWindows())
        {
            _serviceProvider = new ServiceCollection()
                .AddTransient<IKeyStateReader, WindowsKeyStateReader>()
                .AddSingleton<IInputDevice, WindowsInputDevice>()
                .AddSingleton<IKeyboard, WindowsKeyboard>()
                .AddSingleton<IMouse, WindowsMouse>()
                .BuildServiceProvider();
        }
        else
        {
            throw new UnSupportedPlatformException();
        }
    }

    public static IInputDevice GetInputDevice()
        => _serviceProvider.GetRequiredService<IInputDevice>();

    public static IKeyboard GetKeyboard()
        => _serviceProvider.GetRequiredService<IKeyboard>();

    public static IMouse GetMouse()
        => _serviceProvider.GetRequiredService<IMouse>();
}
