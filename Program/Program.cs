using OES.Server;
using OES.Client;
using System.Diagnostics;

namespace OES.Program {
   public class Program {
        public static void Main(string[] args) {

            // Launching the server in a seperate terminal window
            ProcessStartInfo ServerStartInfo = new(); // object to launch new command prompt window
            ServerStartInfo.FileName = "cmd.exe"; // defining cmd as the terminal to run

            /*
             * "/C" tells the command prompt to execute the command and then terminate
             * "start cmd.exe /K" opens a new terminal window and keeps it open - "/K" does this
             * "dotnet run" is self-explanatory, runs the given project
            */
            ServerStartInfo.Arguments = $"/C start cmd.exe /K dotnet run --project C:\\Users\\Viktor\\Documents\\GitHub\\OddEvenServer\\Server\\Server.csproj";

            ServerStartInfo.RedirectStandardInput = false; // do not redirect input
            ServerStartInfo.RedirectStandardOutput = false; // do not redirect output
            ServerStartInfo.UseShellExecute = false; // do not use OS' defualt shell to start the process
            ServerStartInfo.CreateNoWindow = false; // do not hide prompt window

            Process Process = new(); // object to initate the process
            Process.StartInfo = ServerStartInfo; // process start information, defined in ServerStartInfo above
            Process.Start(); // starts the process with our given configurations

            //Giving time to launch server
            Thread.Sleep(2000);

            // Launching clients with 0,5s delay
            StartClientInNewTerminal();
            Thread.Sleep(500);

            StartClientInNewTerminal();
            Thread.Sleep(500);

            StartClientInNewTerminal();
            Thread.Sleep(500);

        }

        private static void StartClientInNewTerminal() {

            // Launching client in a seperate terminal window
            ProcessStartInfo ClientStartInfo = new(); // object to launch new command prompt window
            ClientStartInfo.FileName = "cmd.exe"; // defining cmd as the terminal to run

            /*
             * Same as with the server in Main()
            */
            ClientStartInfo.Arguments = $"/C start cmd.exe /K dotnet run --project C:\\Users\\Viktor\\Documents\\GitHub\\OddEvenServer\\Client\\Client.csproj";

            ClientStartInfo.RedirectStandardInput = false; //do not redirect user input
            ClientStartInfo.RedirectStandardOutput = false; //do not redirect user output
            ClientStartInfo.UseShellExecute = false; // do not use OS' defualt shell to start the process
            ClientStartInfo.CreateNoWindow = false; // do not hide prompt window

            Process ClientProcess = new(); // object to initate the process
            ClientProcess.StartInfo = ClientStartInfo; // process start information, defined in ClientStartINfo above
            ClientProcess.Start(); // starts the process with our given configurations
        }
    }
}