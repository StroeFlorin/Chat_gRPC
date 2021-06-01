using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            String username;

            Console.Write("What's your name? ");
            username = Console.ReadLine();
            var client = new Chat.ChatClient(channel);
            var reply = await client.LoginAsync(new UserRequest { User = username });
            Console.Clear();
            Console.WriteLine("You are now connected! Say something...");

            while (true)
            {
                String message = Console.ReadLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                if (message.ToUpper().Equals("/EXIT"))
                {
                    reply = await client.LogoutAsync(new UserRequest { User = username });
                    System.Environment.Exit(1);
                }
                else
                {
                    reply = await client.SendMessageAsync(new MessageInput { User = username, Message = message });
                }

            }
        }
    }
}
