using Quobject.SocketIoClientDotNet.Client;

namespace IPFS.Desktop.Bridge.EventHandlers
{
    public interface IEventHandler
    {
        void Registrate(Socket channel);
    }
}
