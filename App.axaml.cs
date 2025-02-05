using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Kecalek.Services;

namespace Kecalek;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {

        // Vytvoření DI kontejneru
        var serviceProvider = new ServiceCollection()
            .AddSingleton<ICommunicationProtocol>(provider =>
                new TcpClientService("127.0.0.1", 12345)) // Registrace TCP/IP komunikace
            .AddSingleton<Communication>() // Registrace třídy Communication
            .AddTransient<MainWindow>() // Registrace MainWindow
            .BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}