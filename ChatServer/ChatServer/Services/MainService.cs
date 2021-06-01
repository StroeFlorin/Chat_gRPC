using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class MainService : Chat.ChatBase
    {
        private readonly ILogger<MainService> _logger;
        public MainService(ILogger<MainService> logger)
        {
            _logger = logger;
        }
        public override Task<Empty> Login(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"\n[{DateTime.Now}]{request.User} has connected!\n");
            ChatData.QueueMessage.messages.Add(new ChatData.Message
            {
                Id = new Random().Next(0, int.MaxValue),
                Username = "SERVER",
                UserMessage = $"{request.User} has connected!"
            }
            );
            return Task.FromResult(new Empty());
        }
        public override Task<Empty> Logout(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"\n[{DateTime.Now}]{request.User} has disconnected!\n");
            ChatData.QueueMessage.messages.Add(new ChatData.Message
            {
                Id = new Random().Next(0, int.MaxValue),
                Username = "SERVER",
                UserMessage = $"{request.User} has disconnected!"
            }
            );
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> SendMessage(MessageInput request, ServerCallContext context)
        {
            _logger.LogInformation($"\n[{DateTime.Now}]{request.User}: {request.Message}\n");
            ChatData.QueueMessage.messages.Add(new ChatData.Message
            {
                Id = new Random().Next(0, int.MaxValue),
                Username = request.User,
                UserMessage = request.Message
            }
            );
            return Task.FromResult(new Empty());
        }
    }
}