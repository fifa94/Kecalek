using System;
using System.Net.Sockets;
using System.Security;
using System.Threading.Tasks;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.IO;

public interface ICommunicationProtocol
{
    Task ConnectAsync();
    Task SendMessageAsync(string message);
    bool isConnected { get; }
    void Disconnect();
}

public class TcpClientService : ICommunicationProtocol
{
    /*===================
    Public
    ===================*/
    public bool isConnected { get; private set; }

    /*===================
    Private
    ===================*/
    private readonly string _ipAddress;
    private readonly int _port;
    private TcpClient _client;
    private NetworkStream _stream;
    private Memory<byte> _encodedMessage;

    public TcpClientService(string ipAddress, int port)
    {
        _ipAddress = ipAddress;
        _port = port;
    }

    private void EncodeMessage(string message)
    {
        try
        {
            if (isConnected)
            {
                _encodedMessage = Encoding.Default.GetBytes(message);
            }
            else
            {
                throw new InvalidOperationException("Not connected to the server.");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Disconnect();
        }
    }

    public async Task ConnectAsync()
    {
        try
        {
            Console.WriteLine("Connecting to server...");
            _client = new TcpClient();
            await _client.ConnectAsync(_ipAddress, _port);
            _stream = _client.GetStream();
            Console.WriteLine($"Connected to server {_ipAddress}");
            isConnected = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failure: {ex.Message}");
            Disconnect();
        }
    }

    public void Disconnect()
    {
        _stream?.Close();
        _client?.Close();
        isConnected = false;
        Console.WriteLine($"Disconnected from server {_ipAddress}.");
    }

    public async Task SendMessageAsync(string message)
{
    if (string.IsNullOrEmpty(message))
    {
        Console.WriteLine("Error: Message is null or empty.");
        return;
    }

    try
    {
        EncodeMessage(message);
        await _stream.WriteAsync(_encodedMessage).ConfigureAwait(false);
        await _stream.FlushAsync().ConfigureAwait(false);
        Console.WriteLine("Sent message");
    }
    catch (Exception ex) when (
        ex is ArgumentNullException ||
        ex is ArgumentOutOfRangeException ||
        ex is ArgumentException ||
        ex is NotSupportedException ||
        ex is ObjectDisposedException ||
        ex is InvalidOperationException ||
        ex is IOException ||
        ex is System.Net.Sockets.SocketException)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Disconnect();
    }
}

}