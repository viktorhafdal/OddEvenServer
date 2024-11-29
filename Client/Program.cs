namespace OES.Client {
    public class Program {
        public static void Main(string[] args) {
            Client client = new("127.0.0.1", 6010);
            client.Run();
        }
    }
}