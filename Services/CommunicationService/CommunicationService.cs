
using System;

namespace Kecalek.Services
{
    public class Communication
    {
        private readonly ICommunicationProtocol _protocol;

        public Communication(ICommunicationProtocol protocol)
        {
            Console.WriteLine("Communication constructor");
            _protocol = protocol;
        }
    }
}