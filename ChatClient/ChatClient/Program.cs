using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading;
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

            new Thread(async () =>
            {
                var client2 = new ChatMessagesStreaming.ChatMessagesStreamingClient(channel);
                var dataStream = client2.ChatMessagesStreaming(new Empty());
                await foreach (var messageData in dataStream.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine($"[{DateTime.Now}]{messageData.User}: {messageData.Message}");
                }
            }).Start();

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
