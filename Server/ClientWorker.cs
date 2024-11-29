using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OES.Server {
    public class ClientWorker {
        private static int IdCounter;
        private int WorkerId;
        private TcpClient Connection;
        private ClientManager Manager;
        private string WorkerName;

        public ClientWorker(ClientManager manager, TcpClient connection) {
            this.Manager = manager;
            this.Connection = connection;
            this.WorkerId = WorkerId++;
            this.WorkerName = $"[ClientWorker #{WorkerId}]";
        }

        public void Run() {
            Console.WriteLine($"{WorkerName} New connection from: " + ((IPEndPoint)Connection.Client.RemoteEndPoint).Address.ToString());

            //Stream for two-way communication
            try {
                using (StreamReader reader = new StreamReader(Connection.GetStream()))
                using (StreamWriter writer = new StreamWriter(Connection.GetStream()))
                {
                    string line;

                    bool firstOdd = true;
                    bool firstEven = true;

                    while ((line = reader.ReadLine()) != null) {
                        int lineNum = Int32.Parse(line);

                        if ((lineNum % 2) == 1) {
                            if (firstOdd) {
                                SendLine(writer, $"{WorkerName} odd");
                                firstOdd = false;
                            } else {
                                SendLine(writer, $"{WorkerName} odd again");
                            }
                        }

                        if ((lineNum % 2) == 0) {
                            if (firstEven) {
                                SendLine(writer, $"{WorkerName} even");
                                firstEven = false;
                            } else {
                                SendLine(writer, $"{WorkerName} even again");
                            }
                        }
                    }
                }
            } catch (IOException e) {
                Console.Error.WriteLine($"{WorkerName} I/O error: {e.Message}");
            } finally {
                try {
                    Connection.Close();
                } catch (IOException e) {
                    Console.Error.WriteLine($"{WorkerName} Error when trying to close connection: {e.Message}");
                }
                Console.WriteLine($"{WorkerName} Connection closed");
            }
        }

        private void SendLine(StreamWriter writer, string line) {
            writer.WriteLine(line);
            writer.Flush();
        }
    }
}