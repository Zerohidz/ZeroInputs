using ZeroInputs.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace ZeroInputs;

public static class Extensions
{
    public static IServiceCollection AddZeroInputs(this IServiceCollection services)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("ZeroInputs is only supported on Windows for now.");

        services.AddSingleton<IMouse, WindowsMouse>();
        services.AddSingleton<IKeyboard, WindowsKeyboard>();
        var stateProvider = new WindowsInputStateProvider();
        services.AddSingleton<WindowsInputStateProvider>();
        services.AddSingleton<IInputStateProvider, WindowsInputStateProvider>(
            static (sp) => sp.GetRequiredService<WindowsInputStateProvider>()
        );
        services.AddSingleton<IInputService, InputService>();
        return services;
    }
}
