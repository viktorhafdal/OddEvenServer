using System.Net;
using System.Net.Sockets;

namespace OES.Server {
    public class ClientManager {
        private List<ClientWorker> Workers = [];
        private int WorkerIdCounter;
        private int Port;
        private bool Stop = false;

        public ClientManager(int port) {
            this.Port = port;
            this.Stop = false;
        }

        public void Run() {
            TcpListener? serverSocket = null; // creating a listener to initialise in try/catch

            try {
                serverSocket = new TcpListener(IPAddress.Any, Port); // initialising the listener to listen for incoming connections from any IP address that tries contacting on the given port
                serverSocket.Start(); // start the listener

                Console.WriteLine("[ClientManager] Server online");

                // while shutdown has not been given, listen for incoming connections
                while (!Stop) {
                    try {
                        if (serverSocket.Pending()) { // true if there is a connection waiting to connect
                            TcpClient connection = serverSocket.AcceptTcpClient(); // initialises the pending connection as a client object

                            int workerId = ++this.WorkerIdCounter;
                            ClientWorker worker = new(this, connection, workerId); // initialising a worker to handle the new connection, assigning the manager, the client and workerID
                            Workers.Add(worker);

                            Task.Run(() => worker.Run()); // starts ClientWorker on a new thread
                        } else {
                            Thread.Sleep(100); // small timeout before checking for pending connections again, so the server is not constantly checking
                        }
                    } catch (SocketException e) {
                        Console.Error.WriteLine("[ClientManager] Socket error: " + e.Message);
                    }
                }

                Console.WriteLine("[ClientManager] Server offline");

            } catch (IOException e) {
                Console.Error.WriteLine("[ClientManager] I/O error: " + e.Message);
            } finally {
                serverSocket?.Stop(); // stop the listener again, once we want to shutdown
            }
        }

        public void Shutdown() {
            Stop = true;
        }
    }
}