namespace OES.Main {
    using OES.Server;
    public class Program {
        public static void Main(string[] args) {
            ClientManager manager = new ClientManager(6010);
            manager.Run();

        }
    }
}