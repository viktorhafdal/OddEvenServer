using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OES.Server {
    public class ClientManager {
        private List<ClientWorker> workers = new List<ClientWorker>();
        private int port;
        private bool stop = false;

        public ClientManager(int port) {
            this.port = port;
            this.stop = false;
        }

        public void Run() {
            TcpListener serverSocket = null;
            try {
                serverSocket = new TcpListener(IPAddress.Any, port);
                serverSocket.Start();
                Console.WriteLine("[ClientManager] Server online");

                while (!stop) {
                    try {
                        if (serverSocket.Pending()) {
                            TcpClient connection = serverSocket.AcceptTcpClient();
                            ClientWorker worker = new ClientWorker(this, connection);
                            workers.Add(worker);

                            //Starts ClientWorker on a new thread
                            Task.Run(() => worker.Run());
                        } else {
                            Thread.Sleep(100); //Small timeout to be able to check for pending
                        }
                    } catch (SocketException e) {
                        Console.Error.WriteLine("[ClientManager] Socket error: " + e.Message);
                    }
                }

                Console.WriteLine("[ClientManager] Server offline");

            } catch (IOException e) {
                Console.Error.WriteLine("[ClientManager] I/O error: " + e.Message);
            } finally {
                if (serverSocket != null) {
                    serverSocket.Stop();
                }
            }
        }

        public void Shutdown() {
            stop = true;
        }
    }
}