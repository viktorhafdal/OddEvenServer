using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace OES.Client {
    public class Client {
        private string host;
        private int port;

        public Client(string host, int port) {
            this.host = host;
            this.port = port;
        }

        public void Run() {
            try {
                using(TcpClient connection = new TcpClient(host, port))
                using(StreamWriter writer = new StreamWriter(connection.GetStream()))
                using(StreamReader reader = new StreamReader(connection.GetStream()))
                {
                    Console.WriteLine("[Client] Connected to " + host + ":" + port);
                    string inputLine;

                    while (true) {
                        Console.WriteLine("[Client] Enter a number: ");
                        inputLine = Console.ReadLine();

                        if (inputLine.Equals("EXIT", StringComparison.OrdinalIgnoreCase)) {
                            Console.WriteLine("[Client] Exiting...");
                            break;
                        }
                        SendLine(writer, inputLine);

                        string response = reader.ReadLine();
                        Console.WriteLine("[Client] Response from server: " + response);
                    }
                }
            } catch (IOException e) {
                Console.Error.WriteLine("[Client] Connection failed: " + e.Message);
            }
        }

        private void SendLine(StreamWriter writer, string line) {
            Console.WriteLine("[Client] sending line: " + line);
            writer.WriteLine(line);
            writer.Flush();
        }
    }
}