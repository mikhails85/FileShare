using Quobject.SocketIoClientDotNet.Client;

namespace IPFS.Desktop.Bridge.EventHandlers
{
    public class ExampleEventHandler : IEventHandler
    {
        public void Registrate(Socket channel)
        {
            channel.On("Api:SeyHello", (data) =>
            {
                channel.Emit("App:SeyHello", "Hello I am Api !");
            });
        }
    }
}
