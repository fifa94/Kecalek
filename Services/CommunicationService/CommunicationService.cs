
using System;
using System.Runtime.CompilerServices;

namespace Kecalek.Services
{
    public class Communication
    {
        /*===================
        Public
        ===================*/
        public bool isConnected { get; set; }
        /*===================
        Private
        ===================*/
        private readonly ICommunicationProtocol _protocol;

        public Communication(ICommunicationProtocol protocol)
        {
            Console.WriteLine("Communication class constructor");
            _protocol = protocol;
        }

        public void connectToServer()
        {
            _protocol.ConnectAsync();

            isConnected = _protocol.isConnected;
        }
    }
}