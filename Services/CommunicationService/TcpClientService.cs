using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public interface ICommunicationProtocol
{
    string GetMessage();
}
public class TcpClientService : ICommunicationProtocol
{
    private readonly string _ipAddress;
    private readonly int _port;

    public string GetMessage(){
        return "Ahoj";
    }

    public TcpClientService(string ipAddress, int port)
    {
        _ipAddress = ipAddress;
        _port = port;
        Console.WriteLine("Initialized tcp client communication");
    }
}

