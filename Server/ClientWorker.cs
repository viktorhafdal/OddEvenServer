using System.Net;
using System.Net.Sockets;

namespace OES.Server {
    public class ClientWorker {
        private int WorkerId;
        private TcpClient Connection;
        private ClientManager Manager;
        private string WorkerName;

        public ClientWorker(ClientManager manager, TcpClient connection, int workerId) {
            this.Manager = manager;
            this.Connection = connection;
            this.WorkerId = workerId;
            this.WorkerName = $"[ClientWorker #{workerId}]";
        }

        public void Run() {
            if (Connection?.Client.RemoteEndPoint is IPEndPoint endPoint) {
                Console.WriteLine($"{WorkerName} New connection from: " + ((IPEndPoint)Connection.Client.RemoteEndPoint).Address.ToString());
            } else {
                Console.WriteLine($"{WorkerName} Unable to connect to client endpoint!");
                return;
            }

            //Stream for two-way communication
            try {
                using StreamReader reader = new(Connection.GetStream()); // using writer to write to client
                using StreamWriter writer = new(Connection.GetStream()); // using writer to read client response

                bool firstOdd = true; // check if we've already had an odd number once
                bool firstEven = true; // check if we've already had an even number once

                string? line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.Equals("exit", StringComparison.OrdinalIgnoreCase)) {
                        Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - -");
                        Console.WriteLine($"{WorkerName} Server received EXIT. Shutting down connection.");
                        SendLine(writer, "Server received shutdown. Shutting down connection.");
                    }

                    //if (Int32.TryParse(line, out int lineNum)) { // tries to parse the line as an int, if succesful outputs the result as int lineNum
                    int lineNum = Int32.Parse(line);

                    if ((lineNum % 2) == 1) { // will always be an odd number, as an odd number divided by two will always leave one remainder
                        if (firstOdd) { // if it's the first odd number write this
                            ServerLogResponse(line, "odd"); // loggin to server
                            SendLine(writer, "odd"); // sending to client
                            firstOdd = false; // not first odd number from now on
                        } else { // else this
                            ServerLogResponse(line, "odd again");
                            SendLine(writer, "odd again");
                        }
                    }

                    if ((lineNum % 2) == 0) { // will always be an even number, as an even number divided by two will never leave a remainder
                        if (firstEven) { // if it's the first even number write this
                            ServerLogResponse(line, "even");
                            SendLine(writer, "even");
                            firstEven = false; // not first even number from now on
                        } else { // else this
                            ServerLogResponse(line, "even again");
                            SendLine(writer, "even again");
                        }
                    }
                }

            } catch (IOException e) {
                Console.Error.WriteLine($"{WorkerName} I/O error: {e.Message}");
            } finally {
                try { // attempting to close connection
                    Connection.Close();
                } catch (IOException e) {
                    Console.Error.WriteLine($"{WorkerName} Error when trying to close connection: {e.Message}");
                }
                Console.WriteLine($"{WorkerName} Connection closed"); // log closure to server
            }
        }

        // method to send line to client, using StreamWriter to send the messages
        private void SendLine(StreamWriter writer, string line) {
            writer.WriteLine(line); // writes the line to the writers buffer
            writer.Flush(); // clears the writers buffer and sends the line
        }

        private void ServerLogResponse(string input, string response) {
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - -");
            Console.WriteLine($"{WorkerName} Server received: " + input);
            Console.WriteLine($"{WorkerName} Sending line: " + response);
        }
    }
}