using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class StreamingService : ChatMessagesStreaming.ChatMessagesStreamingBase
    {
        public override async Task ChatMessagesStreaming(Empty request, IServerStreamWriter<ReceivedMessage> responseStream, ServerCallContext context)
        {
            int lastMessageId = -1;
            try
            {
                while (true)
                {
                    await Task.Delay(100);
                    int lastPosition = 0;

                    for (int index = 0; index < ChatData.QueueMessage.messages.Count(); index++)
                    {
                        if (ChatData.QueueMessage.messages[index].Id == lastMessageId)
                        {
                            lastPosition = index++;
                            break;
                        }
                    }
                    for (int index = lastPosition; index < ChatData.QueueMessage.messages.Count(); index++)
                    {
                        if (ChatData.QueueMessage.messages[index].Id != lastMessageId)
                        {
                            var receivedMessage = new ReceivedMessage()
                            {
                                User = ChatData.QueueMessage.messages[index].Username,
                                Message = ChatData.QueueMessage.messages[index].UserMessage
                            };
                            lastMessageId = ChatData.QueueMessage.messages[index].Id;
                            await responseStream.WriteAsync(receivedMessage);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message + "\n");
            }
        }
    }
}
