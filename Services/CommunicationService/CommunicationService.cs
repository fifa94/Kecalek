
namespace Kecalek.Services
{
    public class Communication
    {
        private readonly ICommunicationProtocol _protocol;

        public Communication(ICommunicationProtocol protocol)
        {
            _protocol = protocol;
        }
    }
}