using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public interface ICommunicationProtocol
{
    void ConnectAsync();
    string GetMessage();
    public bool isConnected { get; set; }
}
public class TcpClientService : ICommunicationProtocol
{
    /*===================
    Public
    ===================*/
    public bool isConnected{ get; set; }

    /*===================
    Private
    ===================*/
    private readonly string _ipAddress;
    private readonly int _port;
    private TcpClient _client;
    private NetworkStream _stream;
  
    public string GetMessage()
    {
        return "Ahoj";
    }

    public TcpClientService(string ipAddress, int port)
    {
        _ipAddress = ipAddress;
        _port = port;
    }

    public async void ConnectAsync()
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
        }
    }
}

