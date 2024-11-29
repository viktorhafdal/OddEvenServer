namespace OES.Program {
    using OES.Client;
    public class Program {
        public static void Main(string[] args) {
            Client client = new Client("127.0.0.1", 6010);
            client.Run();

            Client client2 = new Client("127.0.0.1", 6010);
            client2.Run();
        }
    }
}