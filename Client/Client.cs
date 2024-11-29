using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace OES.Client {
    public class Client {
        private readonly string host;
        private readonly int port;

        public Client(string host, int port) {
            this.host = host;
            this.port = port;
        }

        public void Run() {
            try {
                using TcpClient connection = new(host, port); // initialising client and connecting to host:port
                using StreamWriter writer = new(connection.GetStream()); // initialising writer to be able to write to server
                using StreamReader reader = new(connection.GetStream()); // initialising reader to be able to read from server
                Console.WriteLine("[CLIENT] Connected to " + host + ":" + port);

                // Continue running until false or break
                while (true) {
                    SleepOutput(); // sleeping between console writes, to make output easier to read for the user
                    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - -");

                    SleepOutput();
                    Console.WriteLine("[CLIENT] Enter a number (or exit by writing EXIT): ");

                    string? inputLine = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(inputLine)) { // null handling for user input
                        Console.WriteLine("[CLIENT] Input cannot be empty. Please try again.");
                        continue;
                    }

                    // user wishes to exit the connection
                    if (inputLine.Equals("EXIT", StringComparison.OrdinalIgnoreCase)) {
                        SendLine(writer, inputLine); // sending exit to server
                        string? exitResponse = reader.ReadLine();
                        Console.WriteLine("[SERVER] " + exitResponse); // reading and writing server response
                        Console.WriteLine("[CLIENT] Exiting...");
                        break; // break out of loop, closing the connection
                    }

                    // send user input (not null here)
                    SendLine(writer, inputLine); // sending user line to server

                    string? response = reader.ReadLine(); // reading response from server
                    if (response == null) { // null handling for response
                        Console.WriteLine("[CLIENT] No response from server. Server might have closed the conneciton. Try reconnecting.");
                        break;
                    }

                    // read server response (not null here)
                    SleepOutput();
                    Console.WriteLine("[SERVER] " + response);
                }
            } catch (IOException e) {
                Console.Error.WriteLine("[CLIENT] Connection failed: " + e.Message);
            } catch (Exception e) {
                Console.Error.WriteLine("[CLIENT] Unexpected error: " + e.Message);
            }
        }

        // method to send line to server, using StreamWriter to send the messages
        private void SendLine(StreamWriter writer, string line) {
            SleepOutput();
            Console.WriteLine("[CLIENT] sending line: " + line); // sleeping to make console output easier to read for the user
            writer.WriteLine(line); // writes the line to the writers buffer
            writer.Flush(); // clears the writers buffer and sends the line
        }

        // method to sleep to make terminal output easier readable to the user
        private void SleepOutput() {
            Thread.Sleep(500);
        }
    }
}