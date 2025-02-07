using System;
using System.Net.Sockets;
using System.Security;
using System.Threading.Tasks;

public interface ICommunicationProtocol
{
    Task ConnectAsync();
    string GetMessage();
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

    public TcpClientService(string ipAddress, int port)
    {
        _ipAddress = ipAddress;
        _port = port;
    }

    public string GetMessage()
    {
        return "Ahoj";
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
            isConnected = false;
            Console.WriteLine($"Connection failure: {ex.Message}");
            Disconnect();
        }
    }

    public void Disconnect()
    {
        _stream?.Close();
        _client?.Close();
        isConnected = false;
        Console.WriteLine("Disconnected from server.");
    }
}