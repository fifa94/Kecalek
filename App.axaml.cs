using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kecalek.Services;
using System;

namespace Kecalek;

public partial class App : Application
{
    private Kecalek.Services.CommunicationManager _communicationService;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Console.WriteLine("Před připojením k serveru");

        var protocol = new TcpClientService("127.0.0.1", 8888);
        _communicationService = new Kecalek.Services.CommunicationManager(protocol);

        _communicationService.ConnectionStatusChanged += (isConnected) => {
            if (isConnected) {
                Console.WriteLine("Připojeno k serveru!");
                // Zde může být kód pro zahájení komunikace, např. odesílání zpráv.
            } else {
                Console.WriteLine("Odpojeno od serveru!");
                // Zde může být kód pro zpracování odpojení, např. zobrazení hlášky uživateli.
            }
        };

        _communicationService.ConnectToServer(1000); // Spustí opakované pokusy o připojení

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.Closing += MainWindow_Closing;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _communicationService?.StopConnecting();
        Console.WriteLine("Aplikace se ukončuje (MainWindow Closing)...");
    }
}