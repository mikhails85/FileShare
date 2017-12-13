using IPFS.Desktop.Bridge.EventHandlers;
using Quobject.SocketIoClientDotNet.Client;
using System;
using IPFS.Utils.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace IPFS.Desktop.Bridge.AppConfig
{
    public class AppBridge
    {
        private static string SocketPort { get; set; }
        private static Socket socket;

        private static object syncRoot = new Object();

        private static ILogger<AppBridge> log = AppDI.ServiceProvider.GetService<ILogger<AppBridge>>();
        
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
                    
                    log.WarningMessage("Use Electron Port: " + SocketPort);
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
                                log.WarningMessage("Bridge connected!");
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
