using IPFS.Desktop.Bridge.EventHandlers;
using Quobject.SocketIoClientDotNet.Client;
using System;

namespace IPFS.Desktop.Bridge.AppConfig
{
    public static class AppBridge
    {
        private static string SocketPort { get; set; }
        private static Socket socket;

        private static object syncRoot = new Object();

        public static void SetupPort(string[] args)
        {
            ParseSocketPort(args);
        }

        public static void RegistrateEventHandler(IEventHandler handler)
        {
            handler.Registrate(Socket);
        }

        private static void ParseSocketPort(string[] args)
        {
            foreach (string argument in args)
            {
                if (argument.ToUpper().Contains("ELECTRONPORT"))
                {
                    SocketPort = argument.ToUpper().Replace("/ELECTRONPORT=", "");
                    Console.WriteLine("Use Electron Port: " + SocketPort);
                }
            }
        }

        private static Socket Socket
        {
            get
            {
                if (socket == null && !string.IsNullOrWhiteSpace(SocketPort))
                {
                    lock (syncRoot)
                    {
                        if (socket == null && !string.IsNullOrWhiteSpace(SocketPort))
                        {
                            socket = IO.Socket("http://localhost:" + SocketPort);
                            socket.On(Socket.EVENT_CONNECT, () =>
                            {
                                Console.WriteLine("BridgeConnector connected!");
                            });
                        }
                    }
                }
                else if (socket == null && string.IsNullOrWhiteSpace(SocketPort))
                {
                    lock (syncRoot)
                    {
                        if (socket == null && string.IsNullOrWhiteSpace(SocketPort))
                        {
                            socket = IO.Socket(new Uri("http://localhost"), new IO.Options { AutoConnect = false });
                        }
                    }
                }

                return socket;
            }
        }
    }
}
