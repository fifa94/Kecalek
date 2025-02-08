using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kecalek.Services
{
    public class CommunicationManager
    {
        public bool IsConnected { get; private set; }
        private ICommunicationProtocol _protocol;
        private int _retryIntervalMilliseconds;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action<bool> ConnectionStatusChanged;

        public CommunicationManager(ICommunicationProtocol protocol)
        {
            _protocol = protocol;
        }

        public void ConnectToServer(int retryIntervalMilliseconds)
        {
            _retryIntervalMilliseconds = retryIntervalMilliseconds;
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => TryConnectLoop(_cancellationTokenSource.Token));
        }

        private async Task TryConnectLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (IsConnected) return;

                try
                {
                    await _protocol.ConnectAsync();
                    IsConnected = _protocol.isConnected;
                    ConnectionStatusChanged?.Invoke(IsConnected);

                    if (IsConnected)
                    {
                        Console.WriteLine("Připojení úspěšné!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"Pokus o pripojeni neuspesny, zkousim znovu za {_retryIntervalMilliseconds} ms");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Chyba při pokusu o připojení: {ex.Message}");
                    IsConnected = false;
                    ConnectionStatusChanged?.Invoke(IsConnected);
                }

                await Task.Delay(_retryIntervalMilliseconds, cancellationToken);
            }
        }

        public void StopConnecting()
        {
            _cancellationTokenSource?.Cancel();
            IsConnected = false;
            ConnectionStatusChanged?.Invoke(IsConnected);
        }

        public void SendMessage(string message)
        {
            _protocol.SendMessageAsync(message);
        }
    }
}